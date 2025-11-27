using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Authentication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestRouteController : ControllerBase
    {
        [HttpGet("public")]
        public IActionResult PublicEndpoint()
        {
            return Ok(new { message = "This is a public endpoint." });
        }

        [HttpGet("private")]
        [Authorize]
        public IActionResult PrivateEndpoint()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var username = User.Identity?.Name;
            var roles = User.FindAll(ClaimTypes.Role).Select(r => r.Value).ToList();
            return Ok(new
            {
                message = "This is a private endpoint.",
                userId,
                username,
                roles
            }); 
        }
    }
}
