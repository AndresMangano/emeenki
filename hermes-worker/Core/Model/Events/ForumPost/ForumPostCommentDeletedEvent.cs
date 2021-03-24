using System;
using Hermes.Worker.Core.Ports;
using Hermes.Worker.Shell;

namespace Hermes.Worker.Core.Model.Events.ForumPost
{
    public record ForumPostCommentDeletedEvent(
        EventHeader Header,
        Guid ID,
        Guid ForumPostCommentID
    ) : IEvent
    {
        public void Apply(DBInterpreter interpreter)
        {
            interpreter.DeleteForumPostComment(ForumPostCommentID);
        }

        public void Notify(ISignalRPort signalR)
        {
            signalR.SendSignalToGroup(SignalRSignal.FORUM_POST_UPDATED, ID.ToString(),
                SignalRGroup.ForumPost(ID));
        }
    }
}