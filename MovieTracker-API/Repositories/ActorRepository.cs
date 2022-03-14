using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MovieTracker_API.Database;
using MovieTracker_API.DTOs;
using MovieTracker_API.Interfaces;

namespace MovieTracker_API.Repositories
{
    public class ActorRepository : IActorRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public ActorRepository(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<ActorDTO> GetById(int Id)
        {
            var actor = await _context.Actors.FirstOrDefaultAsync(x => x.Id == Id);
            return _mapper.Map<ActorDTO>(actor);
        }

        public async Task<List<ActorsMovieDTO>> GetByName(string Name)
        {
            var actors = await _context.Actors
                .Where(x => x.Name.Contains(Name))
                .OrderBy(x => x.Name)
                .Select(x => new ActorsMovieDTO { Id = x.Id, Name = x.Name, Picture = x.Picture })
                .Take(5)
                .ToListAsync();

            return actors;
        }
    }
}
