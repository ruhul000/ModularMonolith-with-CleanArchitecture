using UserAccess.Infrastructure.Dtos;

namespace UserAccess.Infrastructure.Repository
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly UserAccessDBContext _context;
        public RefreshTokenRepository(UserAccessDBContext context)
        {
            _context = context;
        }

        public async Task<RefreshTokenDto?> Get(String username, string refreshToken)
        {
            var refreshTokenDto = _context.RefreshTokens.FirstOrDefault(obj => obj.UserName == username && obj.RefreshToken == refreshToken);
            return refreshTokenDto;
        }

        public async Task<bool> Save(RefreshTokenDto refreshTokenDto)
        {
            var tokenDto = _context.RefreshTokens.FirstOrDefault(obj => obj.UserName == refreshTokenDto.UserName);
            try
            {
                if (tokenDto != null)
                {
                    tokenDto.RefreshToken = refreshTokenDto.RefreshToken;
                }
                else
                {
                    tokenDto = refreshTokenDto;
                    _context.RefreshTokens.Add(refreshTokenDto);
                }
            }
            catch (Exception ex)
            {
                throw;
            }


            return (await _context.SaveChangesAsync() > 0);
        }
    }
}
