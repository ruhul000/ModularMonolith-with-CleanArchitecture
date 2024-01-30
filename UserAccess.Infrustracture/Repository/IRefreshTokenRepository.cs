using UserAccess.Infrastructure.Dtos;

namespace UserAccess.Infrastructure.Repository
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshTokenDto?> Get(String username, string refreshToken);
        Task<bool> Save(RefreshTokenDto refreshTokenDto);
    }
}
