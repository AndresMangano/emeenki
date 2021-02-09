using System;
using System.Collections.Generic;

namespace Hermes.Core
{
    public class Comment
    {
        public int Index { get; set; }
        public string Text { get; set; }
        public string UserID { get; set; }
        public bool Deleted { get; set; }
        public DateTime Timestamp { get; set; }
        public List<Comment> Replies { get; set; }
    }
}