using System;
using Hermes.Worker.Core.Ports;
using Hermes.Worker.Core.Repositories.Helpers;
using Hermes.Worker.Shell;

namespace Hermes.Worker.Core.Model.Events.Article
{
    public record ArticleMainCommentDeletedEvent(
        EventHeader Header,
        Guid ID,
        int CommentPos,
        int? ChildCommentPos
    ) : IEvent
    {
        public void Apply(DBInterpreter interpreter)
        {
            interpreter.UpdateArticleComment(ID, CommentPos,
                childCommentIndex: ChildCommentPos == null ? null : new DbUpdate<int?>(ChildCommentPos),
                deleted: new DbUpdate<bool>(true)
            );
        }

        public void Notify(ISignalRPort signalR)
        {
            signalR.SendSignalToGroup(SignalRSignal.ARTICLE_UPDATED, ID.ToString(),
                SignalRGroup.Article(ID));
        }
    }
}