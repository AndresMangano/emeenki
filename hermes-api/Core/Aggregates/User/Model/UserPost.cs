using System;
using System.Collections.Generic;

namespace Hermes.Core
{
    public class UserPost
    {
        public Guid UserPostID { get; set; }
        public string Text { get; set; }
        public string UserID { get; set; }
        public DateTime Timestamp { get; set; }
        public List<UserPost> Replies { get; set; }
    }
}