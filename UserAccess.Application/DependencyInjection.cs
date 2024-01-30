using Microsoft.Extensions.DependencyInjection;
using UserAccess.Application.Services;

namespace UserAccess.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddUserAccessApplication(this IServiceCollection services)
        {
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IUserService, UserService>();
            return services;
        }
    }
}
