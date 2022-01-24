using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using dotnet_blog_api.Dtos;
using dotnet_blog_api.Dtos.LoginDtos;
using dotnet_blog_api.Dtos.TokenDtos;
using dotnet_blog_api.Models;
using dotnet_blog_api.Services;
using dotnet_blog_api.Services.Authenticators;
using dotnet_blog_api.Services.RefreshTokenRepository;
using dotnet_blog_api.Services.TokenValidators;
using dotnet_blog_api.Services.UserRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_blog_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly RefreshTokenValidator _refreshTokenValidator;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly Authenticator _authenticator;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AuthController(IUserRepository userRepository, RefreshTokenValidator refreshTokenValidator, IRefreshTokenRepository refreshTokenRepository, Authenticator authenticator, IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _refreshTokenValidator = refreshTokenValidator;
            _refreshTokenRepository = refreshTokenRepository;
            _authenticator = authenticator;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequestDto registerRequestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (registerRequestDto.Password != registerRequestDto.ConfirmPassword)
            {
                return BadRequest(new { message = "Passwords do not match" });
            }

            ServiceResponse<int> response = await _userRepository.CreateUser(
                new User
                {
                    Email = registerRequestDto.Email,
                    Username = registerRequestDto.Username
                }, registerRequestDto.Password);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDto loginRequestDto)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            ServiceResponse<AuthenticatedUserResponse> response = await _userRepository.Login(loginRequestDto);

            if (!response.Success) { return BadRequest(response); }

            return Ok(response);
        }


        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(RefreshRequest refreshRequest)
        {
            bool isValidRefreshToken = _refreshTokenValidator.Validate(refreshRequest.RefreshToken);

            if (!isValidRefreshToken)
            {
                return BadRequest();
            }

            RefreshToken refreshToken = await _refreshTokenRepository.GetByToken(refreshRequest.RefreshToken);

            if (refreshToken is null) { return NotFound(); }

            await _refreshTokenRepository.DeleteById(refreshToken.Id);

            var user = await _userRepository.GetUserById(refreshToken.UserId);

            if (user is null) { return NotFound(); }

            AuthenticatedUserResponse response = await _authenticator.Authenticate(user);

            return Ok(response);
        }

        [Authorize]
        [HttpDelete("logout")]
        public async Task<IActionResult> logout()
        {
            int? userId = int.Parse(
                            _httpContextAccessor
                            .HttpContext
                            .User.FindFirstValue(ClaimTypes.NameIdentifier)
                        );

            if (userId is null) { return Unauthorized(); }

            await _refreshTokenRepository.DeleteAll(userId);

            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            ServiceResponse<List<ShowUsersDto>> response = await _userRepository.GetAllUsers();

            return Ok(response);
        }
    }
}