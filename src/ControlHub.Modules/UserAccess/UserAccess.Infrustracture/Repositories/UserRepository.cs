using Microsoft.EntityFrameworkCore;
using UserAccess.Infrastructure.Dtos;

namespace UserAccess.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserAccessDBContext _context;
        public UserRepository(UserAccessDBContext context)
        {
            _context = context;
        }
        public async Task<bool> IsUserNameExist(string username)
            => username != "" ? await _context.Users.AnyAsync(obj => obj.UserName == username && !obj.IsDeleted) : false;

        public async Task<bool> IsEmailAddressExist(string email)
            => email != "" ? await _context.Users.AnyAsync(obj => obj.Email == email && !obj.IsDeleted) : false;

        public async Task<IEnumerable<UserDto>> GetAll()
            => await _context.Users.Where(obj => !obj.IsDeleted).ToListAsync();

        public async Task<UserDto?> GetById(int id)
            => await _context.Users.FirstOrDefaultAsync(obj => obj.UserId == id);

        public async Task<UserDto?> GetByUserName(string username)
            => await _context.Users.FirstOrDefaultAsync(obj => obj.UserName == username || obj.Email == username && !obj.IsDeleted);

        public async Task<UserDto?> GetUserByVerificationToken(string verificationToken)
            => await _context.Users.FirstOrDefaultAsync(obj => obj.VerificationToken == verificationToken);

        public async Task<UserDto?> GetUserByPasswordResetToken(string passwordResetToken)
         => await _context.Users.FirstOrDefaultAsync(obj => obj.PasswordResetToken == passwordResetToken);

        public async void Add(UserDto userDto)
        {
            userDto.CreatedAt = DateTime.UtcNow;
            userDto.CreatedBy = userDto.UserId;
            await _context.Users.AddAsync(userDto);
        }

        public void Update(UserDto userDto)
        {
            userDto.UpdatedAt = DateTime.UtcNow;
            userDto.UpdatedBy = userDto.UserId;
            _context.Users.Update(userDto);
        }
    }

}
