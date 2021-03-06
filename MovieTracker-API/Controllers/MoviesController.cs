using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "IsAdmin")]
    public class MoviesController : BaseApiController
    {
        private readonly IMovieRepository _movieRepository;
        private readonly ITheaterRepository _theaterRepository;
        private readonly IGenreRepository _genreRepository;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IFileStorageService _fileStorageService;
        private readonly string containerName = "movies";

        public MoviesController(IMovieRepository movieRepository, ITheaterRepository theaterRepository, IGenreRepository genreRepository,
            DataContext context, IMapper mapper, IFileStorageService fileStorageService, UserManager<IdentityUser> userManager)
        {
            _movieRepository = movieRepository;
            _theaterRepository = theaterRepository;
            _genreRepository = genreRepository;
            _userManager = userManager;
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

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateMovie(int id, [FromForm] MovieCreationDTO movieCreationDTO)
        {
            var movie = await _movieRepository.GetMovieById(id);

            if (movie is null)
            {
                return NotFound();
            }

            movie = _mapper.Map(movieCreationDTO, movie);

            if (movieCreationDTO.Poster != null)
            {
                movie.Poster = await _fileStorageService.EditFile(containerName, movieCreationDTO.Poster, movie.Poster);
            }

            AnnotateActorsOrder(movie);

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteMovie(int id)
        {
            var movie = await _context.Movies.FirstOrDefaultAsync(x => x.Id == id);

            if (movie is null)
            {
                return NotFound();
            }

            _context.Remove(movie);
            await _context.SaveChangesAsync();

            await _fileStorageService.DeleteFile(containerName, movie.Poster);

            return NoContent();
        }

        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<MovieDTO>> GetMovie(int Id)
        {
            var movie = await _movieRepository.GetMovieById(Id);

            if (movie is null)
            {
                return NotFound();
            }

            var averageVote = 0.0;
            var userVote = 0;

            if (await _context.Ratings.AnyAsync(x => x.MovieId == Id))
            {
                averageVote = await _context.Ratings.Where(x => x.MovieId == Id).AverageAsync(x => x.Rate);

                if (HttpContext.User.Identity.IsAuthenticated)
                {
                    var email = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "email")?.Value;
                    var user = await _userManager.FindByEmailAsync(email);
                    var userId = user.Id;

                    var ratingDb = await _context.Ratings.FirstOrDefaultAsync(x => x.MovieId == Id && x.UserId == userId);

                    if (ratingDb != null)
                    {
                        userVote = ratingDb.Rate;
                    }
                }
            }

            var dto = _mapper.Map<MovieDTO>(movie);

            dto.AverageVote = averageVote;
            dto.UserVote = userVote;

            dto.Actors = dto.Actors.OrderBy(x => x.Order).ToList();

            return dto;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<HomeDTO>> GetLandingInformations()
        {
            var top = 6;
            var today = DateTime.Today;

            var upcomingReleases = await _movieRepository.GetUpcomingReleases(top, today);

            var inTheaters = await _movieRepository.GetInTheaters(top);

            var homeDTO = new HomeDTO();

            homeDTO.UpcomingReleases = _mapper.Map<List<MovieDTO>>(upcomingReleases);
            homeDTO.InTheaters = _mapper.Map<List<MovieDTO>>(inTheaters);

            return homeDTO;
        }

        [AllowAnonymous]
        [HttpGet("filter")]
        public async Task<ActionResult<List<MovieDTO>>> Filter([FromQuery] FilterMoviesDTO filterMoviesDTO)
        {
            var moviesQueryable = _movieRepository.Filter(filterMoviesDTO); 

            await HttpContext.InsertParametersPaginationInHeader(moviesQueryable);
            var movies = await moviesQueryable.OrderBy(x => x.Title).Paginate(filterMoviesDTO.PaginationDTO).ToListAsync();

            return _mapper.Map<List<MovieDTO>>(movies);
        }

        // Get Genres and Theaters (DTOs)
        [HttpGet("PostGet")]
        public async Task<ActionResult<MoviePostGetDTO>> PostGet()
        {
            var movieTheaters = await _theaterRepository.GetAll();
            var genres = await _genreRepository.GetGenres();

            return new MoviePostGetDTO() { Genres = genres, MovieTheaters = movieTheaters };
        }

        // Get movie details to edit
        [HttpGet("putget/{id:int}")]
        public async Task<ActionResult<MoviePutGetDTO>> PutGet(int id)
        {
            var movieInformationsForEdit = await _movieRepository.GetMovieForEdit(id);

            if(movieInformationsForEdit is null)
            {
                return NotFound();
            }

            return movieInformationsForEdit;
        }




    }
}
