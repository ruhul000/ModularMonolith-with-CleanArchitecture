using Microsoft.Extensions.DependencyInjection;
using UserAccess.Infrastructure.Repositories;

namespace UserAccess.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddUserAccessInfrastructure(this IServiceCollection services)
        {
            //Repositories
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            return services;
        }
    }
}
