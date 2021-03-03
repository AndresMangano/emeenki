using System;
using Hermes.Worker.Core.Ports;
using Hermes.Worker.Shell;

namespace Hermes.Worker.Core.Model.Events.Article
{
    public record ArticleMainCommentedEvent(
        EventHeader Header,
        Guid ID,
        int CommentPos,
        int? ChildCommentPos,
        string Comment,
        string UserID
    ) : IEvent
    {
        public void Apply(DBInterpreter interpreter)
        {
            interpreter.InsertArticleComment(
                articleID: ID,
                commentIndex: CommentPos,
                childCommentIndex: ChildCommentPos,
                comment: Comment,
                userID: UserID,
                timestamp: Header.Timestamp
            );
        }

        public void Notify(ISignalRPort signalR)
        {
            signalR.SendSignalToGroup(SignalRSignal.ARTICLE_UPDATED, ID.ToString(), $"article:{ID}");
        }
    }
}