using System;
using System.Collections.Generic;
using Hermes.Core;
using Hermes.Core.Ports;
using Newtonsoft.Json;

namespace Hermes.Shell
{
    public partial class DomainInterpreter : IRoomsRepository
    {
        void InitRoomsRepository()
        {
            _roomsRepository = new EventRepository<string, Room>(new SQLEventStorage<string>(
                _connection.ConnectionString,
                "Room",
                ParseRoomEvent
            ));
        }
        object ParseRoomEvent(string eventName, string payload)
        {
            switch(eventName) {
                case "opened": return JsonConvert.DeserializeObject<RoomOpenedEvent>(payload);
                case "closed": return JsonConvert.DeserializeObject<RoomClosedEvent>(payload);
                case "user.queued": return JsonConvert.DeserializeObject<RoomUserQueuedEvent>(payload);
                case "user.joined": return JsonConvert.DeserializeObject<RoomUserJoinedEvent>(payload);
                case "user.unqueued": return JsonConvert.DeserializeObject<RoomUserUnqueuedEvent>(payload);
                case "user.left": return JsonConvert.DeserializeObject<RoomUserLeftEvent>(payload);
                case "user.expelled": return JsonConvert.DeserializeObject<RoomUserExpelledEvent>(payload);
                case "usersLimit.changed": return JsonConvert.DeserializeObject<RoomUsersLimitChangedEvent>(payload);
                case "restricted": return JsonConvert.DeserializeObject<RoomRestrictedEvent>(payload);
                case "unrestricted": return JsonConvert.DeserializeObject<RoomUnrestrictedEvent>(payload);
                case "token.renewed": return JsonConvert.DeserializeObject<RoomTokenRenewedEvent>(payload);
                default:
                    throw new InfrastructureException("Unknown room event");
            }
        }
        long? ApplyRoomEvent(RoomEvent @event) {
            var index = _roomsRepository.StoreEvent(@event);
            if (index != null) {
                SendMessage("room_events", index.Value, @event.Metadata, @event.Payload);
            }
            return index;
        }

        public Room FetchRoom(string id) => _roomsRepository.Fetch(id, RoomEvents.Apply);
    }
}