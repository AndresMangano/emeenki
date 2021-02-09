using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace Hermes.API
{
    public class DefaultController : ControllerBase
    {
        public string GetUserID()
        {
            return HttpContext.User.Claims.Single(c => c.Type == "id").Value;
        }
    }
}