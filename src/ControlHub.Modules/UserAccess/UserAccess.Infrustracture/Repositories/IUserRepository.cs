using UserAccess.Infrastructure.Dtos;

namespace UserAccess.Infrastructure.Repositories
{
    public interface IUserRepository
    {
        Task<bool> IsUserNameExist(string email);
        Task<bool> IsEmailAddressExist(string username);
        Task<IEnumerable<UserDto>> GetAll();
        Task<UserDto?> GetById(int id);
        Task<UserDto?> GetByUserName(string username);
        Task<UserDto?> GetUserByVerificationToken(string verificationToken);
        Task<UserDto?> GetUserByEmailAndResetCode(string email, string resetCode);
        void Add(UserDto user);
        void Update(UserDto userDto);
    }
}
