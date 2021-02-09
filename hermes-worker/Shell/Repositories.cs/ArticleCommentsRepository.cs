using System;
using Dapper;
using Hermes.Worker.Core.Repositories;
using Hermes.Worker.Core.Repositories.Helpers;

namespace Hermes.Worker.Shell
{
    public partial class DBInterpreter : IArticleCommentsRepository
    {
        public void InsertArticleComment(Guid articleID, int commentIndex, int? childCommentIndex, string comment, string userID, DateTime timestamp) {
            _connection.Execute(@"
                INSERT INTO Query_ArticleComments(ArticleID, CommentIndex, ChildCommentIndex, `Comment`, UserID, `Timestamp`)
                VALUES (@articleID, @commentIndex, @childCommentIndex, @comment, @userID, @timestamp)
                    ON DUPLICATE KEY UPDATE ArticleID = @articleID",
                new {
                    articleID,
                    commentIndex,
                    childCommentIndex,
                    comment,
                    userID,
                    timestamp
                },
                transaction: _transaction
            );
        }

        public void UpdateArticleComment(Guid articleID, int commentIndex, DbUpdate<int?> childCommentIndex, DbUpdate<bool> deleted = null) {
            if (deleted != null) {
                _connection.Execute(@"
                    UPDATE Query_ArticleComments
                    SET Deleted = CASE @setDeleted WHEN 1 THEN @deleted ELSE Deleted END
                    WHERE ArticleID = @articleID AND CommentIndex = @commentIndex AND (@byChild = 0 OR ChildCommentIndex = @childCommentIndex)",
                    new {
                        articleID,
                        commentIndex,
                        childCommentIndex = childCommentIndex?.Value,
                        byChild = childCommentIndex != null,
                        setDeleted = deleted != null,
                        deleted = deleted?.Value
                    },
                    transaction: _transaction
                );
            }
        }
    }
}