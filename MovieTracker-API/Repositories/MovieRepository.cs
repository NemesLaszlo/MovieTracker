using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MovieTracker_API.Database;
using MovieTracker_API.DTOs;
using MovieTracker_API.Entities;
using MovieTracker_API.Interfaces;

namespace MovieTracker_API.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public MovieRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Movie> GetMovieById(int Id)
        {
            return await _context.Movies
                .Include(x => x.MoviesGenres).ThenInclude(x => x.Genre)
                .Include(x => x.MovieTheatersMovies).ThenInclude(x => x.MovieTheater)
                .Include(x => x.MoviesActors).ThenInclude(x => x.Actor)
                .FirstOrDefaultAsync(x => x.Id == Id);
        }

        public async Task<List<Movie>> GetUpcomingReleases(int TopMoviesNumber, DateTime Date)
        {
            return await _context.Movies
                .Where(x => x.ReleaseDate > Date)
                .OrderBy(x => x.ReleaseDate)
                .Take(TopMoviesNumber)
                .ToListAsync();
        }

        public async Task<List<Movie>> GetInTheaters(int TopMoviesNumber)
        {
            return await _context.Movies
                .Where(x => x.InTheaters)
                .OrderBy(x => x.ReleaseDate)
                .Take(TopMoviesNumber)
                .ToListAsync();
        }

        public async Task<List<MovieDTO>> Filter(FilterMoviesDTO filterMoviesDTO)
        {
            throw new NotImplementedException();
        }
    }
}
