using System;
using Dapper;
using Hermes.Worker.Core.Repositories;

namespace Hermes.Worker.Shell
{
    public partial class DBInterpreter : ITranslationCommentRepository
    {
        public void InsertTranslationComment(Guid articleID, bool inText, int sentenceIndex, int translationIndex, int commentIndex, string comment, string userID, DateTime timestamp)
        {
            _connection.Execute(@"
                INSERT INTO Query_TranslationComment(ArticleID, InText, SentenceIndex, TranslationIndex, CommentIndex, `Comment`, UserID, `Timestamp`)
                VALUES(@articleID, @inText, @sentenceIndex, @translationIndex, @commentIndex, @comment, @userID, @timestamp)
                    ON DUPLICATE KEY UPDATE ArticleID = @articleID",
                new {
                    articleID,
                    inText,
                    sentenceIndex,
                    translationIndex,
                    commentIndex,
                    comment,
                    userID,
                    timestamp
                },
                transaction: _transaction
            );
        }
    }
}