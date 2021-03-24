using System;

namespace Hermes.Core
{
    public class ForumPostCommentDTO
    {
        public Guid ID { get; set; }
        public Guid ForumPostID { get; set; }
        public string Text { get; set; }
        public string UserID { get; set; }
        public string ProfilePhotoURL { get; set; }
        public DateTime Timestamp { get; set; }
    }
}