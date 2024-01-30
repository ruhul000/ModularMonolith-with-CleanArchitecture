using UserAccess.Infrastructure.Dtos;

namespace UserAccess.Infrastructure.Repository
{
    public interface IUserRepository
    {
        Task<IEnumerable<UserDto>> GetAll();
        Task<UserDto> GetById(int id);
        Task<UserDto> GetByUserName(string username);
        Task<bool> Add(UserDto user);
    }
}
