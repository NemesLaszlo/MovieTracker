using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MovieTracker_API.Database;
using MovieTracker_API.DTOs;
using MovieTracker_API.Entities;
using MovieTracker_API.Interfaces;

namespace MovieTracker_API.Repositories
{
    public class GenreRepository : IGenreRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public GenreRepository(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<List<GenreDTO>> GetGenres()
        {
            var genres = await _context.Genres.OrderBy(x => x.Name).ToListAsync();
            return _mapper.Map<List<GenreDTO>>(genres);
        }

        public async Task<GenreDTO> GetGenre(int Id)
        {
            var genre = await _context.Genres.FirstOrDefaultAsync(x => x.Id == Id);
            return _mapper.Map<GenreDTO>(genre);
        }

        public async Task<bool> CreateGenre(GenreCreationDTO genreCreationDTO)
        {
            var genre = _mapper.Map<Genre>(genreCreationDTO);
            var existingCheck = await _context.Genres.AnyAsync(x => x.Name.Equals(genre.Name));
            if(existingCheck)
            {
                return false;
            }
            _context.Add(genre);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateGenre(int Id, GenreCreationDTO genreCreationDTO)
        {
            var genre = _mapper.Map<Genre>(genreCreationDTO);
            genre.Id = Id;
            _context.Entry(genre).State = EntityState.Modified;
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteGenre(int Id)
        {
            var genre = await _context.Genres.FirstOrDefaultAsync(x => x.Id == Id);
            if(genre is null)
            {
                return false;
            }
            _context.Remove(genre);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
