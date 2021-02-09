using System;

namespace Hermes.Core
{
    public class RoomRejectUserCommand
    {
        public string RoomID { get; set; }
        public string RoomUserID { get; set; }
    }
}