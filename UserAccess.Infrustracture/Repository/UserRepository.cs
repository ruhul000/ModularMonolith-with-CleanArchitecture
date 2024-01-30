using Microsoft.EntityFrameworkCore;
using UserAccess.Infrastructure.Dtos;

namespace UserAccess.Infrastructure.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly UserContext _context;
        public UserRepository(UserContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserDto>> GetAll()
        {
            return await _context.Users.ToListAsync();
        }
        public async Task<UserDto> GetById(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(obj => obj.UserId == id);
        }
        public async Task<UserDto> GetByUserName(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(obj => obj.UserName == username || obj.Email == username);
        }
        public async Task<bool> Add(UserDto userDto)
        {
            _context.Users.Add(userDto);
            return (await _context.SaveChangesAsync() > 0);
        }
    }
}
