using Microsoft.Extensions.DependencyInjection;
using UserAccess.Domain.Factories;
using UserAccess.Domain.Factories.FactoryMapper;

namespace UserAccess.Domain
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddUserAccessDomain(this IServiceCollection services)
        {
            services.AddScoped(typeof(IMappingFactory<>), typeof(MappingFactory<>));
            services.AddAutoMapper(typeof(UserProfile));
            services.AddScoped<IUserFactory, UserFactory>();
            return services;
        }
    }
}
