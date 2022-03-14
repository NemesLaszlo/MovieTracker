using MovieTracker_API.DTOs;

namespace MovieTracker_API.Interfaces
{
    public interface IActorRepository
    {
        Task<List<ActorsMovieDTO>> GetByName(string Name);
        Task<ActorDTO> GetById(int Id);
    }
}
