using Microsoft.EntityFrameworkCore;
using UserAccess.Infrastructure.Dtos;

namespace UserAccess.Infrastructure.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly UserAccessDBContext _context;
        public RefreshTokenRepository(UserAccessDBContext context)
        {
            _context = context;
        }

        public async Task<RefreshTokenDto?> GetByUserName(String username) 
            => await _context.RefreshTokens.FirstOrDefaultAsync(obj => obj.UserName == username);

        public void Add(RefreshTokenDto refreshTokenDto) 
            => _context.RefreshTokens.AddAsync(refreshTokenDto);

    }
}
