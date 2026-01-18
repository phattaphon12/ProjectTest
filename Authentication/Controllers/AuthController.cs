using Authentication.Helpers;
using Authentication.Models;
using Authentication.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

            if (result.IsSuccess)
            {
                var refreshToken = result.Data.refreshToken;

                // Set the refresh token as an HttpOnly cookie
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true, // Set to true in production
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTimeOffset.UtcNow.AddDays(1) // Set expiration as needed
                };

                Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);

                var responseData = new
                {
                    accessToken = result.Data.accessToken
                };

                return ApiResult.Success(responseData);
            } else
            {
                return ApiResult.Fail(result.ErrorMessage);
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterReq req)
        {
            var result = await _AuthService.RegisterAsync(req);
            return result;
        }

        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordReq req)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                // กรณีที่ไม่สามารถหา UserId ใน Token หรือแปลงเป็น int ไม่ได้
                return Unauthorized("Invalid or missing user identifier in token.");
            }

            req.userId = userId;

            var result = await _AuthService.ChangePasswordAsync(req);
            Response.Cookies.Delete("refreshToken");
            return result;
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                // กรณีที่ไม่สามารถหา UserId ใน Token หรือแปลงเป็น int ไม่ได้
                return Unauthorized("Invalid or missing user identifier in token.");
            }

            var result = await _AuthService.LogoutAsync(userId);
            Response.Cookies.Delete("refreshToken");
            return result;
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {
            var currentRefreshToken = Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(currentRefreshToken))
            {
                return ApiResult.Fail("Refresh token is missing.");
            }

            var result = await _AuthService.RefreshAccessTokenAsync(currentRefreshToken);

            Response.Cookies.Delete("refreshToken");

            if (result.IsSuccess)
            {
                var newRefreshToken = result.Data.refreshToken;
                // Update the refresh token cookie
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true, // Set to true in production
                    //SameSite = SameSiteMode.Strict,
                    SameSite = SameSiteMode.None,
                    Expires = DateTimeOffset.UtcNow.AddDays(1) // Set expiration as needed
                };
                Response.Cookies.Append("refreshToken", newRefreshToken, cookieOptions);
                var responseData = new
                {
                    accessToken = result.Data.accessToken
                };
                return ApiResult.Success(responseData);
            }
            else
            {
                return ApiResult.Fail(result.ErrorMessage);
            }
        }
    }
}
