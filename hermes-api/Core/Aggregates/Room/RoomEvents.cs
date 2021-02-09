using System.Collections.Generic;

namespace Hermes.Core
{
    public static class RoomEvents
    {
        public static void Apply(Room room, DomainEvent<string, object> evnt)
        {
            switch (evnt.Payload)
            {
                case RoomClosedEvent e:
                    room.Deleted = true;
                    break;
                case RoomOpenedEvent e: 
                    room.ID = evnt.Metadata.ID;
                    room.Token = new RoomToken {
                        Token = e.Token,
                        Timestamp = evnt.Metadata.Timestamp
                    };
                    room.Languages = new string[] { e.LanguageID1, e.LanguageID2 };
                    room.Users = new List<RoomUser> {
                        new RoomUser {
                            UserID = e.UserID,
                            Permission = RoomPermission.ADMIN
                        }
                    };
                    room.UsersQueue = new HashSet<string>();
                    room.Restricted = e.Restricted;
                    room.UsersLimit = e.UsersLimit;
                    room.Created = true;
                    break;
                case RoomRestrictedEvent e: 
                    room.Restricted = true;
                    break;
                case RoomTokenRenewedEvent e: 
                    room.Token.Token = e.Token;
                    room.Token.Timestamp = evnt.Metadata.Timestamp;
                    break;
                case RoomUnrestrictedEvent e: 
                    room.Restricted = false;
                    break;
                case RoomUserExpelledEvent e: 
                    room.Users.RemoveAll(u => u.UserID == e.UserID);
                    break;
                case RoomUserJoinedEvent e: 
                    room.Users.Add(new RoomUser {
                        UserID = e.UserID,
                        Permission = RoomService.ParsePermission(e.Permission)
                    });
                    break;
                case RoomUserLeftEvent e: 
                    room.Users.RemoveAll(u => u.UserID == e.UserID);
                    break;
                case RoomUserQueuedEvent e: 
                    room.UsersQueue.Add(e.UserID);
                    break;
                case RoomUsersLimitChangedEvent e: 
                    room.UsersLimit = e.NewUsersLimit;
                    break;
                case RoomUserUnqueuedEvent e: 
                    room.UsersQueue.Remove(e.UserID);
                    break;
            }
            room.Version = evnt.Metadata.Version;
        }
    }
}