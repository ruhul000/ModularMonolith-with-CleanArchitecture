using System.Security.Claims;
using UserAccess.Domain.Models;

namespace UserAccess.Application.Services
{
    public interface IAuthenticationService
    {
        string GenerateSalt();
        string EncryptPassword(string password, string salt);
        string GenerateJWT(User user);
        string GenerateRefreshToken();
        string GenerateUniqueToken();
        ClaimsPrincipal? GetPrincipalFromTokenValidation(string accessToken);
    }
}
