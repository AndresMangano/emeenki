using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hermes.Core
{
    public interface ITopicQueries
    {
        Task<IEnumerable<TopicDTO>> List();
    }
}