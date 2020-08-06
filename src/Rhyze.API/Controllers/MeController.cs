using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Rhyze.API.Controllers
{
    [ApiController]
    public class MeController : ControllerBase
    {
        [Route("/me")]
        public object Me()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var id = User.FindFirstValue("user_id");

            return new { id, email };
        }
    }
}
