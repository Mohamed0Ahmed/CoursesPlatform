using Courses.Infrastructure;

namespace Courses.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Configure Services

            builder.Services.AddAuthorization();
            builder.Services.AddInfrastructure(builder.Configuration);

            // Add Swagger
            builder.Services.AddEndpointsApiExplorer().AddSwaggerGen();

            #endregion


            var app = builder.Build();

            #region Apply Migrations

            // Apply migrations automatically
            //app.UseInfrastructure();

            #endregion



            #region Configure Middleware

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseStaticFiles();

            app.UseHttpsRedirection();
            //app.UseAuthentication();
            app.UseAuthorization();

            //app.MapControllers();

            #endregion

            app.Run();
        }
    }
}
