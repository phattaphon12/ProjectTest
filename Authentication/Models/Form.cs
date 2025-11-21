using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;

namespace Authentication.Models
{
    public class Form
    {
    }

    public class LoginReq
    {
        [Required]
        public string username { get; set; } = string.Empty;
        [Required]
        public string password { get; set; } = string.Empty;
    }

    public class RegisterReq
    {
        [Required]
        public string username { get; set; } = string.Empty;
        [Required]
        public string password { get; set; } = string.Empty;
        [Required]
        public string confirmPassword { get; set; } = string.Empty;
    }

    public class ChangePasswordReq
    {
        public string username { get; set; } = string.Empty;
        [Required]
        public string oldPassword { get; set; } = string.Empty;
        [Required]
        public string newPassword { get; set; } = string.Empty;
        [Required]
        public string confirmPassword { get; set; } = string.Empty;
    }
}
