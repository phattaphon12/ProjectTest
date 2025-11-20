using Authentication.Helpers;
using Authentication.Models;
using Authentication.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Authentication.Services
{
    public class AuthService
    {
        private readonly AuthRepository _authRepo;

        public AuthService(AuthRepository authRepo)
        {
            _authRepo = authRepo;
        }

        public async Task<IActionResult> LoginAsync(LoginReq req)
        {
            if (req == null)
            {
                return ApiResult.Fail("Username and password are required.");
            }

            User userInDB = await _authRepo.GetUserAsync(req.username);
            if (userInDB == null || string.IsNullOrEmpty(userInDB.username))
            {
                return ApiResult.Fail("No data.");
            }

            if (userInDB.password != req.password)
            {
                return ApiResult.Fail("Invalid password.");
            }

            return ApiResult.Success("Login Success");
        }
    }
}
