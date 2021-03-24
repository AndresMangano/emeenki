using System;
using Hermes.Worker.Core.Ports;
using Hermes.Worker.Shell;

namespace Hermes.Worker.Core.Model.Events.ForumPost
{
    public record ForumPostDeletedEvent(
        EventHeader Header,
        Guid ID
    ) : IEvent
    {
        public void Apply(DBInterpreter interpreter)
        {
            interpreter.DeleteForumPost(ID);
        }

        public void Notify(ISignalRPort signalR)
        {
            signalR.SendSignalToGroup(SignalRSignal.FORUM_POST_UPDATED, ID.ToString(),
                SignalRGroup.FORUM_POSTS,
                SignalRGroup.ForumPost(ID));
        }
    }
}