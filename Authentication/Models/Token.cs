namespace Authentication.Models
{
    public class Token
    {
    }
    public class JwtSettings
    {
        public string Secret { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
    }

    public class TokenSettings
    {
        public int AccessTokenExpirationMinutes { get; set; }
        public int RefreshTokenExpirationDays { get; set; }
    }

    public class LoginRes
    {
        public string accessToken { get; set; } = string.Empty;
        public string refreshToken { get; set; } = string.Empty;
    }

    public class RefreshTokenDb
    {
        public int user_id { get; set; }
        public string refresh_token { get; set; } = string.Empty;
        public DateTime? expire_date { get; set; }
        public DateTime? revoke_date { get; set; }
        public DateTime? login_date { get; set; }
    }
}
