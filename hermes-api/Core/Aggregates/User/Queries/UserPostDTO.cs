using System;

namespace Hermes.Core
{
    public class UserPostDTO
    {
        public Guid UserPostID { get; set; }
        public Guid? ChildUserPostID { get; set; }
        public string Text { get; set; }
        public string SenderUserID { get; set; }
        public string ProfilePhotoURL { get; set; }
        public DateTime Timestamp { get; set; }
    }
}