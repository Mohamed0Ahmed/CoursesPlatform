

namespace Courses.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Register Email Service
            services.AddScoped<IEmailService, EmailService>();

            // Register Two-Factor Service
            services.AddScoped<ITwoFactorService, TwoFactorService>();

            // Add Memory Cache for 2FA codes
            services.AddMemoryCache();

            return services;
        }
    }
}