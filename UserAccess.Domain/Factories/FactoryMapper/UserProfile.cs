using AutoMapper;
using UserAccess.Domain.Models;
using UserAccess.Infrastructure.Dtos;

namespace UserAccess.Domain.Factories.FactoryMapper
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserDto, User>().ReverseMap();
        }
    }
}
