using System;
using Hermes.Worker.Core.Ports;
using Hermes.Worker.Core.Repositories.Helpers;
using Hermes.Worker.Shell;

namespace Hermes.Worker.Core.Model.Events.ForumPost
{
    public record ForumPostEditedEvent(
        EventHeader Header,
        Guid ID,
        string Title,
        string Text,
        string LanguageID
    ) : IEvent
    {
        public void Apply(DBInterpreter interpreter)
        {
            interpreter.UpdateForumPost(ID,
                title: new DbUpdate<string>(Title),
                text: new DbUpdate<string>(Text),
                languageID: new DbUpdate<string>(LanguageID),
                modifiedOn: new DbUpdate<DateTime>(Header.Timestamp)
            );
        }

        public void Notify(ISignalRPort signalR)
        {
            signalR.SendSignalToGroup(SignalRSignal.FORUM_POST_UPDATED, ID.ToString(),
                SignalRGroup.FORUM_POSTS,
                SignalRGroup.ForumPost(ID));
        }
    }
}