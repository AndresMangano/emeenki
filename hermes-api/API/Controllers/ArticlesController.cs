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
    [Route("api/article")]
    [ApiController]
    public class ArticlesController : DefaultController
    {
        private readonly DomainInterpreter _interpreter;
        private readonly IArticleQueries _queries;

        public ArticlesController(DomainInterpreter interpreter, IArticleQueries queries)
        {
            _interpreter = interpreter;
            _queries = queries;
        }

        [HttpGet("get/{articleID}")]
        public async Task<ActionResult<ArticleDTO>> Get(Guid articleID)
            => await _queries.Get(articleID);

        [HttpGet("query/{roomID}/{archived}")]
        public async Task<ActionResult<IEnumerable<ArticlesDTO>>> Query(string roomID, bool archived)
            => Ok(await _queries.Query(roomID, archived));

        [HttpGet("activity")]
        public async Task<ActionResult<IEnumerable<ActivityDTO>>> GetActivityFeed(string userID)
        {
            if (userID == null) {
                return Ok(await _queries.GetActivityFeed());
            } else {
                return Ok(await _queries.GetUserActivity(userID));
            }
        }

        [HttpPost("takeTemplate")]
        public ActionResult<ArticleTakeTemplateResult> TakeTemplate([FromBody]ArticleTakeTemplateCommand command)
        {
            var result = ArticleCommands.Execute(_interpreter, command, GetUserID());
            return Ok(result);
        }

        [HttpPost("translate")]
        public ActionResult Translate([FromBody]ArticleTranslateCommand command)
        {
            ArticleCommands.Execute(_interpreter, command, GetUserID());
            return Ok();
        }

        [HttpPost("vote")]
        public ActionResult Vote([FromBody]ArticleVoteCommand command)
        {
            ArticleCommands.Execute(_interpreter, command, GetUserID());
            return Ok();
        }

        [HttpPost("comment")]
        public ActionResult Comment([FromBody]ArticleCommentCommand command)
        {
            ArticleCommands.Execute(_interpreter, command, GetUserID());
            return Ok();
        }

        [HttpPost("commentMain")]
        public ActionResult CommentMain([FromBody]ArticleCommentMainCommand command)
        {
            ArticleCommands.Execute(_interpreter, command, GetUserID());
            return Ok();
        }

        [HttpPost("deleteMainComment")]
        public ActionResult DeleteMainComment([FromBody]ArticleDeleteMainCommentCommand command)
        {
            ArticleCommands.Execute(_interpreter, command, GetUserID());
            return Ok();
        }

        [HttpPost("archive")]
        public ActionResult Archive([FromBody]ArticleArchiveCommand command)
        {
            ArticleCommands.Execute(_interpreter, command, GetUserID());
            return Ok();
        }
    }
}
