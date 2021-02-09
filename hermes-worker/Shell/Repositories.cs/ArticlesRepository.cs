using System;
using Dapper;
using Hermes.Worker.Core.Repositories;
using Hermes.Worker.Core.Repositories.Helpers;

namespace Hermes.Worker.Shell
{
    public partial class DBInterpreter : IArticlesRepository
    {
        public void InsertArticles(Guid articleID, string roomID, string title, DateTime created, string originalLanguageID, string translationLanguageID, string photoURL, bool archived)
        {
            _connection.Execute(@"
                INSERT INTO Query_Articles(ArticleID, RoomID, Title, Created, OriginalLanguageID, TranslationLanguageID, PhotoURL, Archived)
                VALUES(@articleID, @roomID, @title, @created, @originalLanguageID, @translationLanguageID, @photoURL, @archived)
                    ON DUPLICATE KEY UPDATE ArticleID = @articleID",
                new {
                    articleID,
                    roomID,
                    title,
                    created,
                    originalLanguageID,
                    translationLanguageID,
                    photoURL,
                    archived
                },
                transaction: _transaction
            );
        }

        public void UpdateArticles(Guid articleID, DbUpdate<bool> archived = null)
        {
            if (archived != null) {
                _connection.Execute(@"
                    UPDATE Query_Articles
                    SET Archived = CASE @setArchived WHEN 1 THEN @archived ELSE Archived END
                    WHERE ArticleID = @articleID",
                    new {
                        articleID,
                        setArchived = archived != null,
                        archived = archived?.Value
                    },
                    transaction: _transaction
                );
            }
        }
    }
}