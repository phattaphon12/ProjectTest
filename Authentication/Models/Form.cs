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
        public string username { get; set; } = string.Empty;
        
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
}
