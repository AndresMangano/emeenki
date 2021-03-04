using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hermes.Core;
using Hermes.Shell;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hermes.API
{
    [Authorize]
    [Route("api/articleTemplate")]
    [ApiController]
    public class ArticleTemplatesController : DefaultController
    {
        readonly DomainInterpreter _interpreter;
        readonly IArticleTemplateQueries _queries;

        public ArticleTemplatesController(DomainInterpreter interpreter, IArticleTemplateQueries queries)
        {
            _interpreter = interpreter;
            _queries = queries;
        }

        [HttpGet("get/{articleTemplateID}")]
        public async Task<ActionResult<ArticleTemplateDTO>> Get(Guid articleTemplateID)
            => await _queries.Get(articleTemplateID);

        [HttpGet("query/{archived}")]
        public async Task<ActionResult<IEnumerable<ArticleTemplatesDTO>>> Query(bool archived, [FromQuery]string languageID, [FromQuery]string topicID)
        {
            var result = await _queries.Query(languageID, topicID, archived);
            return Ok(result);
        }
            
        [HttpPost("upload")]
        public ActionResult<ArticleTemplateUploadResult> Upload([FromBody]ArticleTemplateUploadCommand command)
        {
            var result = ArticleTemplateCommands.Execute(_interpreter, command, GetUserID());
            return Ok(result);
        }
        [HttpPost("delete")]
        public ActionResult Delete([FromBody]ArticleTemplateDeleteCommand command)
        {
            ArticleTemplateCommands.Execute(_interpreter, command, GetUserID());
            return Ok();
        }
    }
}
