using System;
using Hermes.Worker.Core.Ports;
using Hermes.Worker.Shell;

namespace Hermes.Worker.Core.Model.Events.Article
{
    public record ArticleUpVoteRemovedEvent(
        EventHeader<Guid> Header,
        bool InText,
        int SentencePos,
        int TranslationPos,
        string UserID
    ) : IEvent<Guid>
    {
        public void Apply(DBInterpreter interpreter)
        {
            interpreter.DeleteUpVote(
                articleID: Header.ID,
                inText: InText,
                sentenceIndex: SentencePos,
                translationIndex: TranslationPos,
                userID: UserID
            );
        }

        public void Notify(ISignalRPort signalR)
        {
            signalR.SendSignalToGroup(SignalRSignal.ARTICLE_UPDATED, Header.ID.ToString(), $"article:{Header.ID}");
        }
    }
}