using System;
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

            ServiceResponse<ShowUsersDto> response = await _userRepository.CreateUser(
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

            Response.Cookies.Append(key: "RefreshToken", value: response.Data.RefreshToken, new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.Now.AddMinutes(131400),
                Secure = true,
            });

            // return Ok(new { accessToken = response.Data.AccessToken });
            return Ok(response);
        }



        //public async Task<IActionResult> Refresh(RefreshRequest refreshRequest)
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh()
        {
            // bool isValidRefreshToken = _refreshTokenValidator.Validate(refreshRequest.RefreshToken);
            var refreshTokenCookie = Request.Cookies["RefreshToken"];

            // bool isValidRefreshToken = _refreshTokenValidator.Validate(refreshRequest.RefreshToken);
            bool isValidRefreshToken = _refreshTokenValidator.Validate(refreshTokenCookie);

            if (!isValidRefreshToken)
            {
                return BadRequest();
            }

            RefreshToken refreshToken = await _refreshTokenRepository.GetByToken(refreshTokenCookie);

            if (refreshToken is null) { return NotFound(); }

            await _refreshTokenRepository.DeleteById(refreshToken.Id);

            // maybe delete the RefreshToken cookie here?

            var user = await _userRepository.GetUserById(refreshToken.UserId);

            if (user is null) { return NotFound(); }

            AuthenticatedUserResponse response = await _authenticator.Authenticate(user);

            Response.Cookies.Append(key: "RefreshToken", value: response.RefreshToken, new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.Now.AddMinutes(131400),
                Secure = true,
            });

            return Ok(new { accessToken = response.AccessToken });
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

            foreach (var cookie in Request.Cookies)
            {
                Response.Cookies.Delete(cookie.Key, new CookieOptions()
                {
                    Secure = true
                });
            }

            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            ServiceResponse<List<ShowUsersDto>> response = await _userRepository.GetAllUsers();

            return Ok(response);
        }

        [Authorize]
        [HttpGet("checkauth")]
        public IActionResult CheckAuth()
        {
            int? userId = int.Parse(
                            _httpContextAccessor
                            .HttpContext
                            .User.FindFirstValue(ClaimTypes.NameIdentifier)
                        );

            if (userId is null) { return Unauthorized(); }

            return Ok();
        }
    }
}