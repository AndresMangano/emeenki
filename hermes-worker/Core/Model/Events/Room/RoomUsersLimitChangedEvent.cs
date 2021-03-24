using Hermes.Worker.Core.Ports;
using Hermes.Worker.Core.Repositories.Helpers;
using Hermes.Worker.Shell;

namespace Hermes.Worker.Core.Model.Events.Room
{
    public record RoomUsersLimitChangedEvent(
        EventHeader Header,
        string ID,
        short NewUsersLimit
    ) : IEvent
    {
        public void Apply(DBInterpreter interpreter)
        {
            interpreter.UpdateRoom(ID,
                usersLimit: new DbUpdate<int>(NewUsersLimit));
        }

        public void Notify(ISignalRPort signalR)
        {
            signalR.SendSignalToGroup(SignalRSignal.ROOM_UPDATED, ID,
                SignalRGroup.ROOMS,
                SignalRGroup.Room(ID));
        }
    }
}