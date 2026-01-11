public class ArticleTemplateUploadVideoCommand
{
    public string LanguageID { get; set; }
    public string TopicID { get; set; }
    public string YoutubeURL { get; set; }

    // New (optional) – default to "youtube"
    public string Source { get; set; } = "youtube";
}
