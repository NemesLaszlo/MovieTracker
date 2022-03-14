using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MovieTracker_API.Entities;
using System.Diagnostics.CodeAnalysis;

namespace MovieTracker_API.Database
{
    public class DataContext: IdentityDbContext
    {
        public DataContext([NotNullAttribute] DbContextOptions options) : base(options) { }

        public DbSet<Genre> Genres { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<MovieTheater> MovieTheaters { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<MoviesActors> MoviesActors { get; set; }
        public DbSet<MoviesGenres> MoviesGenres { get; set; }
        public DbSet<MovieTheatersMovies> MovieTheatersMovies { get; set; }
        public DbSet<Rating> Ratings { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<MoviesActors>().HasKey(x => new { x.ActorId, x.MovieId });

            builder.Entity<MoviesGenres>().HasKey(x => new { x.GenreId, x.MovieId });

            builder.Entity<MovieTheatersMovies>().HasKey(x => new { x.MovieTheaterId, x.MovieId });
        }
    }
}
