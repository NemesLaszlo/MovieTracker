using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MovieTracker_API.Database;
using MovieTracker_API.DTOs;
using MovieTracker_API.Interfaces;

namespace MovieTracker_API.Repositories
{
    public class TheaterRepository : ITheaterRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public TheaterRepository(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<List<MovieTheaterDTO>> GetAll()
        {
            var movieTheaters = await _context.MovieTheaters.OrderBy(x => x.Name).ToListAsync();
            return _mapper.Map<List<MovieTheaterDTO>>(movieTheaters);
        }

        public async Task<MovieTheaterDTO> GetById(int Id)
        {
            var movieTheater = await _context.MovieTheaters.FirstOrDefaultAsync(x => x.Id == Id);
            return _mapper.Map<MovieTheaterDTO>(movieTheater);
        }
    }
}
