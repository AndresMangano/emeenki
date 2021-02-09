using System;

namespace Hermes.Core
{
    public class ArticleCommentCommand
    {
        public Guid ArticleID { get; set; }
        public bool InText { get; set; }
        public int SentencePos { get; set; }
        public int TranslationPos { get; set; }
        public string Comment { get; set; }
    }
}