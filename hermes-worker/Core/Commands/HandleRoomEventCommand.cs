using Hermes.Worker.Core.Model;
using Hermes.Worker.Core.Model.Events.Room;
using Hermes.Worker.Core.Ports;
using Hermes.Worker.Core.Repositories;
using Hermes.Worker.Core.Repositories.Helpers;
using Newtonsoft.Json;

namespace Hermes.Worker.Core.Commands
{
    public static class HandleRoomEventCommand
    {
        public static void Execute<IO, dbIO>(IO io, string routingKey, string message)
        where IO : IUnitOfWorkPort<dbIO>, ISignalRPort
        where dbIO : IRoomRepository, IRoomUserRepository, IRoomQueueRepository {
            switch (routingKey) {
                case "opened": Handle<IO, dbIO>(io, JsonConvert.DeserializeObject<DefaultMessage<string, RoomOpenedEvent>>(message).Message); break;
                case "closed": Handle<IO, dbIO>(io, JsonConvert.DeserializeObject<DefaultMessage<string, RoomClosedEvent>>(message).Message); break;
                case "user.queued": Handle<IO, dbIO>(io, JsonConvert.DeserializeObject<DefaultMessage<string, RoomUserQueuedEvent>>(message).Message); break;
                case "user.joined": Handle<IO, dbIO>(io, JsonConvert.DeserializeObject<DefaultMessage<string, RoomUserJoinedEvent>>(message).Message); break;
                case "user.unqueued": Handle<IO, dbIO>(io, JsonConvert.DeserializeObject<DefaultMessage<string, RoomUserUnqueuedEvent>>(message).Message); break;
                case "user.left": Handle<IO, dbIO>(io, JsonConvert.DeserializeObject<DefaultMessage<string, RoomUserLeftEvent>>(message).Message); break;
                case "user.expelled": Handle<IO, dbIO>(io, JsonConvert.DeserializeObject<DefaultMessage<string, RoomUserExpelledEvent>>(message).Message); break;
                case "usersLimit.changed": Handle<IO, dbIO>(io, JsonConvert.DeserializeObject<DefaultMessage<string, RoomUsersLimitChangedEvent>>(message).Message); break;
                case "restricted": Handle<IO, dbIO>(io, JsonConvert.DeserializeObject<DefaultMessage<string, RoomRestrictedEvent>>(message).Message); break;
                case "unrestricted": Handle<IO, dbIO>(io, JsonConvert.DeserializeObject<DefaultMessage<string, RoomUnrestrictedEvent>>(message).Message); break;
            }
        }
        // Event Handler
        static void Handle<IO, dbIO>(IO io, DefaultMessageBody<string, RoomOpenedEvent> message)
        where IO : IUnitOfWorkPort<dbIO>, ISignalRPort
        where dbIO : IRoomRepository, IRoomUserRepository {
            io.Transaction(dbIO => {
                dbIO.InsertRoom(
                    roomID: message.Metadata.ID,
                    languageID1: message.Payload.LanguageID1,
                    languageID2: message.Payload.LanguageID2,
                    closed: false,
                    restricted: message.Payload.Restricted,
                    usersLimit: message.Payload.UsersLimit
                );
                dbIO.InsertRoomUser(
                    roomID: message.Metadata.ID,
                    userID: message.Payload.UserID,
                    permission: "admin"
                );
            });
            io.SendSignalToGroup(SignalRSignal.ROOM_UPDATED, message.Metadata.ID, "rooms");
        }

        static void Handle<IO, dbIO>(IO io, DefaultMessageBody<string, RoomClosedEvent> message)
        where IO : IUnitOfWorkPort<dbIO>, ISignalRPort
        where dbIO : IRoomRepository {
            io.Execute(dbIO => {
                dbIO.UpdateRoom(message.Metadata.ID,
                    closed: new DbUpdate<bool>(true));
            });
            io.SendSignalToGroup(SignalRSignal.ROOM_UPDATED, message.Metadata.ID,
                "rooms",
                $"room:{message.Metadata.ID}");
        }

        static void Handle<IO, dbIO>(IO io, DefaultMessageBody<string, RoomUserQueuedEvent> message)
        where IO : IUnitOfWorkPort<dbIO>, ISignalRPort
        where dbIO : IRoomQueueRepository {
            io.Execute(dbIO => {
                dbIO.InsertRoomQueue(
                    roomID: message.Metadata.ID,
                    userID: message.Payload.UserID
                );
            });
            io.SendSignalToGroup(SignalRSignal.ROOM_UPDATED, message.Metadata.ID,
                "rooms",
                $"room:{message.Metadata.ID}");
        }

        static void Handle<IO, dbIO>(IO io, DefaultMessageBody<string, RoomUserJoinedEvent> message)
        where IO : IUnitOfWorkPort<dbIO>, ISignalRPort
        where dbIO : IRoomUserRepository {
            io.Execute(dbIO => {
                dbIO.InsertRoomUser(
                    roomID: message.Metadata.ID,
                    userID: message.Payload.UserID,
                    permission: message.Payload.Permission
                );
            });
            io.SendSignalToGroup(SignalRSignal.ROOM_UPDATED, message.Metadata.ID,
                "rooms",
                $"room:{message.Metadata.ID}");
        }

        static void Handle<IO, dbIO>(IO io, DefaultMessageBody<string, RoomUserUnqueuedEvent> message)
        where IO : IUnitOfWorkPort<dbIO>, ISignalRPort
        where dbIO : IRoomQueueRepository {
            io.Execute(dbIO => {
                dbIO.DeleteRoomQueue(message.Metadata.ID, message.Payload.UserID);
            });
            io.SendSignalToGroup(SignalRSignal.ROOM_UPDATED, message.Metadata.ID,
                "rooms",
                $"room:{message.Metadata.ID}");
        }

        static void Handle<IO, dbIO>(IO io, DefaultMessageBody<string, RoomUserLeftEvent> message)
        where IO : IUnitOfWorkPort<dbIO>, ISignalRPort
        where dbIO: IRoomUserRepository {
            io.Execute(dbIO => {
                dbIO.DeleteRoomUser(message.Metadata.ID, message.Payload.UserID);
            });
            io.SendSignalToGroup(SignalRSignal.ROOM_UPDATED, message.Metadata.ID,
                "rooms",
                $"room:{message.Metadata.ID}");
        }

        static void Handle<IO, dbIO>(IO io, DefaultMessageBody<string, RoomUserExpelledEvent> message)
        where IO : IUnitOfWorkPort<dbIO>, ISignalRPort
        where dbIO : IRoomUserRepository {
            io.Execute(dbIO => {
                dbIO.DeleteRoomUser(message.Metadata.ID, message.Payload.UserID);
            });
            io.SendSignalToGroup(SignalRSignal.ROOM_UPDATED, message.Metadata.ID,
                "rooms",
                $"room:{message.Metadata.ID}");
        }

        static void Handle<IO, dbIO>(IO io, DefaultMessageBody<string, RoomUsersLimitChangedEvent> message)
        where IO : IUnitOfWorkPort<dbIO>, ISignalRPort
        where dbIO : IRoomRepository {
            io.Execute(dbIO => {
                dbIO.UpdateRoom(message.Metadata.ID,
                    usersLimit: new DbUpdate<int>(message.Payload.NewUsersLimit));
            });
            io.SendSignalToGroup(SignalRSignal.ROOM_UPDATED, message.Metadata.ID,
                "rooms",
                $"room:{message.Metadata.ID}");
        }

        static void Handle<IO, dbIO>(IO io, DefaultMessageBody<string, RoomRestrictedEvent> message)
        where IO : IUnitOfWorkPort<dbIO>, ISignalRPort
        where dbIO : IRoomRepository {
            io.Execute(dbIO => {
                dbIO.UpdateRoom(message.Metadata.ID,
                    restricted: new DbUpdate<bool>(true));
            });
            io.SendSignalToGroup(SignalRSignal.ROOM_UPDATED, message.Metadata.ID,
                "rooms",
                $"room:{message.Metadata.ID}");
        }

        static void Handle<IO, dbIO>(IO io, DefaultMessageBody<string, RoomUnrestrictedEvent> message)
        where IO : IUnitOfWorkPort<dbIO>, ISignalRPort
        where dbIO : IRoomRepository {
            io.Execute(dbIO => {
                dbIO.UpdateRoom(message.Metadata.ID,
                    restricted: new DbUpdate<bool>(false));
            });
            io.SendSignalToGroup(SignalRSignal.ROOM_UPDATED, message.Metadata.ID,
                "rooms",
                $"room:{message.Metadata.ID}");
        }
    }
}