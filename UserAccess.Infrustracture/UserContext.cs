using Microsoft.EntityFrameworkCore;
using UserAccess.Infrastructure.Dtos;

namespace UserAccess.Infrastructure
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {
        }

        public virtual DbSet<UserDto> Users { get; set; }
        public virtual DbSet<RefreshTokenDto> RefreshTokens { get; set; }

    }
}
