using System;

namespace Hermes.Core
{
    public class UserLoggedOutEvent
    {
        public Guid SessionID { get; }

        public UserLoggedOutEvent(Guid sessionID)
        {
            SessionID = sessionID;
        }
    }
}