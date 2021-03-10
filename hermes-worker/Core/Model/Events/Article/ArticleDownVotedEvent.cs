using System;
using Hermes.Worker.Core.Ports;
using Hermes.Worker.Shell;

namespace Hermes.Worker.Core.Model.Events.Article
{
    public record ArticleDownVotedEvent(
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
            interpreter.InsertDownVote(
                articleID: ID,
                inText: InText,
                sentenceIndex: SentencePos,
                translationIndex: TranslationPos,
                userID: UserID
            );
            interpreter.DeleteUpVote(
                articleID: ID,
                inText: InText,
                sentenceIndex: SentencePos,
                translationIndex: TranslationPos,
                userID: UserID
            );
        }

        public void Notify(ISignalRPort signalR)
        {
            signalR.SendSignalToGroup(SignalRSignal.ARTICLE_UPDATED, ID.ToString(),
                SignalRGroup.Article(ID));
        }
    }
}