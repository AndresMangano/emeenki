using System;

namespace Hermes.Core
{
    public class RoomJoinWithTokenCommand
    {
        public string RoomID { get; set; }
        public Guid Token { get; set; }
    }
}