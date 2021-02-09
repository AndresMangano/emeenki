using System;
using System.Collections.Generic;

namespace Hermes.Core
{
    public class Translation
    {
        public int Index { get; set; }
        public string Text { get; set; }
        public string UserID { get; set; }
        public HashSet<string> Upvotes { get; set; }
        public HashSet<string> Downvotes { get; set; }
        public List<TranslationComment> Comments { get; set; }
        public DateTime Timestamp { get; set; }
    }
}