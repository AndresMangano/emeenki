using Hermes.Worker.Core.Ports;
using Hermes.Worker.Core.Repositories.Helpers;
using Hermes.Worker.Shell;

namespace Hermes.Worker.Core.Model.Events.Room
{
    public record RoomUsersLimitChangedEvent(
        EventHeader<string> Header,
        short NewUsersLimit
    ) : IEvent<string>
    {
        public void Apply(DBInterpreter interpreter)
        {
            interpreter.UpdateRoom(Header.ID,
                usersLimit: new DbUpdate<int>(NewUsersLimit));
        }

        public void Notify(ISignalRPort signalR)
        {
            signalR.SendSignalToGroup(SignalRSignal.ROOM_UPDATED, Header.ID,
                "rooms",
                $"room:{Header.ID}");
        }
    }
}