using System;

namespace Hermes.Core
{
    public class ArticleTemplateSentenceDTOREVERTIFNEEDED
    {
        public Guid ArticleTemplateID { get; set; }
        public bool InText { get; set; }
        public int SentenceIndex { get; set; }
        public string Sentence { get; set; }
    }
}