using System.Collections.Generic;

namespace Hermes.Core
{
    public class RoomDTO
    {
        public string RoomID { get; set; }
        public bool Closed { get; set; }
        public string LanguageID1 { get; set; }
        public string LanguageID2 { get; set; }
        public IEnumerable<RoomUserDTO> Users { get; set; }
        public IEnumerable<string> UsersQueue { get; set; }
        public bool Restricted { get; set; }
        public short UsersLimit { get; set; }
    }
}