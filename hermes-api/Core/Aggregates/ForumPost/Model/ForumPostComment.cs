using System;

namespace Hermes.Core
{
    public class ForumPostComment
    {
        public Guid ID { get; set; }
        public string Text { get; set; }
        public string UserID { get; set; }
        public DateTime Timestamp { get; set; }
    }
}