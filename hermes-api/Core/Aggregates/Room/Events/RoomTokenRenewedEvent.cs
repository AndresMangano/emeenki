using System;

namespace Hermes.Core
{
    public class RoomTokenRenewedEvent
    {
        public Guid Token { get; }

        public RoomTokenRenewedEvent(Guid token)
        {
            Token = token;
        }
    }
}