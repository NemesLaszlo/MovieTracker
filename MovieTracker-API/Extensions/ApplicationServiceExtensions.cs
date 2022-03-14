using Microsoft.EntityFrameworkCore;
using MovieTracker_API.Database;
using MovieTracker_API.MapperProfiles;

namespace MovieTracker_API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static void AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            // SQL Database
            services.AddDbContext<DataContext>(options =>
                options.UseSqlServer(config.GetConnectionString("DefaultConnection"), sqlOptions => sqlOptions.UseNetTopologySuite()));

            // CORS Policy
            services.AddCors(opt =>
            {
                opt.AddPolicy("CorsPolicy", policy =>
                {
                    policy
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials()
                        .WithOrigins("http://localhost:4200");
                });
            });

            // Services
            services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);
        }
    }
}
