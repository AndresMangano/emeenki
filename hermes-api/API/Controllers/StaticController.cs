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
        readonly ILanguageQueries _languageQueries;
        readonly ITopicQueries _topicQueries;

        public StaticController(ILanguageQueries languageQueries, ITopicQueries topicQueries)
        {
            _languageQueries = languageQueries;
            _topicQueries = topicQueries;
        }

        [HttpGet("getLanguages")] // TODO: Change to /languages
        public async Task<ActionResult<IEnumerable<LanguageDTO>>> GetLanguages() =>
            Ok(await _languageQueries.List());

        [HttpGet("topics")]
        public async Task<ActionResult<IEnumerable<TopicDTO>>> GetTopics() =>
            Ok(await _topicQueries.List());
    }
}