using Courses.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Courses.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("AppContext")));

            return services;
        }

        //public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app)
        //{
        //    using var scope = app.ApplicationServices.CreateScope();
        //    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        //    try
        //    {
        //        // Ensure database is created
        //        dbContext.Database.EnsureCreated();

        //        // Apply any pending migrations
        //        if (dbContext.Database.GetPendingMigrations().Any())
        //        {
        //            dbContext.Database.Migrate();
        //            Console.WriteLine("✅ Database migrations applied successfully!");
        //        }
        //        else
        //        {
        //            Console.WriteLine("✅ Database is up to date!");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"❌ Error applying migrations: {ex.Message}");
        //        throw;
        //    }

        //    return app;
        //}
    }
}
