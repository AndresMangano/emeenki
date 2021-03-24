using Hermes.Worker.Core.Ports;
using Hermes.Worker.Shell;

namespace Hermes.Worker.Core.Model.Events.Room
{
    public record RoomUserJoinedEvent(
        EventHeader Header,
        string ID,
        string UserID,
        string Permission
    ) : IEvent
    {
        public void Apply(DBInterpreter interpreter)
        {
            interpreter.InsertRoomUser(
                roomID: ID,
                userID: UserID,
                permission: Permission
            );
            interpreter.DeleteRoomQueue(ID, UserID);
        }

        public void Notify(ISignalRPort signalR)
        {
            signalR.SendSignalToGroup(SignalRSignal.ROOM_UPDATED, ID,
                SignalRGroup.ROOMS,
                SignalRGroup.Room(ID));
        }
    }
}