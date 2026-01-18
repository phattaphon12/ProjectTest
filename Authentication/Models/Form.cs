using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;

namespace Authentication.Models
{
    public class Form
    {
    }

    public class LoginReq
    {
        
        public string username { get; set; } = string.Empty;
        
        public string password { get; set; } = string.Empty;
    }

    public class RegisterReq
    {
        
        public string username { get; set; } = string.Empty;
        
        public string password { get; set; } = string.Empty;
        
        public string confirmPassword { get; set; } = string.Empty;
    }

    public class ChangePasswordReq
    {
        public int userId { get; set; }
        
        public string oldPassword { get; set; } = string.Empty;
        
        public string newPassword { get; set; } = string.Empty;
        
        public string confirmPassword { get; set; } = string.Empty;
    }

    public class LogoutReq
    {
        public string UserId { get; set; } = string.Empty;
    }

    public class RefreshTokenReq
    {
        public string refreshToken { get; set; } = string.Empty;
    }

    public class LoginResult
    {
        public LoginRes Data { get; set; } = new LoginRes();
        public string ErrorMessage { get; set; } = string.Empty;
        public bool IsSuccess => string.IsNullOrEmpty(ErrorMessage);
    }
}
