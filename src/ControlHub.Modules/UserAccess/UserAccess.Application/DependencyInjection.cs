using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using UserAccess.Application.Services;
using UserAccess.Application.Validators;
using UserAccess.Domain.Models;

namespace UserAccess.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddUserAccessApplication(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IValidator<UserRequest>, UserRegistrationValidator>();
            services.AddScoped<IValidator<UserLoginRequest>, UserLoginValidator>();
            return services;
        }
    }
}
