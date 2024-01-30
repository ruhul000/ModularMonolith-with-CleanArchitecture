using UserAccess.Domain.Factories.FactoryMapper;
using UserAccess.Domain.Models;
using UserAccess.Infrastructure.Dtos;

namespace UserAccess.Domain.Factories
{
    public class UserFactory : IUserFactory
    {
        IMappingFactory<User> _userFactory;
        IMappingFactory<UserDto> _userDtoFactory;
        IMappingFactory<UserResponse> _userResponseFactory;

        public UserFactory(IMappingFactory<User> userFactory, IMappingFactory<UserDto> userDtoFactory, IMappingFactory<UserResponse> userResponseFactory)
        {
            _userFactory = userFactory;
            _userDtoFactory = userDtoFactory;
            _userResponseFactory = userResponseFactory;
        }

        public UserDto CreateFrom(User user)
        {
            UserDto response = _userDtoFactory.CreateFrom(user);
            return response;
        }

        public User CreateFrom(UserDto userDto)
        {
            User response = _userFactory.CreateFrom(userDto);
            return response;
        }

        public UserResponse CreateResponseFrom(UserDto userDto)
        {
            UserResponse response = _userResponseFactory.CreateFrom(userDto);
            return response;
        }

        public IEnumerable<UserDto> CreateMultipleFrom(IEnumerable<User> user)
        {
            IEnumerable<UserDto> response = _userDtoFactory.CreateMultipleFrom(user);
            return response;
        }

        public IEnumerable<User> CreateMultipleFrom(IEnumerable<UserDto> userDtos)
        {
            IEnumerable<User> response = _userFactory.CreateMultipleFrom(userDtos);
            return response;
        }

        public IEnumerable<UserResponse> CreateMultipleResponseFrom(IEnumerable<UserDto> userDtos)
        {
            IEnumerable<UserResponse> response = _userResponseFactory.CreateMultipleFrom(userDtos);
            return response;
        }
    }
}
