using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hermes.Core
{
    public interface ILanguageQueries
    {
        Task<IEnumerable<LanguageDTO>> List();
    }
}