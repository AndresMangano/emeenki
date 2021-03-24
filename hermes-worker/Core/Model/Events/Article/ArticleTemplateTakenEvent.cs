using System;
using System.Collections.Generic;
using Hermes.Worker.Core.Ports;
using Hermes.Worker.Shell;

namespace Hermes.Worker.Core.Model.Events.Article
{
    public record ArticleTemplateTakenEvent(
        EventHeader Header,
        Guid ID,
        Guid ArticleTemplateID,
        string RoomID,
        string OriginalLanguageID,
        string TranslationLanguageID,
        List<string> Title,
        List<string> Text,
        string Source,
        string PhotoURL
    ) : IEvent
    {
        public void Apply(DBInterpreter interpreter)
        {
            interpreter.InsertArticles(
                articleID: ID,
                roomID: RoomID,
                title: String.Join(" ", Title),
                created: Header.Timestamp,
                originalLanguageID: OriginalLanguageID,
                translationLanguageID: TranslationLanguageID,
                photoURL: PhotoURL,
                archived: false
            );
            interpreter.InsertArticle(
                articleID: ID,
                articleTemplateID: ArticleTemplateID,
                archived: false,
                roomID: RoomID,
                originalLanguageID: OriginalLanguageID,
                translationLanguageID: TranslationLanguageID,
                source: Source,
                photoURL: PhotoURL,
                timestamp: Header.Timestamp
            );
            for (var index = 0; index < Text.Count; index++) {
                interpreter.InsertSentence(
                    articleID: ID,
                    inText: true,
                    sentenceIndex: index,
                    originalText: Text[index]
                );
            }
            for (var index = 0; index < Title.Count; index++) {
                interpreter.InsertSentence(
                    articleID: ID,
                    inText: false,
                    sentenceIndex: index,
                    originalText: Title[index]
                );
            }
        }

        public void Notify(ISignalRPort signalR)
        {
            signalR.SendSignalToGroup(SignalRSignal.ARTICLE_UPDATED, ID.ToString(),
                SignalRGroup.ARTICLES);
        }
    }
}