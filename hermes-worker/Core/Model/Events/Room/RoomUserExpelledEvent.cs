using Hermes.Worker.Core.Ports;
using Hermes.Worker.Shell;

namespace Hermes.Worker.Core.Model.Events.Room
{
    public record RoomUserExpelledEvent(
        EventHeader<string> header,
        string UserID
    ) : IEvent<string>
    {
        public EventHeader<string> Header => throw new System.NotImplementedException();

        public void Apply(DBInterpreter interpreter)
        {
            interpreter.DeleteRoomUser(Header.ID, UserID);
        }

        public void Notify(ISignalRPort signalR)
        {
            signalR.SendSignalToGroup(SignalRSignal.ROOM_UPDATED, Header.ID,
                "rooms",
                $"room:{Header.ID}");
        }
    }
}