using System;
using Hermes.Worker.Core.Ports;
using Hermes.Worker.Core.Repositories.Helpers;
using Hermes.Worker.Shell;

namespace Hermes.Worker.Core.Model.Events.Article
{
    public record ArticleMainCommentDeletedEvent(
        EventHeader<Guid> Header,
        int CommentPos,
        int? ChildCommentPos
    ) : IEvent<Guid>
    {
        public void Apply(DBInterpreter interpreter)
        {
            interpreter.UpdateArticleComment(Header.ID, CommentPos,
                childCommentIndex: ChildCommentPos == null ? null : new DbUpdate<int?>(ChildCommentPos),
                deleted: new DbUpdate<bool>(true)
            );
        }

        public void Notify(ISignalRPort signalR)
        {
            signalR.SendSignalToGroup(SignalRSignal.ARTICLE_UPDATED, Header.ID.ToString(), $"article:{Header.ID}");
        }
    }
}