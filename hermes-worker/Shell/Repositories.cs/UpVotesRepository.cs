using System;
using Dapper;
using Hermes.Worker.Core.Repositories;

namespace Hermes.Worker.Shell
{
    public partial class DBInterpreter : IUpVotesRepository
    {
        public void DeleteUpVote(Guid articleID, bool inText, int sentenceIndex, int translationIndex, string userID)
        {
            _connection.Execute(@"
                DELETE FROM Query_Upvotes 
                WHERE   ArticleID = @articleID AND
                        InText = @inText AND
                        SentenceIndex = @sentenceIndex AND
                        TranslationIndex = @translationIndex AND
                        UserID = @userID",
                new {
                    articleID,
                    inText,
                    sentenceIndex,
                    translationIndex,
                    userID
                },
                transaction: _transaction
            );
        }

        public void InsertUpVote(Guid articleID, bool inText, int sentenceIndex, int translationIndex, string userID)
        {
            _connection.Execute(@"
                INSERT INTO Query_Upvotes(ArticleID, InText, SentenceIndex, TranslationIndex, UserID)
                VALUES(@articleID, @inText, @sentenceIndex, @translationIndex, @userID)
                    ON DUPLICATE KEY UPDATE ArticleID = @articleID",
                new {
                    articleID,
                    inText,
                    sentenceIndex,
                    translationIndex,
                    userID
                },
                transaction: _transaction
            );
        }
    }
}