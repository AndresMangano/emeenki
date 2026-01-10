using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Hermes.Core.Services
{
    public class YouTubeService
    {
        private readonly string _tempDirectory;

        public YouTubeService(string tempDirectory = null)
        {
            _tempDirectory = tempDirectory ?? Path.Combine(Path.GetTempPath(), "hermes_youtube");
            Directory.CreateDirectory(_tempDirectory);
        }

        public async Task<YouTubeVideoData> FetchVideoDataAsync(string youtubeUrl)
        {
            var videoId = ExtractVideoId(youtubeUrl);
            if (string.IsNullOrEmpty(videoId))
                throw new DomainException("Invalid YouTube URL");

            var videoDir = Path.Combine(_tempDirectory, videoId);
            Directory.CreateDirectory(videoDir);

            try
            {
                // First, get video info using -J flag
                var infoJson = await RunYtDlpAsync(youtubeUrl, videoDir, "--dump-json --no-warnings");
                var videoInfo = JsonDocument.Parse(infoJson);

                var title = videoInfo.RootElement.GetProperty("title").GetString();
                var thumbnailUrl = videoInfo.RootElement.GetProperty("thumbnail").GetString();
                var defaultLanguage = GetDefaultLanguage(videoInfo);

                // Download captions
                await RunYtDlpAsync(
                    youtubeUrl,
                    videoDir,
                    "--write-subs --write-auto-subs --skip-download --sub-format vtt --no-warnings"
                );

                // Parse captions
                var captions = ParseCaptions(videoDir, videoId, defaultLanguage);

                return new YouTubeVideoData
                {
                    Title = title,
                    ThumbnailUrl = thumbnailUrl,
                    VideoUrl = $"https://www.youtube.com/watch?v={videoId}",
                    Captions = captions
                };
            }
            finally
            {
                // Cleanup
                if (Directory.Exists(videoDir))
                {
                    try
                    {
                        Directory.Delete(videoDir, true);
                    }
                    catch
                    {
                        // Ignore cleanup errors
                    }
                }
            }
        }

        private string ExtractVideoId(string url)
        {
            // Extract video ID from various YouTube URL formats
            var patterns = new[]
            {
                @"(?:youtube\.com\/watch\?v=|youtu\.be\/)([a-zA-Z0-9_-]{11})",
                @"youtube\.com\/embed\/([a-zA-Z0-9_-]{11})"
            };

            foreach (var pattern in patterns)
            {
                var match = Regex.Match(url, pattern);
                if (match.Success) return match.Groups[1].Value;
            }

            return null;
        }

        private string GetDefaultLanguage(JsonDocument videoInfo)
        {
            try
            {
                // Try to get language from video metadata
                if (videoInfo.RootElement.TryGetProperty("language", out var langProp))
                {
                    return langProp.GetString()?.Split('-')[0];
                }
            }
            catch
            {
                // Ignore errors
            }

            return "en";
        }

        private async Task<string> RunYtDlpAsync(string url, string workingDir, string arguments)
        {
            var processInfo = new ProcessStartInfo
            {
                FileName = "yt-dlp",
                Arguments = $"{arguments} \"{url}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = workingDir
            };

            using var process = new Process { StartInfo = processInfo };

            process.Start();

            var output = await process.StandardOutput.ReadToEndAsync();
            var error = await process.StandardError.ReadToEndAsync();

            await process.WaitForExitAsync();

            if (process.ExitCode != 0 && !string.IsNullOrEmpty(error))
            {
                throw new DomainException($"yt-dlp error: {error}");
            }

            return output;
        }

        private List<CaptionCue> ParseCaptions(string directory, string videoId, string defaultLanguage)
        {
            // Look for caption files in order of preference:
            // 1. Manual captions in default language
            // 2. Manual captions in any language
            // 3. Auto-generated captions in default language
            // 4. Auto-generated captions in any language

            var vttFiles = Directory.GetFiles(directory, "*.vtt");
            if (vttFiles.Length == 0)
                throw new DomainException("No captions available for this video");

            // Try to find manual captions in default language
            var manualCaption = vttFiles.FirstOrDefault(f =>
                f.Contains($".{defaultLanguage}.vtt", StringComparison.OrdinalIgnoreCase) &&
                !f.Contains("-auto", StringComparison.OrdinalIgnoreCase)
            );

            // If not found, try any manual caption
            if (manualCaption == null)
                manualCaption = vttFiles.FirstOrDefault(f =>
                    !f.Contains("-auto", StringComparison.OrdinalIgnoreCase)
                );

            // If not found, try auto-generated in default language
            var captionFile = manualCaption ?? vttFiles.FirstOrDefault(f =>
                f.Contains($".{defaultLanguage}.vtt", StringComparison.OrdinalIgnoreCase)
            );

            // If still not found, use the first available
            captionFile = captionFile ?? vttFiles[0];

            var cues = ParseVttFile(captionFile);

            // Heuristic: if this looks like the auto-generated "progressive" pattern
            // (lots of tiny micro-cues at the start), apply the 1,5,9,13… filter
            // AND normalize the start times as requested.
            if (LooksLikeProgressiveAutoPattern(cues))
            {
                cues = ApplyEveryFourthPattern(cues);
                cues = NormalizeProgressiveStartTimes(cues);
            }

            return cues;
        }

        private List<CaptionCue> ParseVttFile(string vttFilePath)
        {
            var cues = new List<CaptionCue>();
            var lines = File.ReadAllLines(vttFilePath);

            var inCue = false;
            CaptionCue currentCue = null;
            var textLines = new List<string>();

            foreach (var line in lines)
            {
                // Skip WEBVTT header and empty lines at the start
                if (line.StartsWith("WEBVTT") || line.StartsWith("Kind:") || line.StartsWith("Language:"))
                    continue;

                // Check for timestamp line (format: 00:00:00.000 --> 00:00:05.000)
                var timestampMatch = Regex.Match(
                    line,
                    @"(\d{2}:\d{2}:\d{2}\.\d{3})\s+-->\s+(\d{2}:\d{2}:\d{2}\.\d{3})"
                );

                if (timestampMatch.Success)
                {
                    // Save previous cue if exists
                    if (currentCue != null && textLines.Count > 0)
                    {
                        currentCue.Text = string.Join(" ", textLines).Trim();
                        if (!string.IsNullOrEmpty(currentCue.Text))
                            cues.Add(currentCue);
                    }

                    // Start new cue
                    currentCue = new CaptionCue
                    {
                        StartTimeMs = ParseTimestamp(timestampMatch.Groups[1].Value),
                        EndTimeMs = ParseTimestamp(timestampMatch.Groups[2].Value)
                    };

                    textLines.Clear();
                    inCue = true;
                }
                else if (inCue && !string.IsNullOrWhiteSpace(line))
                {
                    // Remove VTT tags like <c>, </c>, etc.
                    var cleanLine = Regex.Replace(line, @"<[^>]+>", "");
                    textLines.Add(cleanLine);
                }
                else if (string.IsNullOrWhiteSpace(line) && inCue)
                {
                    // End of current cue
                    if (currentCue != null && textLines.Count > 0)
                    {
                        currentCue.Text = string.Join(" ", textLines).Trim();
                        if (!string.IsNullOrEmpty(currentCue.Text))
                            cues.Add(currentCue);
                    }

                    textLines.Clear();
                    inCue = false;
                }
            }

            // Add last cue if exists
            if (currentCue != null && textLines.Count > 0)
            {
                currentCue.Text = string.Join(" ", textLines).Trim();
                if (!string.IsNullOrEmpty(currentCue.Text))
                    cues.Add(currentCue);
            }

            return cues;
        }

        /// <summary>
        /// Detect the “auto-generated progressive caption” pattern:
        /// many very short (micro) cues at the beginning, like 10–50ms.
        /// </summary>
        private bool LooksLikeProgressiveAutoPattern(List<CaptionCue> cues)
        {
            if (cues == null || cues.Count < 4)
                return false;

            const long MicroThresholdMs = 80; // 0–80 ms
            int sampleCount = Math.Min(20, cues.Count);
            int microCount = 0;

            for (int i = 0; i < sampleCount; i++)
            {
                var c = cues[i];
                var duration = c.EndTimeMs - c.StartTimeMs;
                if (duration > 0 && duration <= MicroThresholdMs)
                {
                    microCount++;
                }
            }

            // If we see at least 3 micro-cues in the first 20,
            // it strongly resembles the pattern you showed.
            return microCount >= 3;
        }

        /// <summary>
        /// Apply your specific pattern:
        /// - Drop the very first cue (index 0)
        /// - From the next one on, keep every 4th cue:
        ///   keep indices 1, 5, 9, 13, ...
        /// </summary>
        private List<CaptionCue> ApplyEveryFourthPattern(List<CaptionCue> cues)
        {
            var result = new List<CaptionCue>();

            if (cues == null || cues.Count == 0)
                return result;

            if (cues.Count == 1)
                return new List<CaptionCue>(cues);

            // i = 1, 5, 9, 13, ... (step of 4)
            for (int i = 1; i < cues.Count; i += 4)
            {
                result.Add(cues[i]);
            }

            // Safety: if the pattern somehow produced nothing, fall back to original
            if (result.Count == 0)
                return new List<CaptionCue>(cues);

            return result;
        }

        /// <summary>
        /// For auto-generated captions (after filtering with the 1,5,9,13… pattern),
        /// adjust start times so that:
        /// - first cue has StartTimeMs = 0
        /// - each subsequent cue starts exactly at the previous cue's EndTimeMs.
        /// </summary>
        private List<CaptionCue> NormalizeProgressiveStartTimes(List<CaptionCue> cues)
        {
            if (cues == null || cues.Count == 0)
                return cues;

            var normalized = new List<CaptionCue>(cues.Count);
            long lastEnd = 0;

            for (int i = 0; i < cues.Count; i++)
            {
                var c = cues[i];

                long start = (i == 0) ? 0 : lastEnd;
                long end = c.EndTimeMs;

                normalized.Add(new CaptionCue
                {
                    StartTimeMs = start,
                    EndTimeMs = end,
                    Text = c.Text
                });

                lastEnd = end;
            }

            return normalized;
        }

        private long ParseTimestamp(string timestamp)
        {
            // Format: 00:00:00.000
            var parts = timestamp.Split(':');

            var hours = int.Parse(parts[0]);
            var minutes = int.Parse(parts[1]);

            var secondsParts = parts[2].Split('.');
            var seconds = int.Parse(secondsParts[0]);
            var milliseconds = int.Parse(secondsParts[1]);

            return (hours * 3600000L) + (minutes * 60000L) + (seconds * 1000L) + milliseconds;
        }
    }

    public class YouTubeVideoData
    {
        public string Title { get; set; }
        public string ThumbnailUrl { get; set; }
        public string VideoUrl { get; set; }
        public List<CaptionCue> Captions { get; set; }
    }

    public class CaptionCue
    {
        public long StartTimeMs { get; set; }
        public long EndTimeMs { get; set; }
        public string Text { get; set; }
    }
}
