using System;
using Dapper;
using Hermes.Worker.Core.Repositories;

namespace Hermes.Worker.Shell
{
    public partial class DBInterpreter : ISentenceRepository
    {
        public void InsertSentence(Guid articleID, bool inText, int sentenceIndex, string originalText)
        {
            _connection.Execute(@"
                INSERT INTO Query_Sentence(ArticleID, InText, SentenceIndex, OriginalText)
                VALUES(@articleID, @inText, @sentenceIndex, @originalText)
                    ON DUPLICATE KEY UPDATE ArticleID = @articleID",
                new {
                    articleID,
                    inText,
                    sentenceIndex,
                    originalText
                },
                transaction: _transaction
            );
        }
    }
}