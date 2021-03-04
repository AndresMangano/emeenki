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
    public class UserQuery : IUserQueries
    {
        private readonly string _connectionString;

        public UserQuery(SQLConnection connection)
        {
            _connectionString = connection.ConnectionString;
        }

        public async Task<UserDTO> Get(string userID)
        {
            using(MySqlConnection conn = new MySqlConnection(_connectionString)){
                conn.Open();
                return (await conn.QueryAsync<UserDTO>("SELECT * FROM Query_User WHERE UserID = @UserID", new { UserID = userID }))
                    .FirstOrDefault();
            }
        }

        public async Task<UserDetailsDTO> GetDetails(string userID)
        {
            using(MySqlConnection conn = new MySqlConnection(_connectionString)) {
                conn.Open();
                return await conn.QuerySingleAsync<UserDetailsDTO>(@"
                    SELECT
                        U.UserID,
                        U.ProfilePhotoURL,
                        U.NativeLanguageID,
                        U.Description,
                        U.Country,
                        U.SignInType,
                        COALESCE(SUM(R.Points), 0) AS Points
                    FROM Query_User U
                        LEFT JOIN (
                            SELECT T.UserID, (LENGTH(TRIM(T.Translation)) - LENGTH(REPLACE(TRIM(T.Translation), ' ', '')) + 1) AS Points FROM Query_Translation T
                            UNION ALL
                            SELECT UserID, 5 FROM Query_Upvotes
                        ) R ON R.UserID = U.UserID
                    WHERE U.UserID = @UserID
                    GROUP BY U.UserID, U.ProfilePhotoURL, U.NativeLanguageID, U.Description, U.Country, U.SignInType
                    ORDER BY Points DESC", new { UserID = userID });
            }
        }

        public async Task<IEnumerable<UserDTO>> List()
        {
            using(MySqlConnection conn = new MySqlConnection(_connectionString)){
                conn.Open();
                return await conn.QueryAsync<UserDTO>("SELECT * FROM Query_User");
            }
        }

        public async Task<IEnumerable<UserRankingDTO>> GetRanking()
        {
            using(MySqlConnection conn = new MySqlConnection(_connectionString)) {
                conn.Open();
                return await conn.QueryAsync<UserRankingDTO>(@"
                    SELECT
                        U.UserID,
                        U.ProfilePhotoURL,
                        U.NativeLanguageID,
                        SUM(R.Points) AS Points
                    FROM Query_User U
                        JOIN (
                            SELECT T.UserID, (LENGTH(TRIM(T.Translation)) - LENGTH(REPLACE(TRIM(T.Translation), ' ', '')) + 1) AS Points FROM Query_Translation T
                            UNION ALL
                            SELECT UserID, 5 FROM Query_Upvotes
                        ) R ON R.UserID = U.UserID
                    GROUP BY U.UserID, U.ProfilePhotoURL, U.NativeLanguageID
                    ORDER BY Points DESC");
            }
        }

        public async Task<IEnumerable<UserPostDTO>> GetUserPosts(string userID)
        {
            using(MySqlConnection conn = new MySqlConnection(_connectionString)){
                conn.Open();
                return (await conn.QueryAsync<UserPostDTO>(@"
                    SELECT
                        UP.UserPostID,
                        UP.ChildUserPostID,
                        UP.`Text`,
                        UP.SenderUserID,
                        UP.`Timestamp`,
                        U.ProfilePhotoURL
                    FROM Query_UserPosts UP
                        JOIN Query_User U ON U.UserID = UP.SenderUserID
                    WHERE UP.UserID = @UserID
                    ORDER BY UP.`Timestamp` DESC", new { UserID = userID }))
                    .Select(up => {
                        if (up.ChildUserPostID == Guid.Empty) {
                            up.ChildUserPostID = null;
                        }

                        return up;
                    });
            }
        }

        public async Task<UserRankingDTO> GetUserRanking(string userID)
        {
            using(MySqlConnection conn = new MySqlConnection(_connectionString)) {
                conn.Open();
                return await conn.QuerySingleAsync<UserRankingDTO>(@"
                    SELECT
                        U.UserID,
                        U.ProfilePhotoURL,
                        U.NativeLanguageID,
                        SUM(R.Points) AS Points
                    FROM Query_User U
                        JOIN (
                            SELECT T.UserID, (LENGTH(TRIM(T.Translation)) - LENGTH(REPLACE(TRIM(T.Translation), ' ', '')) + 1) AS Points FROM Query_Translation T
                            UNION ALL
                            SELECT UserID, 5 FROM Query_Upvotes
                        ) R ON R.UserID = U.UserID
                    WHERE U.UserID = @UserID
                    GROUP BY U.UserID, U.ProfilePhotoURL, U.NativeLanguageID
                    ORDER BY Points DESC", new { UserID = userID });
            }
        }
    }
}