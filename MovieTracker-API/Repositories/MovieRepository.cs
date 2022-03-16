using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MovieTracker_API.Database;
using MovieTracker_API.DTOs;
using MovieTracker_API.Entities;
using MovieTracker_API.Helpers;
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

        public IQueryable<Movie> Filter(FilterMoviesDTO filterMoviesDTO)
        {
            var moviesQueryable = _context.Movies.AsQueryable();

            if (!string.IsNullOrEmpty(filterMoviesDTO.Title))
            {
                moviesQueryable = moviesQueryable.Where(x => x.Title.Contains(filterMoviesDTO.Title));
            }

            if (filterMoviesDTO.InTheaters)
            {
                moviesQueryable = moviesQueryable.Where(x => x.InTheaters);
            }

            if (filterMoviesDTO.UpcomingReleases)
            {
                var today = DateTime.Today;
                moviesQueryable = moviesQueryable.Where(x => x.ReleaseDate > today);
            }

            if (filterMoviesDTO.GenreId != 0)
            {
                moviesQueryable = moviesQueryable
                    .Where(x => x.MoviesGenres.Select(y => y.GenreId)
                    .Contains(filterMoviesDTO.GenreId));
            }

            return moviesQueryable;
        }

        public async Task<MoviePutGetDTO?> GetMovieForEdit(int Id)
        {
            var movieResult = await GetMovieById(Id);
            if (movieResult is null) { return null; }

            var movie = _mapper.Map<MovieDTO>(movieResult);

            var genresSelectedIds = movie.Genres.Select(x => x.Id).ToList();

            var nonSelectedGenres = await _context.Genres.Where(x => !genresSelectedIds.Contains(x.Id)).ToListAsync();

            var movieTheatersIds = movie.MovieTheaters.Select(x => x.Id).ToList();

            var nonSelectedMovieTheaters = await _context.MovieTheaters.Where(x => !movieTheatersIds.Contains(x.Id)).ToListAsync();

            var nonSelectedGenresDTOs = _mapper.Map<List<GenreDTO>>(nonSelectedGenres);
            var nonSelectedMovieTheatersDTO = _mapper.Map<List<MovieTheaterDTO>>(nonSelectedMovieTheaters);

            var response = new MoviePutGetDTO();
            response.Movie = movie;
            response.SelectedGenres = movie.Genres;
            response.NonSelectedGenres = nonSelectedGenresDTOs;
            response.SelectedMovieTheaters = movie.MovieTheaters;
            response.NonSelectedMovieTheaters = nonSelectedMovieTheatersDTO;
            response.Actors = movie.Actors;

            return response;
        }

    }
}
