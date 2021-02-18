using System;
using Dapper;
using Hermes.Worker.Core.Repositories;
using Hermes.Worker.Core.Repositories.Helpers;

namespace Hermes.Worker.Shell
{
    public partial class DBInterpreter : IArticleTemplateRepository
    {
        public void InsertArticleTemplate(Guid articleTemplateID, bool deleted, string topicID, string languageID, string source, string photoURL, DateTime timestamp)
        {
            _connection.Execute(@"
                INSERT INTO Query_ArticleTemplate(ArticleTemplateID, Deleted, TopicID, LanguageID, `Source`, PhotoURL, `Timestamp`)
                VALUES(@articleTemplateID, @deleted, @topicID, @languageID, @source, @photoURL, @timestamp)
                    ON DUPLICATE KEY UPDATE ArticleTemplateID = @articleTemplateID",
                new {
                    articleTemplateID,
                    deleted,
                    topicID,
                    languageID,
                    source,
                    photoURL,
                    timestamp
                },
                transaction: _transaction
            );
        }

        public void UpdateArticleTemplate(Guid articleTemplateID, DbUpdate<bool> deleted = null)
        {
            if (deleted != null) {
                _connection.Execute(@"
                    UPDATE Query_ArticleTemplate
                    SET Deleted = CASE @setDeleted WHEN 1 THEN @deleted ELSE Deleted END
                    WHERE ArticleTemplateID = @articleTemplateID",
                    new {
                        setDeleted = deleted != null,
                        deleted = deleted?.Value,
                        articleTemplateID
                    },
                    transaction: _transaction
                );
            }
        }
    }
}