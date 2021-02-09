using System;

namespace Hermes.Core
{
    public static class RoomService
    {
        public static void ValidateExistence(Room room) {
            if (!room.Created || room.Deleted)
                throw new DomainException("The Room does not exist");
        }

        public static void ValidateInexistence(Room room) {
            if (room.Created && room.Deleted)
                throw new DomainException("The Room was deleted");
            else if (room.Created)
                throw new DomainException("The Room already exists");
        }

        public static void ValidateRoomUser(Room room, string userID)
        {
            if (!room.Users.Exists(u => u.UserID == userID))
                throw new DomainException("You are not a user of the room");
        }

        public static void ValidateRoomAdmin(Room room, string userID) {
            if (!room.Users.Exists(u => u.Permission == RoomPermission.ADMIN && u.UserID == userID))
                throw new DomainException("Lack of room admin rights");
        }

        public static void ValidateRoomSpace(Room room) {
            if (room.UsersLimit > 0 && room.Users.Count >= room.UsersLimit)
                throw new DomainException("Full room");
        }

        public static void ValidateUsersLimitChange(Room room, short newUsersLimit) {
            if (newUsersLimit < 0)
                throw new DomainException("Invalid users limit");
            else if (newUsersLimit == room.UsersLimit)
                throw new DomainException("Users limit did not change");
            else if (newUsersLimit > 0 && newUsersLimit < room.Users.Count)
                throw new DomainException("Users limit below actual users");
        }

        public static void ValidateNewMember(Room room, string userID) {
            if (room.Users.Exists(u => u.UserID == userID))
                throw new DomainException("The user is already in the room");
        }

        public static void ValidateUserInQueue(Room room, string userID) {
            if (!room.Users.Exists(u => u.UserID == userID))
                throw new DomainException("The user is not in the queue");
        }

        public static void ValidateInvitationToken(Room room, Guid token) {
            if (token != room.Token.Token)
                throw new DomainException("Invalid invitation token");
            else if ((DateTime.UtcNow - room.Token.Timestamp).Days > 1)
                throw new DomainException("Invitation token expired");
        }
        public static string GetRoomPermissionValue(RoomPermission roomPermission) {
            switch (roomPermission) {
                case RoomPermission.ADMIN: return "admin";
                case RoomPermission.USER: return "user";
                default:
                    return "user";
            }
        }

        public static RoomToken GenerateValidToken(Room room) {
            if ((DateTime.UtcNow - room.Token.Timestamp).Days > 1)
                return new RoomToken {
                    Token = Guid.NewGuid(),
                    Timestamp = DateTime.UtcNow
                };
            else
                return new RoomToken {
                    Token = room.Token.Token,
                    Timestamp = DateTime.UtcNow
                };
        }

        public static RoomPermission ParsePermission(string permission) {
            switch (permission) {
                case "admin": return RoomPermission.ADMIN;
                case "user": return RoomPermission.USER;
                default:
                    return RoomPermission.USER;
            }
        }
    }
}