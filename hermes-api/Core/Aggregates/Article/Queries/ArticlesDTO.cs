using System;

namespace Hermes.Core
{
    public class ArticlesDTO
    {
        public Guid ArticleID { get; set; }
        public string RoomID { get ; set; }
        public string Title { get; set; }
        public DateTime Created { get; set; }
        public string OriginalLanguageID { get; set; }
        public string TranslationLanguageID { get; set; }
        public string PhotoURL { get; set; }
        public bool Archived { get; set; }
    }
}