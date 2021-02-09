using System;

namespace Hermes.Core
{
    public class UserPostDeletedEvent
    {
        public Guid UserPostID { get; }
        public Guid? ChildUserPostID { get; }
        public string SenderUserID { get; }
        
        public UserPostDeletedEvent(Guid userPostID, Guid? childUserPostID, string senderUserID)
        {
            UserPostID = userPostID;
            ChildUserPostID = childUserPostID;
            SenderUserID = senderUserID;
        }
    }
}