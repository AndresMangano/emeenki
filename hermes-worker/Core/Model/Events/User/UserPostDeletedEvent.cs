using System;
using Hermes.Worker.Core.Ports;
using Hermes.Worker.Shell;

namespace Hermes.Worker.Core.Model.Events.User
{
    public record UserPostDeletedEvent(
        EventHeader Header,
        string ID,
        Guid UserPostID,
        Guid? ChildUserPostID,
        string SenderUserID
    ) : IEvent
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
            signalR.SendSignalToGroup(SignalRSignal.USER_UPDATED, ID,
                SignalRGroup.User(ID));
        }
    }
}