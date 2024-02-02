using UserAccess.Infrastructure.Dtos;

namespace UserAccess.Infrastructure.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<UserDto>> GetAll();
        Task<UserDto?> GetById(int id);
        Task<UserDto?> GetByUserName(string username);
        void Add(UserDto user);
    }
}
