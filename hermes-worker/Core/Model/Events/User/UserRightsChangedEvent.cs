using Hermes.Worker.Core.Ports;
using Hermes.Worker.Core.Repositories.Helpers;
using Hermes.Worker.Shell;

namespace Hermes.Worker.Core.Model.Events.User
{
    public record UserRightsChangedEvent(
        EventHeader<string> Header,
        string NewRights
    ) : IEvent<string>
    {
        public void Apply(DBInterpreter interpreter)
        {
            interpreter.UpdateUser(Header.ID,
                    rights: new DbUpdate<string>(NewRights));
        }

        public void Notify(ISignalRPort signalR)
        {
            signalR.SendSignalToGroup(SignalRSignal.USER_UPDATED, Header.ID,
                "users",
                $"user:{Header.ID}");
        }
    }
}