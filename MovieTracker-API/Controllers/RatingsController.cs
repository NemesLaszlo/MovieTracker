using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieTracker_API.Database;
using MovieTracker_API.DTOs;
using MovieTracker_API.Entities;

namespace MovieTracker_API.Controllers
{
    public class RatingsController: BaseApiController
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public RatingsController(UserManager<IdentityUser> userManager, DataContext context, IMapper mapper)
        {
            _userManager = userManager;
            _context = context;
            _mapper = mapper;
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> GiveRating([FromBody] RatingDTO ratingDTO)
        {
            var email = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "email")?.Value;

            var user = await _userManager.FindByEmailAsync(email);
            var userId = user.Id;

            var currentRate = await _context.Ratings.FirstOrDefaultAsync(x => x.MovieId == ratingDTO.MovieId && x.UserId == userId);

            if (currentRate == null)
            {
                var rating = new Rating();
                rating.MovieId = ratingDTO.MovieId;
                rating.Rate = ratingDTO.Rating;
                rating.UserId = userId;

                _context.Add(rating);
            }
            else
            {
                currentRate.Rate = ratingDTO.Rating;
            }

            await _context.SaveChangesAsync();
            
            return NoContent();
        }
    }
}
