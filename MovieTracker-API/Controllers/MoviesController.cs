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
        private readonly ITheaterRepository _theaterRepository;
        private readonly IGenreRepository _genreRepository;
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IFileStorageService _fileStorageService;
        private readonly string containerName = "movies";

        public MoviesController(IMovieRepository movieRepository, ITheaterRepository theaterRepository, IGenreRepository genreRepository,
            DataContext context, IMapper mapper, IFileStorageService fileStorageService)
        {
            _movieRepository = movieRepository;
            _theaterRepository = theaterRepository;
            _genreRepository = genreRepository;
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

        // Get Genres and Theaters (DTOs)
        [HttpGet("PostGet")]
        public async Task<ActionResult<MoviePostGetDTO>> PostGet()
        {
            var movieTheaters = await _theaterRepository.GetAll();
            var genres = await _genreRepository.GetGenres();

            return new MoviePostGetDTO() { Genres = genres, MovieTheaters = movieTheaters };
        }


    }
}
