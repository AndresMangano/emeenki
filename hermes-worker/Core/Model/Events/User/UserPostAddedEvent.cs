using System;

namespace Hermes.Worker.Core.Model.Events.User
{
    public class UserPostAddedEvent
    {
        public Guid UserPostID { get; }
        public string Text { get; }
        public string UserID { get; }
        public Guid? ChildUserPostID { get; }
        
        public UserPostAddedEvent(Guid userPostId, string text, string userID, Guid? childUserPostID)
        {
            UserPostID = userPostId;
            Text = text;
            UserID = userID;
            ChildUserPostID = childUserPostID;
        }
    }
}