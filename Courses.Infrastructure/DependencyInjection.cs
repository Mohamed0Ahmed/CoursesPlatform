
namespace Courses.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Add DbContext
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("AppContext")));

            return services;
        }

        public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            try
            {
                // Apply any pending migrations, and create the database if it doesn't exist.
                dbContext.Database.Migrate();
                Console.WriteLine("Database migrations checked and applied successfully!");


                // Seed default roles
                SeedDefaultRoles(roleManager).Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error applying migrations: {ex.Message}");
                throw;
            }

            return app;
        }

        private static async Task SeedDefaultRoles(RoleManager<IdentityRole> roleManager)
        {
            string[] roleNames = { "Admin", "Instructor", "Student" };

            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                    Console.WriteLine($"Role '{roleName}' created successfully!");
                }
            }
        }
    }
}
