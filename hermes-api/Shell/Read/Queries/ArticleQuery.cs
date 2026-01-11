using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Hermes.Core;
using Hermes.Shell.Write;
using MySql.Data.MySqlClient;

namespace Hermes.Shell.Read
{
    public class ArticleQuery : IArticleQueries
    {
        private readonly string _connectionString;

        public ArticleQuery(SQLConnection connection)
        {
            _connectionString = connection.ConnectionString;
        }

        public async Task<ArticleDTO> Get(Guid articleID)
        {
            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                var article = await conn.QuerySingleAsync<ArticleDTO>(
                    "SELECT * FROM Query_Article WHERE ArticleID = @ArticleID",
                    new { ArticleID = articleID }
                );

                var sentences = await conn.QueryAsync<ArticleDTO.SentenceDTO>(
                    "SELECT * FROM Query_Sentence WHERE ArticleID = @ArticleID",
                    new { ArticleID = articleID }
                );

                var translations = await conn.QueryAsync<ArticleDTO.TranslationDTO>(@"
                    SELECT T.*, U.ProfilePhotoURL, U.NativeLanguageID
                    FROM Query_Translation T
                        JOIN Query_User U ON U.UserID = T.UserID
                    WHERE ArticleID = @ArticleID",
                    new { ArticleID = articleID }
                );

                var translationComments = await conn.QueryAsync<ArticleDTO.TranslationCommentDTO>(@"
                    SELECT C.*, U.ProfilePhotoURL, U.NativeLanguageID
                    FROM Query_TranslationComment C
	                    JOIN Query_User U ON U.UserID = C.UserID
                    WHERE ArticleID = @ArticleID",
                    new { ArticleID = articleID }
                );

                var upvotes = await conn.QueryAsync<UpvoteDTO>(
                    "SELECT * FROM Query_Upvotes WHERE ArticleID = @ArticleID",
                    new { ArticleID = articleID }
                );

                var downvotes = await conn.QueryAsync<DownvoteDTO>(
                    "SELECT * FROM Query_Downvotes WHERE ArticleID = @ArticleID",
                    new { ArticleID = articleID }
                );

                var comments = await conn.QueryAsync<ArticleDTO.CommentDTO>(@"
                    SELECT AC.*, U.ProfilePhotoURL, U.NativeLanguageID
                    FROM Query_ArticleComments AC
                        JOIN Query_User U ON U.UserID = AC.UserID
                    WHERE ArticleID = @ArticleID AND AC.Deleted = 0",
                    new { ArticleID = articleID }
                );

                foreach (var t in translations)
                {
                    t.Comments = translationComments
                        .Where(c =>
                            c.InText == t.InText &&
                            c.SentenceIndex == t.SentenceIndex &&
                            c.TranslationIndex == t.TranslationIndex)
                        .OrderBy(c => c.CommentIndex);

                    t.Upvotes = upvotes
                        .Where(uv =>
                            uv.InText == t.InText &&
                            uv.SentenceIndex == t.SentenceIndex &&
                            uv.TranslationIndex == t.TranslationIndex)
                        .Select(uv => uv.UserID);

                    t.Downvotes = downvotes
                        .Where(dv =>
                            dv.InText == t.InText &&
                            dv.SentenceIndex == t.SentenceIndex &&
                            dv.TranslationIndex == t.TranslationIndex)
                        .Select(dv => dv.UserID);
                }

                foreach (var s in sentences)
                {
                    s.TranslationHistory = translations
                        .Where(t =>
                            t.InText == s.InText &&
                            t.SentenceIndex == s.SentenceIndex)
                        .OrderByDescending(t => t.TranslationIndex);
                }

                article.title = sentences
                    .Where(s => !s.InText)
                    .OrderBy(s => s.SentenceIndex);

                article.text = sentences
                    .Where(s => s.InText)
                    .OrderBy(s => s.SentenceIndex);

                article.Comments = comments.OrderBy(c => c.CommentIndex);

                return article;
            }
        }

        public async Task<IEnumerable<ArticlesDTO>> Query(string roomID, bool archived)
        {
            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                conn.Open();

                // JOIN Query_Articles (list of room articles) with Query_ArticleTemplate
                // so we can get TopicID from the underlying template.
                var sql = @"
                    SELECT
                        a.ArticleID,
                        a.RoomID,
                        a.Title,
                        a.Created,
                        a.OriginalLanguageID,
                        a.TranslationLanguageID,
                        a.PhotoURL,
                        a.Archived,
                        a.IsVideo,
                        a.VideoURL,
                        a.ArticleTemplateID,
                        t.TopicID
                    FROM Query_Articles a
                    LEFT JOIN Query_ArticleTemplate t
                        ON a.ArticleTemplateID = t.ArticleTemplateID
                    WHERE a.RoomID = @RoomID
                      AND a.Archived = @Archived";

                return await conn.QueryAsync<ArticlesDTO>(
                    sql,
                    new { RoomID = roomID, Archived = archived }
                );
            }
        }

        public async Task<IEnumerable<ActivityDTO>> GetActivityFeed()
        {
            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                return await conn.QueryAsync<ActivityDTO>(@"
                    SELECT
                        U.ProfilePhotoURL,
                        U.UserID,
                        'added a translation to' AS `Event`,
                        A.Title,
                        (LENGTH(TRIM(T.Translation)) - LENGTH(REPLACE(TRIM(T.Translation), ' ', '')) + 1) AS Points,
                        T.`Timestamp`
                    FROM Query_Translation T
                        JOIN Query_Articles A ON A.ArticleID = T.ArticleID
                        JOIN Query_User U ON T.UserID = U.UserID
                    WHERE HOUR(TIMEDIFF(UTC_TIMESTAMP(), T.`Timestamp`)) < 24
                    ORDER BY T.`Timestamp` DESC");
            }
        }

        public async Task<IEnumerable<ActivityDTO>> GetUserActivity(string userID)
        {
            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                return await conn.QueryAsync<ActivityDTO>(@"
                    SELECT
                        U.ProfilePhotoURL,
                        U.UserID,
                        'added a translation to' AS `Event`,
                        A.Title,
                        (LENGTH(TRIM(T.Translation)) - LENGTH(REPLACE(TRIM(T.Translation), ' ', '')) + 1) AS Points,
                        T.`Timestamp`
                    FROM Query_Translation T
                        JOIN Query_Articles A ON A.ArticleID = T.ArticleID
                        JOIN Query_User U ON T.UserID = U.UserID
                    WHERE U.UserID = @UserID
                      AND HOUR(TIMEDIFF(UTC_TIMESTAMP(), T.`Timestamp`)) < 24
                    ORDER BY T.`Timestamp` DESC",
                    new { UserID = userID });
            }
        }
    }
}
