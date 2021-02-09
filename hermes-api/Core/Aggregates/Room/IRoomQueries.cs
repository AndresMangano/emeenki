using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hermes.Core
{
    public interface IRoomQueries
    {
        Task<RoomDTO> Get(string roomID);
        Task<IEnumerable<RoomDTO>> Query(string filter, string userID, string languageID1, string languageID2);
        Task<IEnumerable<RoomUsersDTO>> GetUsers(string roomID);
        Task<IEnumerable<RoomUsersDTO>> GetPendingUsers(string roomID);
    }
}