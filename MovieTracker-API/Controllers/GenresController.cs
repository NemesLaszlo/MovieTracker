using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieTracker_API.DTOs;
using MovieTracker_API.Interfaces;

namespace MovieTracker_API.Controllers
{
    public class GenresController : BaseApiController
    {
        private readonly IGenreRepository _genreRepository;

        public GenresController(IGenreRepository genreRepository)
        {
            _genreRepository = genreRepository;
        }

        [HttpGet] // api/genres
        [AllowAnonymous]
        public async Task<ActionResult<List<GenreDTO>>> Get()
        {
            var genres = await _genreRepository.GetGenres();
            return Ok(genres);
        }

        [HttpGet("{Id:int}", Name = "getGenre")] // api/genres/:id
        public async Task<ActionResult<GenreDTO>> Get(int Id)
        {
            var genre = await _genreRepository.GetGenre(Id);

            if (genre is null)
            {
                return NotFound();
            }

            return Ok(genre);
        }

        [HttpPost]
        public async Task<ActionResult> CreateGenre([FromBody] GenreCreationDTO genreCreationDTO)
        {
            var creationProcess = await _genreRepository.CreateGenre(genreCreationDTO);
            if(creationProcess)
            {
                return NoContent();
            }
            
            return BadRequest();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateGenre(int id, [FromBody] GenreCreationDTO genreCreationDTO)
        {
            var updateProcess = await _genreRepository.UpdateGenre(id, genreCreationDTO);
            if(updateProcess)
            {
                return NoContent();
            }

            return BadRequest();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteGenre(int Id)
        {
            var deleteProcess = await _genreRepository.DeleteGenre(Id);
            if (deleteProcess)
            {
                return NoContent();
            }

            return NotFound();
        }
    }
}
