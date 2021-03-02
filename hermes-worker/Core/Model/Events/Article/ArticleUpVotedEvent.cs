using System;
using Hermes.Worker.Core.Ports;
using Hermes.Worker.Shell;

namespace Hermes.Worker.Core.Model.Events.Article
{
    public record ArticleUpVotedEvent(
        EventHeader<Guid> Header,
        bool InText,
        int SentencePos,
        int TranslationPos,
        string UserID
    ) : IEvent<Guid>
    {
        public void Apply(DBInterpreter interpreter)
        {
            interpreter.InsertUpVote(
                articleID: Header.ID,
                inText: InText,
                sentenceIndex: SentencePos,
                translationIndex: TranslationPos,
                userID: UserID
            );
            interpreter.DeleteDownVote(
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