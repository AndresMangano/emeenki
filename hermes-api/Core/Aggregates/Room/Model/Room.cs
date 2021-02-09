using System;
using System.Collections.Generic;

namespace Hermes.Core
{
    public class Room : IAggregateRoot<string>
    {
        public string ID { get; set; }
        public int Version { get; set; }
        public bool Created { get; set; }
        public bool Deleted { get; set; }
        public string[] Languages { get; set;}
        public short UsersLimit { get; set; }
        public List<RoomUser> Users { get; set; }
        public HashSet<string> UsersQueue { get; set; }
        public bool Restricted { get; set; }
        public RoomToken Token { get; set; }
    }
}