using System;
using Hermes.Worker.Core.Ports;
using Hermes.Worker.Shell;

namespace Hermes.Worker.Core.Model.Events.Article
{
    public record ArticleTranslatedEvent(
        EventHeader Header,
        Guid ID,
        bool InText,
        int SentencePos,
        int TranslationPos,
        string Translation,
        string UserID
    ) : IEvent
    {
        public void Apply(DBInterpreter interpreter)
        {
            interpreter.InsertTranslation(
                articleID:  ID,
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
            signalR.SendSignalToGroup(SignalRSignal.ARTICLE_UPDATED, ID.ToString(),
                SignalRGroup.Article(ID));
        }
    }
}