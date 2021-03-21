using System;
using Dapper;
using Hermes.Worker.Core.Repositories;
using Hermes.Worker.Core.Repositories.Helpers;

namespace Hermes.Worker.Shell
{
    public partial class DBInterpreter : IForumPostRepository
    {
        public void DeleteForumPost(Guid ID)
        {
            _connection.Execute(@"
                DELETE FROM Query_ForumPosts
                WHERE ID = @ID",
                new { ID },
                _transaction
            );
        }

        public void InsertForumPost(Guid ID, string title, string text, string languageID, string userID, DateTime timestamp)
        {
            _connection.Execute(@"
                INSERT INTO Query_ForumPosts(ID, Title, `Text`, LanguageID, UserID, `Timestamp`)
                VALUES (@ID, @title, @text, @languageID, @userID, @timestamp)
                    ON DUPLICATE KEY UPDATE ID = @ID",
                new {
                    ID,
                    title,
                    text,
                    languageID,
                    userID,
                    timestamp
                },
                _transaction
            );
        }

        public void UpdateForumPost(Guid ID, DbUpdate<string> title = null, DbUpdate<string> text = null, DbUpdate<string> languageID = null, DbUpdate<DateTime> modifiedOn = null,
            DbUpdate<string> lastCommentUserID = null, DbUpdate<DateTime> lastCommentTimestamp = null)
        {
            _connection.Execute(@"
                UPDATE Query_ForumPosts
                SET Title = CASE WHEN @setTitle = 1 THEN @title ELSE Title END,
                    `Text` = CASE WHEN @setText = 1 THEN @text ELSE `Text` END,
                    LanguageID = CASE WHEN @setLanguageID = 1 THEN @languageID ELSE LanguageID END,
                    LastCommentUserID = CASE WHEN @setLastCommentUserID = 1 THEN @lastCommentUserID ELSE LastCommentUserID END,
                    LastCommentTimestamp = CASE WHEN @setLastCommentTimestamp = 1 THEN @lastCommentTimestamp ELSE LastCommentTimestamp END
                WHERE ID = @ID",
                new {
                    ID,
                    setTitle = title != null,
                    title = title.Value,
                    setText = text != null,
                    text = text.Value,
                    setLanguageID = languageID != null,
                    languageID = languageID.Value,
                    setLastCommentUserID = lastCommentUserID != null,
                    lastCommentUserID,
                    setLastCommentTimestamp = lastCommentTimestamp != null,
                    lastCommentTimestamp
                },
                _transaction
            );
        }
    }
}