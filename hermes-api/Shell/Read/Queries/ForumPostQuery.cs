using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Hermes.Core;
using MySql.Data.MySqlClient;

namespace Hermes.Shell.Read
{
    public class ForumPostQuery : IForumPostQueries
    {
        readonly string _connectionString;

        public ForumPostQuery(SQLConnection connection)
        {
            _connectionString = connection.ConnectionString;
        }

        public async Task<ForumPostDTO> Get(Guid forumPostID)
        {
            using (var conn = new MySqlConnection(_connectionString)) {
                conn.Open();
                return await conn.QuerySingleAsync<ForumPostDTO>(@"
                    SELECT FP.*, U.ProfilePhotoURL
                    FROM Query_ForumPosts FP
                        JOIN Query_User U ON U.UserID = FP.UserID
                    WHERE FP.ID = @forumPostID
                    LIMIT 1",
                    new {
                        forumPostID
                    });
            }
        }

        public async Task<IEnumerable<ForumPostCommentDTO>> GetComments(Guid forumPostID)
        {
            using (var conn = new MySqlConnection(_connectionString)) {
                conn.Open();
                return await conn.QueryAsync<ForumPostCommentDTO>(@"
                    SELECT FPC.*, U.ProfilePhotoURL
                    FROM Query_ForumPostComments FPC
                        JOIN Query_User U ON U.UserID = FPC.UserID
                    WHERE FPC.ForumPostID = @forumPostID
                    ORDER BY FPC.`Timestamp` ASC",
                    new {
                        forumPostID
                    });
            }
        }

        public async Task<IEnumerable<ForumPostDTO>> Query(string languageID)
        {
            using (var conn = new MySqlConnection(_connectionString)) {
                conn.Open();
                return await conn.QueryAsync<ForumPostDTO>(@"
                    SELECT FP.*, U.ProfilePhotoURL
                    FROM Query_ForumPosts FP
                        JOIN Query_User U ON U.UserID = FP.UserID
                    WHERE (@languageID IS NULL OR @languageID = FP.LanguageID)
                    ORDER BY COALESCE(FP.LastCommentTimestamp, FP.`Timestamp`) DESC",
                    new {
                        languageID
                    });
            }
        }
    }
}