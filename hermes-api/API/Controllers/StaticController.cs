using System.Collections.Generic;
using System.Threading.Tasks;
using Hermes.Core;
using Microsoft.AspNetCore.Mvc;

namespace Hermes.API
{
    [Route("api/static")]
    [ApiController]
    public class StaticController : ControllerBase
    {
        readonly ILanguageQueries _queries;

        public StaticController(ILanguageQueries queries)
        {
            _queries = queries;
        }

        [HttpGet("getLanguages")]
        public async Task<ActionResult<IEnumerable<LanguageDTO>>> GetLanguages()
            => Ok(await _queries.List());
    }
}