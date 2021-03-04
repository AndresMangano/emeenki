using System;
using Dapper;
using Hermes.Worker.Core.Repositories;
using Hermes.Worker.Core.Repositories.Helpers;

namespace Hermes.Worker.Shell
{
    public partial class DBInterpreter : IArticleTemplatesRepository
    {
        public void InsertArticleTemplates(Guid articleTemplateID, string title, DateTime created, string topicID, string languageID, string photoURL, bool archived)
        {
            _connection.Execute(@"
                INSERT INTO Query_ArticleTemplates(ArticleTemplateID, Title, Created, TopicID, LanguageID, PhotoURL, Archived)
                VALUES(@articleTemplateID, @title, @created, @topicID, @languageID, @photoURL, @archived)
                    ON DUPLICATE KEY UPDATE ArticleTemplateID = @articleTemplateID",
                new {
                    articleTemplateID,
                    title,
                    created,
                    topicID,
                    languageID,
                    photoURL,
                    archived
                },
                transaction: _transaction
            );
        }

        public void UpdateArticleTemplates(Guid articleTemplateID, DbUpdate<bool> archived = null)
        {
            if (archived != null) {
                _connection.Execute(
                    @"  UPDATE Query_ArticleTemplates
                        SET Archived = CASE @setArchived WHEN 1 THEN @archived ELSE Archived END
                        WHERE ArticleTemplateID = @articleTemplateID",
                    new {
                        setArchived = archived != null,
                        archived = archived?.Value,
                        articleTemplateID
                    },
                    transaction: _transaction
                );
            }
        }
    }
}