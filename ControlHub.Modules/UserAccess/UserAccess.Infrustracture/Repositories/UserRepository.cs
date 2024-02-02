using Microsoft.EntityFrameworkCore;
using UserAccess.Infrastructure.Dtos;
using UserAccess.Infrastructure.Repositories;

namespace UserAccess.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserAccessDBContext _context;
        public UserRepository(UserAccessDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserDto>> GetAll()
        {
            return await _context.Users.ToListAsync();
        }
        public async Task<UserDto?> GetById(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(obj => obj.UserId == id);
        }
        public async Task<UserDto?> GetByUserName(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(obj => obj.UserName == username || obj.Email == username);
        }
        public async void Add(UserDto userDto)
        {
            _context.Users.Add(userDto);
        }
    }
}
