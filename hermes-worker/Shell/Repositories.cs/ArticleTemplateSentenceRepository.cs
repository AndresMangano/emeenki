using System;
using Dapper;
using Hermes.Worker.Core.Repositories;

namespace Hermes.Worker.Shell
{
    public partial class DBInterpreter : IArticleTemplateSentenceRepository
    {
        public void InsertArticleTemplateSentence(Guid articleTemplateID, bool inText, int sentenceIndex, string sentence)
        {
            _connection.Execute(@"
                INSERT INTO Query_ArticleTemplateSentence(ArticleTemplateID, InText, SentenceIndex, Sentence)
                VALUES(@articleTemplateID, @inText, @sentenceIndex, @sentence)
                    ON DUPLICATE KEY UPDATE ArticleTemplateID = @articleTemplateID",
                new {
                    articleTemplateID,
                    inText,
                    sentenceIndex,
                    sentence
                },
                transaction: _transaction
            );
        }

        public void InsertArticleTemplateSentenceWithTimestamp(Guid articleTemplateID, bool inText, int sentenceIndex, string sentence, long? startTimeMs, long? endTimeMs)
        {
            _connection.Execute(@"
                INSERT INTO Query_ArticleTemplateSentence(ArticleTemplateID, InText, SentenceIndex, Sentence, StartTimeMs, EndTimeMs)
                VALUES(@articleTemplateID, @inText, @sentenceIndex, @sentence, @startTimeMs, @endTimeMs)
                    ON DUPLICATE KEY UPDATE ArticleTemplateID = @articleTemplateID",
                new {
                    articleTemplateID,
                    inText,
                    sentenceIndex,
                    sentence,
                    startTimeMs,
                    endTimeMs
                },
                transaction: _transaction
            );
        }
    }
}