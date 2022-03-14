using MovieTracker_API.DTOs;

namespace MovieTracker_API.Interfaces
{
    public interface IGenreRepository
    {
        Task<List<GenreDTO>> GetGenres();
        Task<GenreDTO> GetGenre(int Id);
        Task<bool> CreateGenre(GenreCreationDTO genreCreationDTO);
        Task<bool> UpdateGenre(int Id, GenreCreationDTO genreCreationDTO);
        Task<bool> DeleteGenre(int Id);
    }
}
