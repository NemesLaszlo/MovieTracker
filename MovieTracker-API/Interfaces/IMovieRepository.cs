using MovieTracker_API.DTOs;
using MovieTracker_API.Entities;

namespace MovieTracker_API.Interfaces
{
    public interface IMovieRepository
    {
        Task<List<Movie>> GetUpcomingReleases(int TopMoviesNumber, DateTime Date);
        Task<List<Movie>> GetInTheaters(int TopMoviesNumber);
        Task<Movie> GetMovieById(int Id);
        Task<MoviePutGetDTO> GetMovieForEdit(int Id);
        IQueryable<Movie> Filter(FilterMoviesDTO filterMoviesDTO);
    }
}
