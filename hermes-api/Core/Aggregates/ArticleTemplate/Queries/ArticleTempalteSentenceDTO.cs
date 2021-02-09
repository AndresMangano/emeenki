using System;

namespace Hermes.Core
{
    public class ArticleTemplateSentenceDTO
    {
        public Guid ArticleTemplateID { get; set; }
        public bool InText { get; set; }
        public int SentenceIndex { get; set; }
        public string Sentence { get; set; }
    }
}