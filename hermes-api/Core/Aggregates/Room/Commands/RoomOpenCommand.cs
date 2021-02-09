using System;

namespace Hermes.Core
{
    public class RoomOpenCommand
    {
        public string RoomID { get; set; }
        public string[] Languages { get; set; }
        public short UsersLimit { get; set; }
        public bool Restricted { get; set; }
    }
}