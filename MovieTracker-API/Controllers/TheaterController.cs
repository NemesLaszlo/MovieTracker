using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieTracker_API.Database;
using MovieTracker_API.DTOs;
using MovieTracker_API.Entities;
using MovieTracker_API.Interfaces;

namespace MovieTracker_API.Controllers
{
    public class TheaterController : BaseApiController
    {
        private readonly ITheaterRepository _theaterRepository;
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public TheaterController(ITheaterRepository theaterRepository, DataContext context, IMapper mapper)
        {
            _theaterRepository = theaterRepository;
            _mapper = mapper;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<MovieTheaterDTO>>> Get()
        {
            var entities = await _theaterRepository.GetAll();
            return Ok(entities);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<MovieTheaterDTO>> Get(int Id)
        {
            var movieTheater = await _theaterRepository.GetById(Id);

            if (movieTheater is null)
            {
                return NotFound();
            }

            return Ok(movieTheater);
        }

        [HttpPost]
        public async Task<ActionResult> CreateTheater(MovieTheaterCreationDTO movieCreationDTO)
        {
            var movieTheater = _mapper.Map<MovieTheater>(movieCreationDTO);
            _context.Add(movieTheater);

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateTheater(int Id, MovieTheaterCreationDTO movieCreationDTO)
        {
            var movieTheater = await _context.MovieTheaters.FirstOrDefaultAsync(x => x.Id == Id);

            if (movieTheater is null)
            {
                return NotFound();
            }

            movieTheater = _mapper.Map(movieCreationDTO, movieTheater);

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteTheater(int id)
        {
            var movieTheater = await _context.MovieTheaters.FirstOrDefaultAsync(x => x.Id == id);

            if (movieTheater == null)
            {
                return NotFound();
            }

            _context.Remove(movieTheater);

            await _context.SaveChangesAsync();
            return NoContent();
        }

    }
}
