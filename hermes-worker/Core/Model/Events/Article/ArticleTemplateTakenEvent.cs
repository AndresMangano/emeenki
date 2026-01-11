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
            // Determine if the underlying template is a video template.
            // For video templates created via YouTube upload, Source is "youtube".
            var isVideo =
                !string.IsNullOrWhiteSpace(Source) &&
                Source.Equals("youtube", StringComparison.OrdinalIgnoreCase);

            // Insert into Query_Articles (list table) with video metadata.
            interpreter.InsertArticles(
                articleID: ID,
                roomID: RoomID,
                title: string.Join(" ", Title),
                created: Header.Timestamp,
                originalLanguageID: OriginalLanguageID,
                translationLanguageID: TranslationLanguageID,
                photoURL: PhotoURL,
                archived: false,
                isVideo: isVideo,
                videoURL: null,                   // optional; ArticleCard will fall back to photoURL
                articleTemplateID: ArticleTemplateID
            );

            // Insert into Query_Article (detail table) as before.
            // If you later add video columns here too, you can switch to an InsertArticleWithVideo
            // variant, similar to InsertArticleTemplateWithVideo used for templates.
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

            // Insert text sentences
            for (var index = 0; index < Text.Count; index++)
            {
                interpreter.InsertSentence(
                    articleID: ID,
                    inText: true,
                    sentenceIndex: index,
                    originalText: Text[index]
                );
            }

            // Insert title sentences
            for (var index = 0; index < Title.Count; index++)
            {
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
            signalR.SendSignalToGroup(
                SignalRSignal.ARTICLE_UPDATED,
                ID.ToString(),
                SignalRGroup.ARTICLES
            );
        }
    }
}
