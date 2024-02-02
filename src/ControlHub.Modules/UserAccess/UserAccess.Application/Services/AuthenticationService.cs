using System.Text;
using UserAccess.Domain.Models;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace UserAccess.Application.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private const int ITERATIONS = 2048;

        private readonly IConfiguration _configuration;
        public AuthenticationService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string GenerateSalt()
        {
            var hmac = new HMACSHA512();

            return Convert.ToBase64String(hmac.Key);
        }
        public string EncryptPassword(string password, string salt)
        {
            string hash = null;
            try
            {
                var saltBytes = Convert.FromBase64String(salt);

                using (var rfcbytes = new Rfc2898DeriveBytes(password, saltBytes, ITERATIONS))
                {
                    hash = Convert.ToBase64String(rfcbytes.GetBytes(256));
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return hash;
        }
        public string GenerateJWT(User user)
        {
            var jwtSettings = _configuration.GetSection("JWTSettings").Get<JWTSettings>();

            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, jwtSettings.Subject),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds().ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim("UserId", user.UserId.ToString()),
                new Claim("FullName", user.FullName),
                new Claim("UserName", user.UserName),
                new Claim("Password", user.Password),
                new Claim("Salt", user.Salt),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                jwtSettings.Issuer,
                jwtSettings.Audience,
                claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: signIn
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using (var randomNumberGenerator = RandomNumberGenerator.Create())
            {
                randomNumberGenerator.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public ClaimsPrincipal? GetPrincipalFromTokenValidation(string accessToken)
        {
            // Validate Token
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtSettings = _configuration.GetSection("JWTSettings").Get<JWTSettings>();

            return tokenHandler.ValidateToken(accessToken, new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
            }, out _);

        }
    }
}
