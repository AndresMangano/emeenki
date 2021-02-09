using System;
using System.Collections.Generic;

namespace Hermes.Core
{
    public class ArticleTemplateDTO
    {
        public int ID { get; set; }
        public Guid ArticleTemplateID { get; set; }
        public bool Deleted { get; set; }
        public string LanguageID { get; set; }
        public IEnumerable<string> Title { get; set; }
        public IEnumerable<string> Text { get; set; }
        public string Source { get; set; }
        public string PhotoURL { get; set; }
        public DateTime Timestamp { get; set; }
    }
}