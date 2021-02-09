using System;

namespace Hermes.Core
{
    public class ArticleTemplateUploadCommand
    {
        public string LanguageID { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public string Source { get; set; }
        public string PhotoURL { get; set; }
    }
}