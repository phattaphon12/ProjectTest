using Authentication.Helpers;
using Authentication.Models;
using Authentication.Repositories;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Crypto.Generators;

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

            bool checkPassword = BCrypt.Net.BCrypt.Verify(req.password, userInDB.password);
            if (!checkPassword)
            {
                return ApiResult.Fail("Invalid password.");
            }

            return ApiResult.Success("Login Success");
        }

        public async Task<IActionResult> RegisterAsync(RegisterReq req)
        {
            if (req == null)
            {
                return ApiResult.Fail("Form is required.");
            }

            if (req.password != req.confirmPassword)
            {
                return ApiResult.Fail("Password do not match.");
            }

            // Check username
            User checkUsername = await _authRepo.GetUserAsync(req.username);
            if (checkUsername != null && !string.IsNullOrEmpty(checkUsername.username))
            {
                return ApiResult.Fail("Username already exists.");
            }

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(req.password);

            req.password = hashedPassword;

            var rowsAffected = await _authRepo.RegisterUserAsync(req);
            if (rowsAffected <= 0)
            {
                return ApiResult.Fail("Register failed.");
            }

            return ApiResult.Success("Register Success");
        }

        public async Task<IActionResult> ChangePasswordAsync(ChangePasswordReq req)
        {
            if (req == null)
            {
                return ApiResult.Fail("Form is required.");
            }

            if (req.oldPassword == req.newPassword)
            {
                return ApiResult.Fail("Old password and new password same as.");
            }            

            User userInDB = await _authRepo.GetUserAsync(req.username);
            if (string.IsNullOrEmpty(userInDB.username))
            {
                return ApiResult.Fail("No data");
            }

            bool passwordInDb = BCrypt.Net.BCrypt.Verify(req.oldPassword, userInDB.password);
            if (!passwordInDb)
            {
                return ApiResult.Fail("Invalid old password.");
            }

            if (req.newPassword != req.confirmPassword)
            {
                return ApiResult.Fail("New password and confirm password do not match.");
            }

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(req.confirmPassword);

            var rowAffected = await _authRepo.ChangePasswordAsync(userInDB.username, hashedPassword);
            if (rowAffected <= 0)
            {
                return ApiResult.Fail("Change password failed.");
            }

            return ApiResult.Success("Change password success.");
        }
    }
}
