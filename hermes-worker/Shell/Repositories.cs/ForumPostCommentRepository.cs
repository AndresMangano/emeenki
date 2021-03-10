using System;
using Dapper;
using Hermes.Worker.Core.Repositories;

namespace Hermes.Worker.Shell
{
    public partial class DBInterpreter : IForumPostCommentRepository
    {
        public void DeleteForumPostComment(Guid ID)
        {
            _connection.Execute(@"
                DELETE FROM Query_ForumPostComments
                WHERE ID = @ID",
                new {
                    ID
                },
                _transaction
            );
        }

        public void InsertForumPostComment(Guid ID, Guid forumPostID, string text, string userID, DateTime timestamp)
        {
            _connection.Execute(@"
                INSERT INTO Query_ForumPostComments(ID, ForumPostID, `Text`, UserID, `Timestamp`)
                VALUES(@ID, @forumPostID, @text, @userID, @timestamp)
                    ON DUPLICATE KEY UPDATE ID = @ID",
                new {
                    ID,
                    forumPostID,
                    text,
                    userID,
                    timestamp
                },
                _transaction
            );
        }
    }
}