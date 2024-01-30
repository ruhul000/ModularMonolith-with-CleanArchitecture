using UserAccess.Domain.Models;
using UserAccess.Infrastructure.Dtos;

namespace UserAccess.Domain.Factories
{
    public interface IUserFactory
    {
        UserDto CreateFrom(User user);
        User CreateFrom(UserDto userDto);
        UserResponse CreateResponseFrom(UserDto userDto);
        IEnumerable<UserDto> CreateMultipleFrom(IEnumerable<User> user);
        IEnumerable<User> CreateMultipleFrom(IEnumerable<UserDto> userDtos);
        IEnumerable<UserResponse> CreateMultipleResponseFrom(IEnumerable<UserDto> userDtos);
    }
}
