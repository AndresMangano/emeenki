using System;
using Hermes.Worker.Core.Ports;
using Hermes.Worker.Shell;

namespace Hermes.Worker.Core.Model.Events.Article
{
    public record ArticleMainCommentedEvent(
        EventHeader<Guid> Header,
        int CommentPos,
        int? ChildCommentPos,
        string Comment,
        string UserID
    ) : IEvent<Guid>
    {
        public void Apply(DBInterpreter interpreter)
        {
            interpreter.InsertArticleComment(
                articleID: Header.ID,
                commentIndex: CommentPos,
                childCommentIndex: ChildCommentPos,
                comment: Comment,
                userID: UserID,
                timestamp: Header.Timestamp
            );
        }

        public void Notify(ISignalRPort signalR)
        {
            signalR.SendSignalToGroup(SignalRSignal.ARTICLE_UPDATED, Header.ID.ToString(), $"article:{Header.ID}");
        }
    }
}