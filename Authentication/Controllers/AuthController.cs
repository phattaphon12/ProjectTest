using Authentication.Models;
using Authentication.Services;
using Microsoft.AspNetCore.Mvc;

namespace Authentication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _AuthService;

        public AuthController(AuthService authService)
        {
            _AuthService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginReq req)
        {
            var result = await _AuthService.LoginAsync(req);
            return Ok(result);
        }
    }
}
