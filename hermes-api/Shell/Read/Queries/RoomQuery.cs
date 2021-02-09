using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Hermes.Core;
using Hermes.Shell.Write;
using MySql.Data.MySqlClient;

namespace Hermes.Shell.Write
{
    public class RoomQuery : IRoomQueries
    {
        private readonly string _connectionString;

        public RoomQuery(SQLConnection connection)
        {
            _connectionString = connection.ConnectionString;
        }

        public async Task<RoomDTO> Get(string roomID)
        {
            using(MySqlConnection conn = new MySqlConnection(_connectionString)){
                conn.Open();
                var room = await conn.QuerySingleAsync<RoomDTO>(
                    "SELECT * FROM Query_Room WHERE RoomID = @RoomID", new { RoomID = roomID }
                );
                var roomUsers = await conn.QueryAsync<RoomUserDTO>(
                    "SELECT * FROM Query_RoomUser WHERE RoomID = @RoomID", new { RoomID = roomID }
                );
                var usersQueue = await conn.QueryAsync<RoomQueueDTO>(
                    "SELECT * FROM Query_RoomQueue WHERE RoomID = @RoomID", new { RoomID = roomID }
                );
                room.Users = roomUsers;
                room.UsersQueue = usersQueue.Select(uq => uq.UserID);

                return room;
            }
        }

        public async Task<IEnumerable<RoomUsersDTO>> GetUsers(string roomID)
        {
            using(MySqlConnection conn = new MySqlConnection(_connectionString)){
                conn.Open();
                return await conn.QueryAsync<RoomUsersDTO>(
                    @"  SELECT
                            R.RoomID,
                            RU.UserID AS Username,
                            U.NativeLanguageID,
                            U.ProfilePhotoURL AS PhotoURL,
                            RU.Permission AS Rights,
                            0
                        FROM Query_Room R
                            JOIN Query_RoomUser RU ON RU.RoomID = R.RoomID
                            JOIN Query_User U ON RU.UserID = U.UserID
                        WHERE   R.RoomID = @RoomID", new { RoomID = roomID }
                );
            }
        }

        public async Task<IEnumerable<RoomUsersDTO>> GetPendingUsers(string roomID)
        {
            using(MySqlConnection conn = new MySqlConnection(_connectionString)){
                conn.Open();
                return await conn.QueryAsync<RoomUsersDTO>(
                    @"  SELECT
                            R.RoomID,
                            RQ.UserID AS Username,
                            U.NativeLanguageID,
                            U.ProfilePhotoURL AS PhotoURL,
                            NULL AS Rights,
                            1
                        FROM Query_Room R
                            JOIN Query_RoomQueue RQ ON RQ.RoomID = R.RoomID
                            JOIN Query_User U ON RQ.UserID = U.UserID
                        WHERE   R.RoomID = @RoomID", new { RoomID = roomID }
                );
            }
        }

        public async Task<IEnumerable<RoomDTO>> Query(string filter, string userID, string languageID1, string languageID2)
        {
            using(MySqlConnection conn = new MySqlConnection(_connectionString)){
                conn.Open();
                var rooms = await conn.QueryAsync<RoomDTO>(
                    @"  SELECT * FROM Query_Room
                        WHERE   Closed = 0 AND
                                ((LanguageID1 = @LanguageID1 AND LanguageID2 = @LanguageID2) OR (LanguageID1 = @LanguageID2 AND LanguageID2 = @LanguageID1))",
                    new { LanguageID1 = languageID1, LanguageID2 = languageID2 }
                );
                var roomUsers = await conn.QueryAsync<RoomUserDTO>("SELECT * FROM Query_RoomUser");
                var usersQueue = await conn.QueryAsync<RoomQueueDTO>("SELECT * FROM Query_RoomQueue");

                foreach(var room in rooms){
                    room.Users = roomUsers.Where(ru => ru.RoomID == room.RoomID);
                    room.UsersQueue = usersQueue.Where(uq => uq.RoomID == room.RoomID).Select(uq => uq.UserID);
                }

                if(filter == "mine")
                    return rooms.Where(r => r.Users.Any(x => x.UserID == userID));
                else
                    return rooms;
            }
        }
    }
}