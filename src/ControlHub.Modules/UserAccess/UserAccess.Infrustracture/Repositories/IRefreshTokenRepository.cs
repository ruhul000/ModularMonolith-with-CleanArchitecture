using UserAccess.Infrastructure.Dtos;

namespace UserAccess.Infrastructure.Repositories
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshTokenDto?> GetByUserName(string username);
        void Add(RefreshTokenDto refreshTokenDto);
    }
}
