using System;

namespace Hermes.Core
{
    public class ForumPostDTO
    {
        public Guid ID { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public string LanguageID { get; set; }
        public string UserID { get; set; }
        public string ProfilePhotoURL { get; set; }
        public DateTime Timestamp { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}