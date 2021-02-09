using Hermes.Worker.Core.Repositories.Helpers;

namespace Hermes.Worker.Core.Repositories
{
    public interface IRoomRepository
    {
        void InsertRoom(string roomID, string languageID1, string languageID2, bool closed, bool restricted, int usersLimit);
        void UpdateRoom(string roomID,
            DbUpdate<int> usersLimit = null,
            DbUpdate<bool> closed = null,
            DbUpdate<bool> restricted = null);
    }
}