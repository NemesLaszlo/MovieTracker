using MovieTracker_API.DTOs;

namespace MovieTracker_API.Interfaces
{
    public interface ITheaterRepository
    {
        Task<List<MovieTheaterDTO>> GetAll();
        Task<MovieTheaterDTO> GetById(int Id);
    }
}
