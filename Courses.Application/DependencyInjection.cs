using Courses.Application.Abstraction.Email;
using Courses.Application.Abstraction.Jwt;
using Courses.Application.Abstraction.TwoFactor;
using Courses.Application.Services.Email;
using Courses.Application.Services.Jwt;
using Courses.Application.Services.TwoFactor;
using Microsoft.Extensions.DependencyInjection;

namespace Courses.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Register MediatR
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

        // Register Services
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<ITwoFactorService, TwoFactorService>();
        services.AddScoped<IJwtService, JwtService>();

        return services;
    }
}