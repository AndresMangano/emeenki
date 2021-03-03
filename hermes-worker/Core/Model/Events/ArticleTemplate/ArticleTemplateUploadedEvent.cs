using System;
using System.Collections.Generic;
using Hermes.Worker.Core.Ports;
using Hermes.Worker.Shell;

namespace Hermes.Worker.Core.Model.Events.ArticleTemplate
{
    public record ArticleTemplateUploadedEvent(
        EventHeader Header,
        Guid ID,
        string TopicID,
        string LanguageID,
        List<string> Title,
        List<string> Text,
        string Source,
        string PhotoURL
    ) : IEvent
    {
        public void Apply(DBInterpreter interpreter)
        {
            interpreter.InsertArticleTemplates(
                articleTemplateID: ID,
                title: String.Join(" ", Title),
                created: Header.Timestamp,
                topicID: TopicID,
                languageID: LanguageID,
                photoURL: PhotoURL,
                archived: false
            );
            interpreter.InsertArticleTemplate(
                articleTemplateID: ID,
                deleted: false,
                topicID: TopicID,
                languageID: LanguageID,
                source: Source,
                photoURL: PhotoURL,
                timestamp: Header.Timestamp
            );
            for (var index = 0; index < Text.Count; index++) {
                interpreter.InsertArticleTemplateSentence(
                    articleTemplateID: ID,
                    inText: true,
                    sentenceIndex: index,
                    sentence: Text[index]
                );
            }
            for (var index = 0; index < Title.Count; index++) {
                interpreter.InsertArticleTemplateSentence(
                    articleTemplateID: ID,
                    inText: false,
                    sentenceIndex: index,
                    sentence: Title[index]
                );
            }
        }

        public void Notify(ISignalRPort signalR)
        {
            signalR.SendSignalToGroup(SignalRSignal.ARTICLE_TEMPLATE_UPDATED, ID.ToString(), "articleTemplates");
        }
    }
}