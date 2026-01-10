using System;
using System.Collections.Generic;
using Hermes.Worker.Core.Ports;
using Hermes.Worker.Shell;

namespace Hermes.Worker.Core.Model.Events.ArticleTemplate
{
    public record VideoSentence(
        string Text,
        long? StartTimeMs,
        long? EndTimeMs
    );

    public record ArticleTemplateVideoUploadedEvent(
        EventHeader Header,
        Guid ID,
        string TopicID,
        string LanguageID,
        List<VideoSentence> Title,
        List<VideoSentence> Text,
        string Source,
        string PhotoURL,
        string VideoURL
    ) : IEvent
    {
        public void Apply(DBInterpreter interpreter)
        {
            // Insert into Query_ArticleTemplates
            var titleTexts = new List<string>();
            foreach (var t in Title)
            {
                titleTexts.Add(t.Text);
            }

            interpreter.InsertArticleTemplates(
                articleTemplateID: ID,
                title: String.Join(" ", titleTexts),
                created: Header.Timestamp,
                topicID: TopicID,
                languageID: LanguageID,
                photoURL: PhotoURL,
                archived: false
            );

            // Insert into Query_ArticleTemplate with video fields
            interpreter.InsertArticleTemplateWithVideo(
                articleTemplateID: ID,
                deleted: false,
                topicID: TopicID,
                languageID: LanguageID,
                source: Source,
                photoURL: PhotoURL,
                timestamp: Header.Timestamp,
                isVideo: true,
                videoURL: VideoURL
            );

            // Insert text sentences with timestamps
            for (var index = 0; index < Text.Count; index++)
            {
                interpreter.InsertArticleTemplateSentenceWithTimestamp(
                    articleTemplateID: ID,
                    inText: true,
                    sentenceIndex: index,
                    sentence: Text[index].Text,
                    startTimeMs: Text[index].StartTimeMs,
                    endTimeMs: Text[index].EndTimeMs
                );
            }

            // Insert title sentences
            for (var index = 0; index < Title.Count; index++)
            {
                interpreter.InsertArticleTemplateSentenceWithTimestamp(
                    articleTemplateID: ID,
                    inText: false,
                    sentenceIndex: index,
                    sentence: Title[index].Text,
                    startTimeMs: Title[index].StartTimeMs,
                    endTimeMs: Title[index].EndTimeMs
                );
            }
        }

        public void Notify(ISignalRPort signalR)
        {
            signalR.SendSignalToGroup(SignalRSignal.ARTICLE_TEMPLATE_UPDATED, ID.ToString(),
                SignalRGroup.ARTICLE_TEMPLATES);
        }
    }
}




