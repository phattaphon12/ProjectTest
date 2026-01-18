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
        private readonly TokenRepository _tokenRepo;
        private readonly TokenService _tokenService;

        public AuthService(
            AuthRepository authRepo,
            TokenRepository tokenRepo,
            TokenService tokenService)
        {
            _authRepo = authRepo;
            _tokenRepo = tokenRepo;
            _tokenService = tokenService;
        }

        public async Task<LoginResult> LoginAsync(LoginReq req)
        {
            if (string.IsNullOrEmpty(req.username))
            {
                return new LoginResult { ErrorMessage = "Username is required." };
            }

            if (string.IsNullOrEmpty(req.password))
            {
                return new LoginResult { ErrorMessage = "Password is required." };
            }

            User userInDB = await _authRepo.GetUserAsync(req.username);
            if (userInDB == null || string.IsNullOrEmpty(userInDB.username))
            {
                return new LoginResult { ErrorMessage = "No data." };
            }

            bool checkPassword = BCrypt.Net.BCrypt.Verify(req.password, userInDB.password);
            if (!checkPassword)
            {
                return new LoginResult { ErrorMessage = "Invalid password." };
            }

            List<int> roleIds = await _authRepo.GetRoleIdByUserIdAsync(userInDB.user_id);
            if (roleIds == null || !roleIds.Any())
            {
                return new LoginResult { ErrorMessage = "No role assigned." };
            }

            var authResult = _tokenService.GenerateAccessToken(userInDB.user_id, userInDB.username, roleIds);
            var refreshToken = _tokenService.GenerateRefreshToken();

            var saveRefreshToken = await _tokenRepo.SaveRefreshTokenAsync(userInDB.user_id, refreshToken);
            if (saveRefreshToken <= 0)
            {
                return new LoginResult { ErrorMessage = "Login failed." };
            }

            var result = new LoginRes
            {
                accessToken = authResult,
                refreshToken = refreshToken
            };

            return new LoginResult { Data = result };

            /*if (string.IsNullOrEmpty(req.username))
            {
                return ApiResult.Fail("Username is required.");
            }

            if (string.IsNullOrEmpty(req.password))
            {
                return ApiResult.Fail("Password is required.");
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

            List<int> roleIds = await _authRepo.GetRoleIdByUserIdAsync(userInDB.user_id);
            if (roleIds == null || !roleIds.Any())
            {
                return ApiResult.Fail("No role assigned.");
            }

            var authResult = _tokenService.GenerateAccessToken(userInDB.user_id, userInDB.username, roleIds);
            var refreshToken = _tokenService.GenerateRefreshToken();

            var saveRefreshToken = await _tokenRepo.SaveRefreshTokenAsync(userInDB.user_id, refreshToken);
            if (saveRefreshToken <= 0)
            {
                return ApiResult.Fail("Login failed.");
            }

            var result = new LoginRes
            {
                accessToken = authResult,
                refreshToken = refreshToken
            };

            return ApiResult.Success(result);*/
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

            var rowsAffectedRegister = await _authRepo.RegisterUserAsync(req);
            if (rowsAffectedRegister <= 0)
            {
                return ApiResult.Fail("Register failed.");
            }

            if (checkUsername == null || string.IsNullOrEmpty(checkUsername.username))
            {
                checkUsername = await _authRepo.GetUserAsync(req.username);
            }
            var rowsAffectedUserRole = await _authRepo.InsertRoleUserAsync(checkUsername.user_id);
            if (rowsAffectedUserRole <= 0)
            {
                return ApiResult.Fail("Assign role failed.");
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

            User userInDB = await _authRepo.GetUserByIdAsync(req.userId);
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

        public async Task<IActionResult> LogoutAsync(int userId)
        {
            if (userId <= 0)
            {
                return ApiResult.Fail("Invalid user ID.");
            }

            var rowsAffected = await _tokenRepo.RevokeRefreshTokenAsync(userId);
            if (rowsAffected <= 0)
            {
                return ApiResult.Fail("Logout failed.");
            }

            return ApiResult.Success("Logout success.");
        }

        public async Task<LoginResult> RefreshAccessTokenAsync(string refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken))
            {
                return new LoginResult { ErrorMessage = "Refresh token is required." };
            }

            var authResult = await _tokenRepo.GetDataByRefreshTokenAsync(refreshToken);
            if (authResult == null)
            {
                return new LoginResult { ErrorMessage = "Invalid refresh token." };
            }

            if (authResult.expire_date <= DateTime.Now || authResult.revoke_date != null)
            {
                return new LoginResult { ErrorMessage = "Refresh token expired." };
            }

            User userInDB = await _authRepo.GetUserByIdAsync(authResult.user_id);
            if (string.IsNullOrEmpty(userInDB.username))
            {
                return new LoginResult { ErrorMessage = "No data." };
            }

            List<int> roleIds = await _authRepo.GetRoleIdByUserIdAsync(userInDB.user_id);
            if (roleIds == null || !roleIds.Any())
            {
                return new LoginResult { ErrorMessage = "No role assigned." };
            }

            await _tokenRepo.RevokeRefreshTokenByRfAsync(refreshToken);

            var newAccessToken = _tokenService.GenerateAccessToken(userInDB.user_id, userInDB.username, roleIds);
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            var saveRefreshToken = await _tokenRepo.SaveRefreshTokenAsync(userInDB.user_id, newRefreshToken);
            if (saveRefreshToken <= 0)
            {
                return new LoginResult { ErrorMessage = "Refresh token failed." };
            }

            var result = new LoginRes
            {
                accessToken = newAccessToken,
                refreshToken = newRefreshToken
            };

            return new LoginResult { Data = result };

            /*if (string.IsNullOrEmpty(refreshToken))
            {
                return ApiResult.Fail("Refresh token is required.");
            }

            var authResult = await _tokenRepo.GetDataByRefreshTokenAsync(refreshToken);
            if (authResult == null)
            {
                return ApiResult.Fail("Invalid refresh token.");
            }

            if (authResult.expire_date <= DateTime.Now || authResult.revoke_date != null)
            {
                return ApiResult.Fail("Refresh token expired.");
            }

            User userInDB = await _authRepo.GetUserByIdAsync(authResult.user_id);
            if (string.IsNullOrEmpty(userInDB.username))
            {
                return ApiResult.Fail("No data.");
            }

            List<int> roleIds = await _authRepo.GetRoleIdByUserIdAsync(userInDB.user_id);
            if (roleIds == null || !roleIds.Any())
            {
                return ApiResult.Fail("No role assigned.");
            }

            var newAccessToken = _tokenService.GenerateAccessToken(userInDB.user_id, userInDB.username, roleIds);
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            var saveRefreshToken = await _tokenRepo.SaveRefreshTokenAsync(userInDB.user_id, newRefreshToken);
            if (saveRefreshToken <= 0)
            {
                return ApiResult.Fail("Login failed.");
            }

            var result = new LoginRes
            {
                accessToken = newAccessToken
            };

            return ApiResult.Success(result, newRefreshToken);*/
        }
    }
}
