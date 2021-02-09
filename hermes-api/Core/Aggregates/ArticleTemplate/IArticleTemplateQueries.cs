using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hermes.Core
{
    public interface IArticleTemplateQueries
    {
        Task<ArticleTemplateDTO> Get(Guid articleTemplateID);
        Task<IEnumerable<ArticleTemplatesDTO>> Query(string languageID, bool archived);
    }
}