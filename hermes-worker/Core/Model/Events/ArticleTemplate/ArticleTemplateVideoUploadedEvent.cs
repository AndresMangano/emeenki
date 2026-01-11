using System;
using System.Collections.Generic;
using Hermes.Worker.Core.Ports;
using Hermes.Worker.Shell;

namespace Hermes.Worker.Core.Model.Events.ArticleTemplate
{
    public record VideoSentence(
        string Text,
        long? StartTimeMs,
        long? EndTimeMs
    );

    public record ArticleTemplateVideoUploadedEvent(
        EventHeader Header,
        Guid ID,
        string TopicID,
        string LanguageID,
        List<VideoSentence> Title,
        List<VideoSentence> Text,
        string Source,
        string PhotoURL,
        string VideoURL
    ) : IEvent
    {
        public void Apply(DBInterpreter interpreter)
        {
            // This event is *only* for video uploads, so we know it's a video.
            const bool isVideo = true;

            // Build title string for list view
            var titleTexts = new List<string>();
            foreach (var t in Title)
            {
                titleTexts.Add(t.Text);
            }
            var joinedTitle = string.Join(" ", titleTexts);

            // Insert into Query_ArticleTemplates (list table) WITH video fields
            interpreter.InsertArticleTemplates(
                articleTemplateID: ID,
                title: joinedTitle,
                created: Header.Timestamp,
                topicID: TopicID,
                languageID: LanguageID,
                photoURL: PhotoURL,
                archived: false,
                isVideo: isVideo,
                videoURL: VideoURL
            );

            // Insert into Query_ArticleTemplate (detail table) WITH video fields
            interpreter.InsertArticleTemplateWithVideo(
                articleTemplateID: ID,
                deleted: false,
                topicID: TopicID,
                languageID: LanguageID,
                source: Source,
                photoURL: PhotoURL,
                timestamp: Header.Timestamp,
                isVideo: isVideo,
                videoURL: VideoURL
            );

            // Insert text sentences with timestamps
            for (var index = 0; index < Text.Count; index++)
            {
                interpreter.InsertArticleTemplateSentenceWithTimestamp(
                    articleTemplateID: ID,
                    inText: true,
                    sentenceIndex: index,
                    sentence: Text[index].Text,
                    startTimeMs: Text[index].StartTimeMs,
                    endTimeMs: Text[index].EndTimeMs
                );
            }

            // Insert title sentences with timestamps
            for (var index = 0; index < Title.Count; index++)
            {
                interpreter.InsertArticleTemplateSentenceWithTimestamp(
                    articleTemplateID: ID,
                    inText: false,
                    sentenceIndex: index,
                    sentence: Title[index].Text,
                    startTimeMs: Title[index].StartTimeMs,
                    endTimeMs: Title[index].EndTimeMs
                );
            }
        }

        public void Notify(ISignalRPort signalR)
        {
            signalR.SendSignalToGroup(
                SignalRSignal.ARTICLE_TEMPLATE_UPDATED,
                ID.ToString(),
                SignalRGroup.ARTICLE_TEMPLATES
            );
        }
    }
}
