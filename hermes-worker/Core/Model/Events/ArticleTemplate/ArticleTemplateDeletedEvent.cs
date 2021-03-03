using System;
using Hermes.Worker.Core.Ports;
using Hermes.Worker.Core.Repositories.Helpers;
using Hermes.Worker.Shell;

namespace Hermes.Worker.Core.Model.Events.ArticleTemplate
{
    public record ArticleTemplateDeletedEvent(
        EventHeader Header,
        Guid ID,
        string UserID
    ) : IEvent
    {
        public void Apply(DBInterpreter interpreter)
        {
            interpreter.UpdateArticleTemplate(ID,
                deleted: new DbUpdate<bool>(true));
            interpreter.UpdateArticleTemplates(ID,
                archived: new DbUpdate<bool>(true));
        }

        public void Notify(ISignalRPort signalR)
        {
            signalR.SendSignalToGroup(SignalRSignal.ARTICLE_TEMPLATE_UPDATED, ID.ToString(), "articleTemplates");
        }
    }
}