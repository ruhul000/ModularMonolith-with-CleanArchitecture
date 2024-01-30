using Microsoft.EntityFrameworkCore;
using UserAccess.Infrastructure.Dtos;

namespace UserAccess.Infrastructure
{
    public class UserAccessDBContext : DbContext
    {
        public UserAccessDBContext(DbContextOptions<UserAccessDBContext> options) : base(options)
        {
        }

        public virtual DbSet<UserDto> Users { get; set; }
        public virtual DbSet<RefreshTokenDto> RefreshTokens { get; set; }

    }
}
