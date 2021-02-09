using System;

namespace Hermes.Core
{
    public class UserLoggedInEvent
    {
        public Guid SessionID { get; }

        public UserLoggedInEvent(Guid sessionID)
        {
            SessionID = sessionID;
        }
    }
}