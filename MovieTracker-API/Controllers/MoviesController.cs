using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MovieTracker_API.Database;
using MovieTracker_API.DTOs;
using MovieTracker_API.Entities;
using MovieTracker_API.Interfaces;

namespace MovieTracker_API.Controllers
{
    public class MoviesController : BaseApiController
    {
        private readonly IMovieRepository _movieRepository;
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IFileStorageService _fileStorageService;
        private readonly string containerName = "movies";

        public MoviesController(IMovieRepository movieRepository, DataContext context, IMapper mapper, 
            IFileStorageService fileStorageService)
        {
            _movieRepository = movieRepository;
            _fileStorageService = fileStorageService;
            _mapper = mapper;
            _context = context;
        }

        private void AnnotateActorsOrder(Movie movie)
        {
            if (movie.MoviesActors != null)
            {
                for (int i = 0; i < movie.MoviesActors.Count; i++)
                {
                    movie.MoviesActors[i].Order = i;
                }
            }
        }

        [HttpPost]
        public async Task<ActionResult<int>> CreateMovie([FromForm] MovieCreationDTO movieCreationDTO)
        {
            var movie = _mapper.Map<Movie>(movieCreationDTO);

            if (movieCreationDTO.Poster != null)
            {
                movie.Poster = await _fileStorageService.SaveFile(containerName, movieCreationDTO.Poster);
            }

            AnnotateActorsOrder(movie);

            _context.Add(movie);
            await _context.SaveChangesAsync();

            return movie.Id;
        }


    }
}
