using System;

namespace Hermes.Worker.Core.Model.Events.User
{
    public class UserDeletedEvent
    {
        public Guid SessionID { get; }

        public UserDeletedEvent(Guid sessionID)
        {
            SessionID = sessionID;
        }
    }
}