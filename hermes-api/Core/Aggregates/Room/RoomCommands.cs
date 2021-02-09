using System;
using Hermes.Core.Ports;

namespace Hermes.Core
{
    public static class RoomCommands
    {
        public static RoomOpenResult Execute<IO>(IO io, RoomOpenCommand cmd, string userID)
        where IO : IEventsRepository, IRoomsRepository, IUsersRepository
        {
            var room = io.FetchRoom(cmd.RoomID);
            var user = io.FetchUser(userID);
            UserService.ValidateAdminRights(user);
            RoomService.ValidateInexistence(room);
            if (cmd.Languages[0] == cmd.Languages[1])
                throw new DomainException("Both languages cannot be equals");
            var token = Guid.NewGuid();
            io.StoreEvent(new RoomEvent (
                id: cmd.RoomID,
                version: room.Version + 1,
                timestamp: DateTime.UtcNow,
                stream: "room",
                eventName: "opened",
                payload: new RoomOpenedEvent(
                    token: token,
                    languageID1: cmd.Languages[0],
                    languageID2: cmd.Languages[1],
                    usersLimit: cmd.UsersLimit,
                    restricted: cmd.Restricted,
                    userID: user.ID
                )
            ));
            return new RoomOpenResult(cmd.RoomID);
        }

        public static void Execute<IO>(IO io, RoomCloseCommand cmd, string userID)
        where IO : IEventsRepository, IRoomsRepository
        {
            var room = io.FetchRoom(cmd.RoomID);
            if (room.Deleted)
                throw new DomainException("The room is already closed");
            RoomService.ValidateRoomAdmin(room, userID);            
            io.StoreEvent(new RoomEvent (
                id: room.ID,
                version: room.Version + 1,
                timestamp: DateTime.UtcNow,
                stream: "room",
                eventName: "closed",
                payload: new RoomClosedEvent(
                    userID: userID
                )
            ));
        }

        public static void Execute<IO>(IO io, RoomJoinCommand cmd, string userID)
        where IO : IEventsRepository, IRoomsRepository
        {
            var room = io.FetchRoom(cmd.RoomID);
            RoomService.ValidateNewMember(room, userID);
            RoomService.ValidateRoomSpace(room);
            if (room.Restricted)
                io.StoreEvent(new RoomEvent (
                    id: room.ID,
                    version: room.Version + 1,
                    timestamp: DateTime.UtcNow,
                    stream: "room",
                    eventName: "user.queued",
                    payload: new RoomUserQueuedEvent(
                        userID: userID
                    )
                ));
            else
                io.StoreEvent(new RoomEvent(
                    id: room.ID,
                    version: room.Version + 1,
                    timestamp: DateTime.UtcNow,
                    stream: "room",
                    eventName: "user.joined",
                    payload: new RoomUserJoinedEvent(
                        userID: userID,
                        permission: "user"
                    )
                ));
        }

        public static void Execute<IO>(IO io, RoomAcceptUserCommand cmd, string userID)
        where IO : IEventsRepository, IRoomsRepository
        {
            var room = io.FetchRoom(cmd.RoomID);
            RoomService.ValidateRoomAdmin(room, userID);
            RoomService.ValidateUserInQueue(room, cmd.RoomUserID);
            RoomService.ValidateRoomSpace(room);
            io.StoreEvent(new RoomEvent (
                id: room.ID,
                version: room.Version + 1,
                timestamp: DateTime.UtcNow,
                stream: "room",
                eventName: "user.joined",
                payload: new RoomUserJoinedEvent(
                    userID: cmd.RoomUserID,
                    permission: cmd.Permission
                )
            ));
        }

        public static void Execute<IO>(IO io, RoomRejectUserCommand cmd, string userID)
        where IO : IEventsRepository, IRoomsRepository
        {
            var room = io.FetchRoom(cmd.RoomID);
            RoomService.ValidateRoomAdmin(room, userID);
            RoomService.ValidateUserInQueue(room, cmd.RoomUserID);
            io.StoreEvent(new RoomEvent (
                id: room.ID,
                version: room.Version + 1,
                timestamp: DateTime.UtcNow,
                stream: "room",
                eventName: "user.unqueued",
                payload: new RoomUserUnqueuedEvent(
                    userID: cmd.RoomUserID
                )
            ));
        }

        public static void Execute<IO>(IO io, RoomLeaveCommand cmd, string userID)
        where IO : IEventsRepository, IRoomsRepository
        {
            var room = io.FetchRoom(cmd.RoomID);
            RoomService.ValidateRoomUser(room, userID);
            io.StoreEvent(new RoomEvent (
                id: room.ID,
                version: room.Version + 1,
                timestamp: DateTime.UtcNow,
                stream: "room",
                eventName: "user.left",
                payload: new RoomUserLeftEvent(
                    userID: userID
                )
            ));
        }

        public static void Execute<IO>(IO io, RoomExpelUserCommand cmd, string userID)
        where IO : IEventsRepository, IRoomsRepository
        {
            var room = io.FetchRoom(cmd.RoomID);
            RoomService.ValidateRoomAdmin(room, userID);
            RoomService.ValidateRoomUser(room, cmd.RoomUserID);
            io.StoreEvent(new RoomEvent (
                id: room.ID,
                version: room.Version + 1,
                timestamp: DateTime.UtcNow,
                stream: "room",
                eventName: "user.expelled",
                payload: new RoomUserExpelledEvent(
                    userID: cmd.RoomUserID
                )
            ));
        }

        public static void Execute<IO>(IO io, RoomChangeUsersLimitCommand cmd, string userID)
        where IO : IEventsRepository, IRoomsRepository
        {
            var room = io.FetchRoom(cmd.RoomID);
            RoomService.ValidateRoomAdmin(room, userID);
            RoomService.ValidateUsersLimitChange(room, cmd.NewLimit);
            io.StoreEvent(new RoomEvent (
                id: room.ID,
                version: room.Version + 1,
                timestamp: DateTime.UtcNow,
                stream: "room",
                eventName: "usersLimit.changed",
                payload: new RoomUsersLimitChangedEvent(
                    newUsersLimit: cmd.NewLimit
                )
            ));
        }

        public static void Execute<IO>(IO io, RoomRestrictCommand cmd, string userID)
        where IO : IEventsRepository, IRoomsRepository
        {
            var room = io.FetchRoom(cmd.RoomID);
            if (room.Restricted)
                throw new DomainException("Room already restricted");
            RoomService.ValidateRoomAdmin(room, userID);
            io.StoreEvent(new RoomEvent (
                id: room.ID,
                version: room.Version + 1,
                timestamp: DateTime.UtcNow,
                stream: "room",
                eventName: "restricted",
                payload: new RoomRestrictedEvent(
                    userID: userID
                )
            ));
        }

        public static void Execute<IO>(IO io, RoomUnrestrictCommand cmd, string userID)
        where IO : IEventsRepository, IRoomsRepository
        {
            var room = io.FetchRoom(cmd.RoomID);
            if (!room.Restricted)
                throw new DomainException("The room is already public");
            RoomService.ValidateRoomAdmin(room, userID);
            io.StoreEvent(new RoomEvent (
                id: room.ID,
                version: room.Version + 1,
                timestamp: DateTime.UtcNow,
                stream: "room",
                eventName: "unrestricted",
                payload: new RoomUnrestrictedEvent(
                    userID: userID
                )
            ));
        }

        public static RoomInviteUserResult Execute<IO>(IO io, RoomInviteUserCommand cmd, string userID)
        where IO : IEventsRepository, IRoomsRepository
        {
            var room = io.FetchRoom(cmd.RoomID);
            RoomService.ValidateRoomAdmin(room, userID);
            RoomToken token = RoomService.GenerateValidToken(room);
            io.StoreEvent(new RoomEvent (
                id: room.ID,
                version: room.Version + 1,
                timestamp: token.Timestamp,
                stream: "room",
                eventName: "token.renewed",
                payload: new RoomTokenRenewedEvent(
                    token: token.Token
                )
            ));
            return new RoomInviteUserResult(token.Token);
        }

        public static void Execute<IO>(IO io, RoomJoinWithTokenCommand cmd, string userID)
        where IO : IEventsRepository, IRoomsRepository
        {
            var room = io.FetchRoom(cmd.RoomID);
            RoomService.ValidateInvitationToken(room, cmd.Token);
            RoomService.ValidateNewMember(room, userID);
            RoomService.ValidateRoomSpace(room);
            io.StoreEvent(new RoomEvent (
                id: room.ID,
                version: room.Version + 1,
                timestamp: DateTime.UtcNow,
                stream: "room",
                eventName: "user.joined",
                payload: new RoomUserJoinedEvent(
                    userID: userID,
                    permission: "user"
                )
            ));
        }
    }
}