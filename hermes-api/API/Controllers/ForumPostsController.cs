using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hermes.Core;
using Hermes.Shell;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hermes.API.Controllers
{
    [Authorize]
    [Route("api/forumPost")]
    [ApiController]
    public class ForumPostController : DefaultController
    {
        private readonly DomainInterpreter _interpreter;
        private readonly IForumPostQueries _queries;

        public ForumPostController(DomainInterpreter interpreter, IForumPostQueries queries)
        {
            _interpreter = interpreter;
            _queries = queries;
        }

        [HttpGet]
        public async Task<IEnumerable<ForumPostDTO>> Query()
            => await _queries.Query();

        [HttpGet("{forumPostID}")]
        public async Task<ForumPostDTO> Get(Guid forumPostID)
            => await _queries.Get(forumPostID);

        [HttpGet("{forumPostID}/comments")]
        public async Task<IEnumerable<ForumPostCommentDTO>> GetComments(Guid forumPostID)
            => await _queries.GetComments(forumPostID);

        [HttpPost("create")]
        public ActionResult Create([FromBody]ForumPostCreateCommand command)
        {
            var forumPostID = ForumPostCommands.Execute(_interpreter, command, GetUserID());
            return Ok(forumPostID);
        }

        [HttpPost("edit")]
        public ActionResult Edit([FromBody]ForumPostEditCommand command)
        {
            ForumPostCommands.Execute(_interpreter, command, GetUserID());
            return Ok();
        }

        [HttpPost("delete")]
        public ActionResult Delete([FromBody]ForumPostDeleteCommand command)
        {
            ForumPostCommands.Execute(_interpreter, command, GetUserID());
            return Ok();
        }

        [HttpPost("comment")]
        public ActionResult Comment([FromBody]ForumPostCommentCommand command)
        {
            var forumPostCommentID = ForumPostCommands.Execute(_interpreter, command, GetUserID());
            return Ok(forumPostCommentID);
        }

        [HttpPost("deleteComment")]
        public ActionResult DeleteComment([FromBody]ForumPostDeleteCommentCommand command)
        {
            ForumPostCommands.Execute(_interpreter, command, GetUserID());
            return Ok();
        }
    }
}