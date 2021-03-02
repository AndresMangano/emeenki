using System;
using Hermes.Worker.Core.Ports;
using Hermes.Worker.Shell;

namespace Hermes.Worker.Core.Model.Events.User
{
    public record UserDeletedEvent(
        EventHeader<string> Header,
        Guid SessionID
    ) : IEvent<string>
    {
        public void Apply(DBInterpreter interpreter)
        {
            interpreter.DeleteUser(Header.ID);
        }

        public void Notify(ISignalRPort signalR)
        {
            signalR.SendSignalToGroup(SignalRSignal.USER_UPDATED, Header.ID,
                "users",
                $"user:{Header.ID}");
        }
    }
}