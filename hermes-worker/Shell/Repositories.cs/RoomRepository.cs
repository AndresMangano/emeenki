using Dapper;
using Hermes.Worker.Core.Repositories;
using Hermes.Worker.Core.Repositories.Helpers;

namespace Hermes.Worker.Shell
{
    public partial class DBInterpreter : IRoomRepository
    {
        public void InsertRoom(string roomID, string languageID1, string languageID2, bool closed, bool restricted, int usersLimit)
        {
            _connection.Execute(@"
                INSERT INTO Query_Room(RoomID, LanguageID1, LanguageID2, Closed, Restricted, UsersLimit)
                VALUES(@roomID, @languageID1, @languageID2, @closed, @restricted, @usersLimit)
                    ON DUPLICATE KEY UPDATE RoomID = @roomID",
                new {
                    roomID,
                    languageID1,
                    languageID2,
                    closed,
                    restricted,
                    usersLimit
                },
                transaction: _transaction
            );
        }

        public void UpdateRoom(string roomID, DbUpdate<int> usersLimit = null, DbUpdate<bool> closed = null, DbUpdate<bool> restricted = null)
        {
            if (usersLimit != null || closed != null || restricted != null) {
                _connection.Execute(@"
                    UPDATE Query_Room
                    SET Closed = CASE @setClosed WHEN 1 THEN @closed ELSE Closed END,
                        Restricted = CASE @setRestricted WHEN 1 THEN @restricted ELSE Restricted END,
                        UsersLimit = CASE @setUsersLimit WHEN 1 THEN @usersLimit ELSE UsersLimit END
                    WHERE RoomID = @roomID",
                    new {
                        roomID,
                        setClosed = closed != null,
                        closed = closed?.Value,
                        setRestricted = restricted != null,
                        restricted = restricted?.Value,
                        setUsersLimit = usersLimit != null,
                        usersLimit = usersLimit?.Value
                    },
                    transaction: _transaction
                );
            }
        }
    }
}