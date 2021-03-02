using System;
using Hermes.Worker.Core.Ports;
using Hermes.Worker.Core.Repositories.Helpers;
using Hermes.Worker.Shell;

namespace Hermes.Worker.Core.Model.Events.ArticleTemplate
{
    public record ArticleTemplateDeletedEvent(
        EventHeader<Guid> Header,
        string UserID
    ) : IEvent<Guid>
    {
        public void Apply(DBInterpreter interpreter)
        {
            interpreter.UpdateArticleTemplate(Header.ID,
                deleted: new DbUpdate<bool>(true));
            interpreter.UpdateArticleTemplates(Header.ID,
                archived: new DbUpdate<bool>(true));
        }

        public void Notify(ISignalRPort signalR)
        {
            signalR.SendSignalToGroup(SignalRSignal.ARTICLE_TEMPLATE_UPDATED, Header.ID.ToString(), "articleTemplates");
        }
    }
}