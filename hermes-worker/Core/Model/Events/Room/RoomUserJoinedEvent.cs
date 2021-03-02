using Hermes.Worker.Core.Ports;
using Hermes.Worker.Shell;

namespace Hermes.Worker.Core.Model.Events.Room
{
    public record RoomUserJoinedEvent(
        EventHeader<string> Header,
        string UserID,
        string Permission
    ) : IEvent<string>
    {
        public void Apply(DBInterpreter interpreter)
        {
            interpreter.InsertRoomUser(
                roomID: Header.ID,
                userID: UserID,
                permission: Permission
            );
        }

        public void Notify(ISignalRPort signalR)
        {
            signalR.SendSignalToGroup(SignalRSignal.ROOM_UPDATED, Header.ID,
                "rooms",
                $"room:{Header.ID}");
        }
    }
}