using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Hermes.Core;
using Hermes.Shell;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Hermes.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokensController : ControllerBase
    {
        readonly DomainInterpreter _interpreter;
        readonly string _secret;

        public TokensController(DomainInterpreter interpreter, IConfiguration configuration)
        {
            _interpreter = interpreter;
            _secret = configuration["Authentication:Secret"];
        }

        [HttpPost("loginWithPassword")]
        public UserLogInResult LoginWithPassword([FromBody]UserLogInWithPasswordCommand command)
        {
            var result = UserCommands.Execute(_interpreter, command);
            result.Token = GenerateJWTToken(command.UserID.ToLower());
            return result;
        }

        [Authorize(AuthenticationSchemes = "GoogleToken")]
        [HttpPost("loginWithGoogle")]
        public UserLogInResult LoginWithGoogle()
        {
            var result = UserCommands.Execute(_interpreter, new UserLogInWithGoogleCommand {
                GoogleEmail = GetGoogleEmail()
            });
            result.Token = GenerateJWTToken(result.UserID);
            return result;
        }

        [HttpPost("registerWithPassword")]
        public ActionResult<UserRegisterResult> RegisterWithPassword([FromBody]UserRegisterWithPasswordCommand command)
        {
            var result = UserCommands.Execute(_interpreter, command);
            return Ok(result);
        }

        [Authorize(AuthenticationSchemes = "GoogleToken")]
        [HttpPost("registerWithGoogle")]
        public ActionResult<UserRegisterResult> RegisterWithGoogle([FromBody]UserRegisterWithGoogleCommand command)
        {
            ValidateGoogleEmail(command.GoogleEmail);
            var result = UserCommands.Execute(_interpreter, command);
            return Ok(result);
        }

        private void ValidateGoogleEmail(string email)
        {
            if (email != HttpContext.User.Claims.Single(c => c.Type == ClaimTypes.Email).Value)
                throw new Exception("Invalid Google account");
        }
        private string GetGoogleEmail()
        {
            return HttpContext.User.Claims.Single(c => c.Type == ClaimTypes.Email).Value;
        }

        private string GenerateJWTToken(string userID)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secret);
            var tokenDescriptor = new SecurityTokenDescriptor {
                Subject = new ClaimsIdentity(new    [] {
                    new Claim("id", userID)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}