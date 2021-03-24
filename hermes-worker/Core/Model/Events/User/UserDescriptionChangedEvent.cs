using Hermes.Worker.Core.Ports;
using Hermes.Worker.Core.Repositories.Helpers;
using Hermes.Worker.Shell;

namespace Hermes.Worker.Core.Model.Events.User
{
    public record UserDescriptionChangedEvent(
        EventHeader Header,
        string ID,
        string Description
    ) : IEvent
    {
        public void Apply(DBInterpreter interpreter)
        {
            interpreter.UpdateUser(ID,
                    description: new DbUpdate<string>(Description));
        }

        public void Notify(ISignalRPort signalR)
        {
            signalR.SendSignalToGroup(SignalRSignal.USER_UPDATED, ID,
                SignalRGroup.USERS,
                SignalRGroup.User(ID));
        }
    }
}