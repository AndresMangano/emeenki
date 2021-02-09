using Dapper;
using Hermes.Worker.Core.Repositories;

namespace Hermes.Worker.Shell
{
    public partial class DBInterpreter : IRoomUserRepository
    {
        public void DeleteRoomUser(string roomID, string userID)
        {
            _connection.Execute(@"
                DELETE FROM Query_RoomUser
                WHERE   RoomID = @roomID AND
                        UserID = @userID",
                new {
                    roomID,
                    userID
                },
                transaction: _transaction
            );
        }

        public void InsertRoomUser(string roomID, string userID, string permission)
        {
            _connection.Execute(@"
                INSERT INTO Query_RoomUser(RoomID, UserID, Permission)
                VALUES(@roomID, @userID, @permission)
                    ON DUPLICATE KEY UPDATE RoomID = @roomID",
                new {
                    roomID,
                    userID,
                    permission
                },
                transaction: _transaction
            );
        }
    }
}