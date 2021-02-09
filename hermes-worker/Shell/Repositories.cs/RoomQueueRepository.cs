using Dapper;
using Hermes.Worker.Core.Repositories;

namespace Hermes.Worker.Shell
{
    public partial class DBInterpreter : IRoomQueueRepository
    {
        public void DeleteRoomQueue(string roomID, string userID)
        {
            _connection.Execute(@"
                DELETE FROM Query_RoomQueue
                WHERE   RoomID = @roomID AND
                        UserID = @userID",
                new {
                    roomID,
                    userID
                },
                transaction: _transaction
            );
        }

        public void InsertRoomQueue(string roomID, string userID)
        {
            _connection.Execute(@"
                INSERT INTO Query_RoomQueue(RoomID, UserID)
                VALUES(@roomID, @userID)
                    ON DUPLICATE KEY UPDATE RoomID = @roomID",
                new {
                    roomID,
                    userID
                },
                transaction: _transaction
            );
        }
    }
}