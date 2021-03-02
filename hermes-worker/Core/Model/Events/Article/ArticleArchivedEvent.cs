using System;
using System.Threading.Tasks;
using Hermes.Worker.Core.Ports;
using Hermes.Worker.Core.Repositories.Helpers;
using Hermes.Worker.Shell;

namespace Hermes.Worker.Core.Model.Events.Article
{
    public record ArticleArchivedEvent(
        EventHeader<Guid> Header,
        string UserID
    ) : IEvent<Guid>
    {
        public void Apply(DBInterpreter interpreter)
        {
            interpreter.UpdateArticle(Header.ID,
                archived: new DbUpdate<bool>(true)
            );
            interpreter.UpdateArticles(Header.ID,
                archived: new DbUpdate<bool>(true)
            );
        }

        public void Notify(ISignalRPort signalR)
        {
            signalR.SendSignalToGroup(SignalRSignal.ARTICLE_UPDATED, Header.ID.ToString(),
                $"article:{Header.ID}",
                "articles");
        }
    }
}