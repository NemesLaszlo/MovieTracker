using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MovieTracker_API.Database;
using MovieTracker_API.Interfaces;
using MovieTracker_API.MapperProfiles;
using MovieTracker_API.Repositories;
using NetTopologySuite;
using NetTopologySuite.Geometries;

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
                        .WithOrigins("http://localhost:4200")
                        .WithExposedHeaders(new string[] { "totalAmountOfRecords" });
                });
            });

            // Services
            services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);
            services.AddScoped<IGenreRepository, GenreRepository>();
            services.AddScoped<IActorRepository, ActorRepository>();

            services.AddSingleton(provider => new MapperConfiguration(config =>
            {
                var geometryFactory = provider.GetRequiredService<GeometryFactory>();
                config.AddProfile(new AutoMapperProfiles(geometryFactory));
            }).CreateMapper());

            services.AddSingleton<GeometryFactory>(NtsGeometryServices
                .Instance.CreateGeometryFactory(srid: 4326));


            // Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MovieTracker_API", Version = "v1" });
            });
        }
    }
}
