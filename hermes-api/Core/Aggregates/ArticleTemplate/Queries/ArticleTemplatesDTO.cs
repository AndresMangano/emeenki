using System;

namespace Hermes.Core
{
    public class ArticleTemplatesDTO
    {
        public Guid ArticleTemplateID { get; set; }
        public string Title { get; set; }
        public DateTime Created { get; set; }
        public string LanguageID { get; set; }
        public string PhotoURL { get; set; }
        public bool Archived { get; set; }
    }
}