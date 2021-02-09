using System;

namespace Hermes.Core
{
    public class RoomInviteUserResult
    {
        public Guid Token { get; }
        public RoomInviteUserResult(Guid token)
        {
            Token = token;
        }
    }
}