using System.Collections.Generic;

namespace Hermes.Worker.Core.Model.Events.ArticleTemplate
{
    public class ArticleTemplateUploadedEvent
    {
        public string TopicID { get; }
        public string LanguageID { get; }
        public List<string> Title { get; }
        public List<string> Text { get; }
        public string Source { get; }
        public string PhotoURL { get; }

        public ArticleTemplateUploadedEvent(string topicID, string languageID, List<string> title, List<string> text, string source, string photoURL)
        {
            TopicID = topicID;
            LanguageID = languageID;
            Title = title;
            Text = text;
            Source = source;
            PhotoURL = photoURL;
        }
    }
}