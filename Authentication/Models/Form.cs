using System.ComponentModel.DataAnnotations;

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
}
