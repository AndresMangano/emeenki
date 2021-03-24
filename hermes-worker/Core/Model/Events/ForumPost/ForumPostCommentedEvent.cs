using System;
using Hermes.Worker.Core.Ports;
using Hermes.Worker.Core.Repositories.Helpers;
using Hermes.Worker.Shell;

namespace Hermes.Worker.Core.Model.Events.ForumPost
{
    public record ForumPostCommentedEvent(
        EventHeader Header,
        Guid ID,
        Guid ForumPostCommentID,
        string Text,
        string UserID
    ) : IEvent
    {
        public void Apply(DBInterpreter interpreter)
        {
            interpreter.InsertForumPostComment(ForumPostCommentID, ID, Text, UserID, Header.Timestamp);
            interpreter.UpdateForumPost(ID,
                lastCommentUserID: new DbUpdate<string>(UserID),
                lastCommentTimestamp: new DbUpdate<DateTime>(Header.Timestamp));
        }

        public void Notify(ISignalRPort signalR)
        {
            signalR.SendSignalToGroup(SignalRSignal.FORUM_POST_UPDATED, ID.ToString(),
                SignalRGroup.ForumPost(ID));
        }
    }
}