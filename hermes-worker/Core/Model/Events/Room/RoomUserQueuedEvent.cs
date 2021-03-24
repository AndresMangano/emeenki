using Hermes.Worker.Core.Ports;
using Hermes.Worker.Shell;

namespace Hermes.Worker.Core.Model.Events.Room
{
    public record RoomUserQueuedEvent(
        EventHeader Header,
        string ID,
        string UserID
    ) : IEvent
    {
        public void Apply(DBInterpreter interpreter)
        {
            interpreter.InsertRoomQueue(
                roomID: ID,
                userID: UserID
            );
        }

        public void Notify(ISignalRPort signalR)
        {
            signalR.SendSignalToGroup(SignalRSignal.ROOM_UPDATED, ID,
                SignalRGroup.ROOMS,
                SignalRGroup.Room(ID));
        }
    }
}