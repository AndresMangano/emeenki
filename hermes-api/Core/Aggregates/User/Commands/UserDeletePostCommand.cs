using System;

namespace Hermes.Core
{
    public class UserDeletePostCommand
    {
        public string UserID { get; set; }
        public Guid UserPostID { get; set; }
        public Guid? ChildUserPostID { get; set; }
    }
}