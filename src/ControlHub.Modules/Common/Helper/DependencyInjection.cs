using Helper.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Helper
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddHelper(this IServiceCollection services)
        {
            services.AddScoped<IEmailSender, EmailSender>();
            return services;
        }
    }
}
