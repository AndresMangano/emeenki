using System;

namespace Hermes.Core
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