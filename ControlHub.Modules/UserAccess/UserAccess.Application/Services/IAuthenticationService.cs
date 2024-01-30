using UserAccess.Domain.Models;

namespace UserAccess.Application.Services
{
    public interface IAuthenticationService
    {
        string GenerateSalt();
        string EncryptPassword(string password, string salt);
        string GenerateJWT(User user);
        string GenerateRefreshToken(string username);
    }
}
