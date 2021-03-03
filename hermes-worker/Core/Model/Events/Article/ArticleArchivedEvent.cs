using System;
using System.Threading.Tasks;
using Hermes.Worker.Core.Ports;
using Hermes.Worker.Core.Repositories.Helpers;
using Hermes.Worker.Shell;

namespace Hermes.Worker.Core.Model.Events.Article
{
    public record ArticleArchivedEvent(
        EventHeader Header,
        Guid ID,
        string UserID
    ) : IEvent
    {
        public void Apply(DBInterpreter interpreter)
        {
            interpreter.UpdateArticle(ID,
                archived: new DbUpdate<bool>(true)
            );
            interpreter.UpdateArticles(ID,
                archived: new DbUpdate<bool>(true)
            );
        }

        public void Notify(ISignalRPort signalR)
        {
            signalR.SendSignalToGroup(SignalRSignal.ARTICLE_UPDATED, ID.ToString(),
                $"article:{ID}",
                "articles");
        }
    }
}