using System;
using Hermes.Worker.Core.Ports;
using Hermes.Worker.Shell;

namespace Hermes.Worker.Core.Model.Events.User
{
    public record UserPostAddedEvent(
        EventHeader Header,
        string ID,
        Guid UserPostID,
        string Text,
        string UserID,
        Guid? ChildUserPostID
    ) : IEvent
    {
        public void Apply(DBInterpreter interpreter)
        {
            interpreter.InsertUserPost(
                userPostID: UserPostID,
                childUserPostID: ChildUserPostID,
                userID: ID,
                text: Text,
                senderUserID: UserID,
                timestamp: Header.Timestamp
            );
        }

        public void Notify(ISignalRPort signalR)
        {
            signalR.SendSignalToGroup(SignalRSignal.USER_UPDATED, ID, $"user:{ID}");
        }
    }
}