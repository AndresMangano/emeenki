using System;

namespace Hermes.Core
{
    public class ArticleTranslateCommand
    {
        public Guid ArticleID { get; set; }
        public bool InText { get; set; }
        public int SentencePos { get; set; }
        public string Translation { get; set; }
    }
}