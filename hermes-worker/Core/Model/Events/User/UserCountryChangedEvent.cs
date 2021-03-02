using Hermes.Worker.Core.Ports;
using Hermes.Worker.Core.Repositories.Helpers;
using Hermes.Worker.Shell;

namespace Hermes.Worker.Core.Model.Events.User
{
    public record UserCountryChangedEvent(
        EventHeader<string> Header,
        string Country
    ) : IEvent<string>
    {
        public void Apply(DBInterpreter interpreter)
        {
            interpreter.UpdateUser(Header.ID,
                    country: new DbUpdate<string>(Country));
        }

        public void Notify(ISignalRPort signalR)
        {
            signalR.SendSignalToGroup(SignalRSignal.USER_UPDATED, Header.ID,
                "users",
                $"user:{Header.ID}");
        }
    }
}