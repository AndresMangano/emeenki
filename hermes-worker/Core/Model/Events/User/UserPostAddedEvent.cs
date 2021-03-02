using System;
using Hermes.Worker.Core.Ports;
using Hermes.Worker.Shell;

namespace Hermes.Worker.Core.Model.Events.User
{
    public record UserPostAddedEvent(
        EventHeader<string> Header,
        Guid UserPostID,
        string Text,
        string UserID,
        Guid? ChildUserPostID
    ) : IEvent<string>
    {
        public void Apply(DBInterpreter interpreter)
        {
            interpreter.InsertUserPost(
                userPostID: UserPostID,
                childUserPostID: ChildUserPostID,
                userID: Header.ID,
                text: Text,
                senderUserID: UserID,
                timestamp: Header.Timestamp
            );
        }

        public void Notify(ISignalRPort signalR)
        {
            signalR.SendSignalToGroup(SignalRSignal.USER_UPDATED, Header.ID, $"user:{Header.ID}");
        }
    }
}