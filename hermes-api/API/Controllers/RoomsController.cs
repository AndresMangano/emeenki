using System.Collections.Generic;
using System.Threading.Tasks;
using Hermes.Core;
using Hermes.Shell;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hermes.API
{
    [Authorize]
    [Route("api/room")]
    [ApiController]
    public class RoomsController : DefaultController
    {
        readonly DomainInterpreter _interpreter;
        readonly IRoomQueries _queries;

        public RoomsController(DomainInterpreter interpreter, IRoomQueries queries)
        {
            _interpreter = interpreter;
            _queries = queries;
        }
        
        [HttpGet("get/{roomID}")]
        public async Task<ActionResult<RoomDTO>> Get(string roomID)
            => await _queries.Get(roomID);

        [HttpGet("get/{roomID}/users")]
        public async Task<ActionResult<IEnumerable<RoomUsersDTO>>> GetUsers(string roomID)
            => Ok(await _queries.GetUsers(roomID));

        [HttpGet("get/{roomID}/pendingusers")]
        public async Task<ActionResult<IEnumerable<RoomUsersDTO>>> GetPendingUsers(string roomID)
            => Ok(await _queries.GetPendingUsers(roomID));

        [HttpGet("query/{filter}/{userID}/{language1}/{language2}")]
        public async Task<ActionResult<IEnumerable<RoomDTO>>> Query(string filter, string userID, string language1, string language2)
            => Ok(await _queries.Query(filter, userID, language1, language2));

        [HttpPost("open")]
        public ActionResult<RoomOpenResult> Open([FromBody]RoomOpenCommand command)
        {
            var result = RoomCommands.Execute(_interpreter, command, GetUserID());
            return Ok(result);
        }

        [HttpPost("close")]
        public ActionResult Close([FromBody]RoomCloseCommand command)
        {
            RoomCommands.Execute(_interpreter, command, GetUserID());
            return Ok();
        }

        [HttpPost("join")]
        public ActionResult Join([FromBody]RoomJoinCommand command)
        {
            RoomCommands.Execute(_interpreter, command, GetUserID());
            return Ok();
        }

        [HttpPost("acceptUser")]
        public ActionResult AcceptUser([FromBody]RoomAcceptUserCommand command)
        {
            RoomCommands.Execute(_interpreter, command, GetUserID());
            return Ok();
        }

        [HttpPost("rejectUser")]
        public ActionResult RejectUser([FromBody]RoomRejectUserCommand command)
        {
            RoomCommands.Execute(_interpreter, command, GetUserID());
            return Ok();
        }

        [HttpPost("left")]
        public ActionResult Leave([FromBody]RoomLeaveCommand command)
        {
            RoomCommands.Execute(_interpreter, command, GetUserID());
            return Ok();
        }

        [HttpPost("expelUser")]
        public ActionResult ExpelUser([FromBody]RoomExpelUserCommand command)
        {
            RoomCommands.Execute(_interpreter, command, GetUserID());
            return Ok();
        }

        [HttpPost("changeUsersLimit")]
        public ActionResult ChangeUsersLimit([FromBody]RoomChangeUsersLimitCommand command)
        {
            RoomCommands.Execute(_interpreter, command, GetUserID());
            return Ok();
        }

        [HttpPost("restrict")]
        public ActionResult Restrict([FromBody]RoomRestrictCommand command)
        {
            RoomCommands.Execute(_interpreter, command, GetUserID());
            return Ok();
        }

        [HttpPost("unrestrict")]
        public ActionResult UnRestrict([FromBody]RoomUnrestrictCommand command)
        {
            RoomCommands.Execute(_interpreter, command, GetUserID());
            return Ok();
        }

        [HttpPost("inviteUser")]
        public ActionResult<RoomInviteUserResult> InviteUser([FromBody]RoomInviteUserCommand command)
        {
            var result = RoomCommands.Execute(_interpreter, command, GetUserID());
            return Ok(result);
        }
        [HttpPost("joinWithToken")]
        public ActionResult JoinWithToken([FromBody]RoomJoinWithTokenCommand command)
        {
            RoomCommands.Execute(_interpreter, command, GetUserID());
            return Ok();
        }
    }
}
