using System;

namespace Hermes.Core
{
    public class RoomChangeUsersLimitCommand
    {
        public string RoomID { get; set; }
        public short NewLimit { get; set; }
    }
}