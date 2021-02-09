using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hermes.Core
{
    public interface IArticleQueries
    {
        Task<ArticleDTO> Get(Guid articleID);
        Task<IEnumerable<ArticlesDTO>> Query(string roomID, bool archived);
        Task<IEnumerable<ActivityDTO>> GetActivityFeed();
        Task<IEnumerable<ActivityDTO>> GetUserActivity(string userID);
    }
}