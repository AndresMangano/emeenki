using System;
using Dapper;
using Hermes.Worker.Core.Repositories;

namespace Hermes.Worker.Shell
{
    public partial class DBInterpreter : IUserPostsRepository
    {
        public void DeleteUserPost(Guid userPostID, Guid? childUserPostID)
        {
            _connection.Execute(@"
                DELETE FROM Query_UserPosts
                WHERE
                    UserPostID = @userPostID AND
                    (@childUserPostID IS NULL OR ChildUserPostID = @childUserPostID)",
                new {
                    userPostID,
                    childUserPostID = childUserPostID
                },
                transaction: _transaction
            );
        }

        public void InsertUserPost(Guid userPostID, Guid? childUserPostID, string userID, string text, string senderUserID, DateTime timestamp)
        {
            _connection.Execute(@"
                INSERT INTO Query_UserPosts(UserPostID, ChildUserPostID, UserID, `Text`, SenderUserID, `Timestamp`)
                VALUES(@userPostID, @childUserPostID, @userID, @text, @senderUserID, @timestamp)
                    ON DUPLICATE KEY UPDATE UserPostID = @userPostID",
                new {
                    userPostID,
                    childUserPostID = childUserPostID ?? Guid.Empty,
                    userID,
                    text,
                    senderUserID,
                    timestamp
                },
                transaction: _transaction
            );
        }
    }
}