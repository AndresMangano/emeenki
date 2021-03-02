using System;
using Hermes.Worker.Core.Ports;
using Hermes.Worker.Shell;

namespace Hermes.Worker.Core.Model.Events.User
{
    public record UserPostDeletedEvent(
        EventHeader<string> Header,
        Guid UserPostID,
        Guid? ChildUserPostID,
        string SenderUserID
    ) : IEvent<string>
    {
        public void Apply(DBInterpreter interpreter)
        {
            interpreter.DeleteUserPost(
                userPostID: UserPostID,
                childUserPostID: ChildUserPostID
            );
        }

        public void Notify(ISignalRPort signalR)
        {
            signalR.SendSignalToGroup(SignalRSignal.USER_UPDATED, Header.ID, $"user:{Header.ID}");
        }
    }
}