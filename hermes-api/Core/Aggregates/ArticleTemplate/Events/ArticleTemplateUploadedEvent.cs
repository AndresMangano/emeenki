using System.Collections.Generic;

namespace Hermes.Core
{
    public class ArticleTemplateUploadedEvent
    {
        public string LanguageID { get; }
        public List<string> Title { get; }
        public List<string> Text { get; }
        public string Source { get; }
        public string PhotoURL { get; }
        public string TopicID { get; }

        public ArticleTemplateUploadedEvent(string languageID, List<string> title, List<string> text, string source, string photoURL, string topicID)
        {
            LanguageID = languageID;
            Title = title;
            Text = text;
            Source = source;
            PhotoURL = photoURL;
            TopicID = topicID;
        }
    }
}