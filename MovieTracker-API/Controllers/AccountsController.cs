using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieTracker_API.DTOs;
using MovieTracker_API.Extensions;
using MovieTracker_API.Helpers;
using MovieTracker_API.Interfaces;
using System.Security.Claims;

namespace MovieTracker_API.Controllers
{
    public class AccountsController : BaseApiController
    {
        private readonly ITokenService _tokenService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IMapper _mapper;

        public AccountsController(ITokenService tokenService, UserManager<IdentityUser> userManager, 
            SignInManager<IdentityUser> signInManager, IMapper mapper)
        {
            _tokenService = tokenService;
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<AuthenticationResponse>> Registration([FromBody] UserCredentials userCredentials)
        {
            var user = new IdentityUser { UserName = userCredentials.Email, Email = userCredentials.Email };

            var result = await _userManager.CreateAsync(user, userCredentials.Password);

            if (result.Succeeded)
            {
                return await _tokenService.BuildToken(userCredentials);
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<AuthenticationResponse>> Login([FromBody] UserCredentials userCredentials)
        {
            var result = await _signInManager.PasswordSignInAsync(userCredentials.Email,
                userCredentials.Password, isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return await _tokenService.BuildToken(userCredentials);
            }
            else
            {
                return BadRequest("Incorrect Login");
            }
        }

        [HttpGet("listUsers")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "IsAdmin")]
        public async Task<ActionResult<List<UserDTO>>> GetListUsers([FromQuery] PaginationDTO paginationDTO)
        {
            var queryable = _userManager.Users.AsQueryable();

            await HttpContext.InsertParametersPaginationInHeader(queryable);

            var users = await queryable.OrderBy(x => x.Email).Paginate(paginationDTO).ToListAsync();

            return _mapper.Map<List<UserDTO>>(users);
        }

        [HttpPost("makeAdmin")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "IsAdmin")]
        public async Task<ActionResult> MakeAdmin([FromBody] string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            await _userManager.AddClaimAsync(user, new Claim("role", "admin"));

            return NoContent();
        }

        [HttpPost("removeAdmin")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "IsAdmin")]
        public async Task<ActionResult> RemoveAdmin([FromBody] string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            await _userManager.RemoveClaimAsync(user, new Claim("role", "admin"));

            return NoContent();
        }
    }
}
