using System;
using Hermes.Worker.Core.Ports;
using Hermes.Worker.Shell;

namespace Hermes.Worker.Core.Model.Events.ForumPost
{
    public record ForumPostCreatedEvent(
        EventHeader Header,
        Guid ID,
        string Title,
        string Text,
        string LanguageID,
        string UserID
    ) : IEvent
    {
        public void Apply(DBInterpreter interpreter)
        {
            interpreter.InsertForumPost(ID, Title, Text, LanguageID, UserID, Header.Timestamp);
        }

        public void Notify(ISignalRPort signalR)
        {
            signalR.SendSignalToGroup(SignalRSignal.FORUM_POST_UPDATED, ID.ToString(),
                SignalRGroup.FORUM_POSTS);
        }
    }
}