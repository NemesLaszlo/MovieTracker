using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieTracker_API.Database;
using MovieTracker_API.DTOs;
using MovieTracker_API.Entities;
using MovieTracker_API.Extensions;
using MovieTracker_API.Helpers;
using MovieTracker_API.Interfaces;

namespace MovieTracker_API.Controllers
{
    public class ActorsController : BaseApiController
    {
        private readonly IActorRepository _actorRepository;
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public ActorsController(IActorRepository actorRepository, DataContext context, IMapper mapper)
        {
            _actorRepository = actorRepository;
            _mapper = mapper;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<ActorDTO>>> Get([FromQuery] PaginationDTO paginationDTO)
        {
            var queryable = _context.Actors.AsQueryable();

            await HttpContext.InsertParametersPaginationInHeader(queryable);

            var actors = await queryable.OrderBy(x => x.Name).Paginate(paginationDTO).ToListAsync();
            return _mapper.Map<List<ActorDTO>>(actors);
        }

        [HttpPost("searchByName")]
        public async Task<ActionResult<List<ActorsMovieDTO>>> SearchByName([FromBody] string Name)
        {
            if (string.IsNullOrWhiteSpace(Name)) 
            { 
                return new List<ActorsMovieDTO>(); 
            }

            return await _actorRepository.GetByName(Name);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ActorDTO>> Get(int Id)
        {
            var actor = await _actorRepository.GetById(Id);

            if (actor is null)
            {
                return NotFound();
            }

            return Ok(actor);
        }

        [HttpPost]
        public async Task<ActionResult> CreateActor([FromForm] ActorCreationDTO actorCreationDTO)
        {
            var actor = _mapper.Map<Actor>(actorCreationDTO);

            /*if (actorCreationDTO.Picture != null)
            {
                actor.Picture = await fileStorageService.SaveFile(containerName, actorCreationDTO.Picture);
            }*/

            _context.Add(actor);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateActor(int Id, [FromForm] ActorCreationDTO actorCreationDTO)
        {
            var actor = await _context.Actors.FirstOrDefaultAsync(x => x.Id == Id);

            if (actor is null)
            {
                return NotFound();
            }

            actor = _mapper.Map(actorCreationDTO, actor);

            /*if (actorCreationDTO.Picture != null)
            {
                actor.Picture = await fileStorageService.EditFile(containerName,
                    actorCreationDTO.Picture, actor.Picture);
            }*/

            _context.Entry(actor).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int Id)
        {
            var actor = await _context.Actors.FirstOrDefaultAsync(x => x.Id == Id);

            if (actor is null)
            {
                return NotFound();
            }

            _context.Remove(actor);
            await _context.SaveChangesAsync();

            // await fileStorageService.DeleteFile(actor.Picture, containerName);

            return NoContent();
        }
    }
}
