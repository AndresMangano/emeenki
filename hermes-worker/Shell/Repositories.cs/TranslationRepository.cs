using System;
using Dapper;
using Hermes.Worker.Core.Repositories;

namespace Hermes.Worker.Shell
{
    public partial class DBInterpreter : ITranslationRepository
    {
        public void InsertTranslation(Guid articleID, bool inText, int sentenceIndex, int translationIndex, string translation, string userID, DateTime timestamp)
        {
            _connection.Execute(@"
                INSERT INTO Query_Translation(ArticleID, InText, SentenceIndex, TranslationIndex, Translation, UserID, `Timestamp`)
                VALUES(@articleID, @inText, @sentenceIndex, @translationIndex, @translation, @userID, @timestamp)
                    ON DUPLICATE KEY UPDATE
                        Translation = @translation,
                        `Timestamp` = @timestamp",
                new {
                    articleID,
                    inText,
                    sentenceIndex,
                    translationIndex,
                    translation,
                    userID,
                    timestamp
                },
                transaction: _transaction
            );
        }
    }
}