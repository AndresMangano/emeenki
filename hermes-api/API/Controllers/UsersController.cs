using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hermes.Core;
using Hermes.Shell;
using Hermes.Shell.Write;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hermes.API
{
    [Authorize]
    [Route("api/user")]
    [ApiController]
    public class UsersController : DefaultController
    {
        readonly DomainInterpreter _interpreter;
        readonly IUserQueries _queries;

        public UsersController(DomainInterpreter interpreter, IUserQueries queries)
        {
            _interpreter = interpreter;
            _queries = queries;
        }

        [HttpGet("{userID}")]
        public async Task<ActionResult<UserDTO>> Get(string userID)
            => await _queries.Get(userID);
        
        [HttpGet("{userID}/details")]
        public async Task<ActionResult<UserDetailsDTO>> GetDetails(string userID)
            => await _queries.GetDetails(userID);
        
        [HttpGet("query")]
        public async Task<ActionResult<IEnumerable<UserDTO>>> Query()
            => Ok(await _queries.List());

        [HttpGet("ranking")]
        public async Task<ActionResult<IEnumerable<UserRankingDTO>>> GetRanking()
            => Ok(await _queries.GetRanking());

        [HttpGet("{userID}/ranking")]
        public async Task<ActionResult<UserRankingDTO>> GetUserRanking(string userID)
            => Ok(await _queries.GetUserRanking(userID));

        [HttpGet("{userID}/posts")]
        public async Task<ActionResult<IEnumerable<UserPostDTO>>> GetPosts(string userID)
            => Ok(await _queries.GetUserPosts(userID));

        [HttpPost("setRights")]
        public ActionResult SetRights([FromBody]UserSetRightsCommand command)
        {
            UserCommands.Execute(_interpreter, command, GetUserID());
            return Ok();
        }

        [HttpPost("ban")]
        public ActionResult Ban([FromBody]UserBanCommand command)
        {
            UserCommands.Execute(_interpreter, command, GetUserID());
            return Ok();
        }

        [HttpPost("unban")]
        public ActionResult Unban([FromBody]UserUnbanCommand command)
        {
            UserCommands.Execute(_interpreter, command, GetUserID());
            return Ok();
        }

        [HttpPost("delete")]
        public ActionResult Delete([FromBody]UserDeleteCommand command)
        {
            UserCommands.Execute(_interpreter, command, GetUserID());
            return Ok();
        }

        [HttpPost("changeProfilePhoto")]
        public ActionResult ChangeProfilePhoto([FromBody]UserChangeProfilePhotoCommand command)
        {
            UserCommands.Execute(_interpreter, command, GetUserID());
            return Ok();
        }
        
        [HttpPost("addPost")]
        public ActionResult AddPost([FromBody]UserAddPostCommand command)
        {
            UserCommands.Execute(_interpreter, command, GetUserID());
            return Ok();
        }

        [HttpPost("deletePost")]
        public ActionResult DeletePost([FromBody]UserDeletePostCommand command)
        {
            UserCommands.Execute(_interpreter, command, GetUserID());
            return Ok();
        }

        [HttpPost("changeLanguage")]
        public ActionResult ChangeLanguage([FromBody]UserChangeLanguageCommand command)
        {
            UserCommands.Execute(_interpreter, command, GetUserID());
            return Ok();
        }

        [HttpPost("updateDescription")]
        public ActionResult UpdateDescription([FromBody]UserChangeDescriptionCommand command)
        {
            UserCommands.Execute(_interpreter, command, GetUserID());
            return Ok();
        }

        [HttpPost("updateCountry")]
        public ActionResult UpdateCountry([FromBody]UserChangeCountryCommand command)
        {
            UserCommands.Execute(_interpreter, command, GetUserID());
            return Ok();
        }
        
        [HttpPost("changePassword")]
        public ActionResult ChangePasword([FromBody]UserChangePasswordCommand command)
        {
            UserCommands.Execute(_interpreter, command, GetUserID());
            return Ok();
        }
    }
}
