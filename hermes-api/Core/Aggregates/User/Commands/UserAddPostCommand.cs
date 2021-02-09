using System;

namespace Hermes.Core
{
    public class UserAddPostCommand
    {
        public string UserID { get; set; }
        public string Text { get; set; }
        public Guid? ParentUserPostID { get; set; }
    }
}