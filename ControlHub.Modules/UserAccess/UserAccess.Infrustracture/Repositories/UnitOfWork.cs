using UserAccess.Infrastructure.Repositories;

namespace UserAccess.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly UserAccessDBContext _context;
        public UnitOfWork(UserAccessDBContext context)
        {
            _context = context;
        }
        public IUserRepository UserRepository => new UserRepository(_context);

        public IRefreshTokenRepository RefreshTokenRepository => new RefreshTokenRepository(_context);

        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync(true) > 0;
        }
    }
}
