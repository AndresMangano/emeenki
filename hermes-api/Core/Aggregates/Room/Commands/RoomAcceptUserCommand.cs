using System;

namespace Hermes.Core
{
    public class RoomAcceptUserCommand
    {
        public string RoomID { get; set; }
        public string RoomUserID { get; set; }
        public string Permission { get; set; }
    }
}