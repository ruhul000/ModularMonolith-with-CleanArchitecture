using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UserAccess.Infrastructure.Repository;

namespace UserAccess.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddUserAccessInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

            return services;
        }
    }
}
