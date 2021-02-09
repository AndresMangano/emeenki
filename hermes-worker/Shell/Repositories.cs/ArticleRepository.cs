using System;
using Dapper;
using Hermes.Worker.Core.Repositories;
using Hermes.Worker.Core.Repositories.Helpers;

namespace Hermes.Worker.Shell
{
    public partial class DBInterpreter : IArticleRepository
    {
        public void InsertArticle(Guid articleID, Guid articleTemplateID, bool archived, string roomID, string originalLanguageID, string translationLanguageID, string source, string photoURL, DateTime timestamp)
        {
            _connection.Execute(@"
                INSERT INTO Query_Article(ArticleID, ArticleTemplateID, Archived, RoomID, OriginalLanguageID, TranslationLanguageID, `Source`, PhotoURL, `Timestamp`)
                VALUES(@articleID, @articleTemplateID, @archived, @roomID, @originalLanguageID, @translationLanguageID, @source, @photoURL, @timestamp)
                    ON DUPLICATE KEY UPDATE ArticleID = @articleID",
                new {
                    articleID,
                    articleTemplateID,
                    archived,
                    roomID,
                    originalLanguageID,
                    translationLanguageID,
                    source,
                    photoURL,
                    timestamp
                },
                transaction: _transaction
            );
        }

        public void UpdateArticle(Guid articleID, DbUpdate<bool> archived = null)
        {
            if (archived != null) {
                _connection.Execute(@"
                    UPDATE Query_Article
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