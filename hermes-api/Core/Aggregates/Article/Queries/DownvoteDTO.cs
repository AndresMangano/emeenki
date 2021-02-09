using System;

namespace Hermes.Core
{
    public class DownvoteDTO
    {
        public Guid ArticleID { get; set; }
        public bool InText { get; set; }
        public int SentenceIndex { get; set; }
        public int TranslationIndex { get ;set; }
        public string UserID { get; set; }
    }
}