using System;
using Hermes.Worker.Core.Ports;
using Hermes.Worker.Shell;

namespace Hermes.Worker.Core.Model.Events.Article
{
    public record ArticleTranslatedEvent(
        EventHeader<Guid> Header,
        bool InText,
        int SentencePos,
        int TranslationPos,
        string Translation,
        string UserID
    ) : IEvent<Guid>
    {
        public void Apply(DBInterpreter interpreter)
        {
            interpreter.InsertTranslation(
                articleID:  Header.ID,
                inText: InText,
                sentenceIndex: SentencePos,
                translationIndex: TranslationPos,
                translation: Translation,
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