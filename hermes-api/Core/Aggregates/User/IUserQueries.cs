using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hermes.Core
{
    public interface IUserQueries
    {
        Task<UserDTO> Get(string userID);
        Task<UserDetailsDTO> GetDetails(string userID);
        Task<IEnumerable<UserDTO>> List();
        Task<IEnumerable<UserRankingDTO>> GetRanking();
        Task<IEnumerable<UserPostDTO>> GetUserPosts(string userID);
        Task<UserRankingDTO> GetUserRanking(string userID);
    }
}