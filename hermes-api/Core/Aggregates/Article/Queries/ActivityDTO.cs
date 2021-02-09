using System;

namespace Hermes.Core
{
    public class ActivityDTO
    {
        public string ProfilePhotoURL { get; set; }
        public string UserID { get; set; }
        public string Event { get; set; }
        public string Title { get; set; }
        public int Points { get; set; }
        public DateTime Timestamp { get; set; }
    }
}