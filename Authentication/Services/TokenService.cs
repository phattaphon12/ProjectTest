using Authentication.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Authentication.Services
{
    public class TokenService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly TokenSettings _tokenSettings;

        public TokenService(JwtSettings jwtSettings, TokenSettings tokenSettings)
        {
            _jwtSettings = jwtSettings;
            _tokenSettings = tokenSettings;
        }

        public string GenerateAccessToken(int userId, string username, List<int> roleId)
        {
            // Set claims
            var claims = new List<Claim>
            {
            // Set claims
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim(ClaimTypes.Name, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            foreach (var role in roleId)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.ToString()));
            }

            // Set security key from secret key
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Generate token descriptor
            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_tokenSettings.AccessTokenExpirationMinutes), // from AppSettings.json
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public DateTime GetRefreshTokenExpiry()
        {
            return DateTime.UtcNow.AddDays(_tokenSettings.RefreshTokenExpirationDays); // from AppSettings.json
        }
    }
}
