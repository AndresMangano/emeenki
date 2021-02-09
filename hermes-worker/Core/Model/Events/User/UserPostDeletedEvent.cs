using System;

namespace Hermes.Worker.Core.Model.Events.User
{
    public class UserPostDeletedEvent
    {
        public Guid UserPostID { get; }
        public Guid? ChildUserPostID { get; }
        public string SenderUserID { get; }
        
        public UserPostDeletedEvent(Guid userPostID, Guid? childUserPostID, string senderUserID)
        {
            UserPostID = userPostID;
            SenderUserID = senderUserID;
            ChildUserPostID = childUserPostID;
        }
    }
}