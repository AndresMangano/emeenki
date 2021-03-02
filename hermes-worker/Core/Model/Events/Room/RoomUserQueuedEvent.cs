using Hermes.Worker.Core.Ports;
using Hermes.Worker.Shell;

namespace Hermes.Worker.Core.Model.Events.Room
{
    public record RoomUserQueuedEvent(
        EventHeader<string> Header,
        string UserID
    ) : IEvent<string>
    {
        public void Apply(DBInterpreter interpreter)
        {
            interpreter.InsertRoomQueue(
                roomID: Header.ID,
                userID: UserID
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