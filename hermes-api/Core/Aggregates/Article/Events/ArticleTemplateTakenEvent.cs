using System;
using System.Collections.Generic;

namespace Hermes.Core
{
    public class ArticleTemplateTakenEvent
    {
        public Guid ArticleTemplateID { get; }
        public string RoomID { get; }
        public string OriginalLanguageID { get; }
        public string TranslationLanguageID { get; }
        public List<string> Title { get; }
        public List<string> Text { get; }
        public string Source { get; }
        public string PhotoURL { get; }

        public ArticleTemplateTakenEvent(Guid articleTemplateID, string roomID, string originalLanguageID, string translationLanguageID,
            List<string> title, List<string> text, string source, string photoURL)
        {
            ArticleTemplateID = articleTemplateID;
            RoomID = roomID;
            OriginalLanguageID = originalLanguageID;
            TranslationLanguageID = translationLanguageID;
            Title = title;
            Text = text;
            Source = source;
            PhotoURL = photoURL;
        }
    }
}