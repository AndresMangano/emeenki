using System;
using Hermes.Worker.Core.Ports;
using Hermes.Worker.Shell;

namespace Hermes.Worker.Core.Model.Events.Article
{
    public record ArticleDownVoteRemovedEvent(
        EventHeader Header,
        Guid ID,
        bool InText,
        int SentencePos,
        int TranslationPos,
        string UserID
    ) : IEvent
    {
        public void Apply(DBInterpreter interpreter)
        {
            interpreter.DeleteDownVote(
                articleID: ID,
                inText: InText,
                sentenceIndex: SentencePos,
                translationIndex: TranslationPos,
                userID: UserID
            );
        }

        public void Notify(ISignalRPort signalR)
        {
            signalR.SendSignalToGroup(SignalRSignal.ARTICLE_UPDATED, ID.ToString(), $"article:{ID}");
        }
    }
}