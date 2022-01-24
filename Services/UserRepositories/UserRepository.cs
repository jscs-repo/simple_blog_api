using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnet_blog_api.Data;
using dotnet_blog_api.Dtos;
using dotnet_blog_api.Dtos.LoginDtos;
using dotnet_blog_api.Models;
using dotnet_blog_api.Services.Authenticators;
using dotnet_blog_api.Services.PasswordHasher;
using Microsoft.EntityFrameworkCore;

namespace dotnet_blog_api.Services.UserRepositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        private readonly IPasswordHasher _passwordHasher;
        private readonly Authenticator _authenticator;

        public UserRepository(DataContext context, IPasswordHasher passwordHasher, Authenticator authenticator)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _authenticator = authenticator;
        }

        public async Task<ServiceResponse<int>> CreateUser(User user, string password)
        {
            ServiceResponse<int> serviceResponse = new ServiceResponse<int>();

            var userEmail = await GetByEmail(user.Email);
            if (userEmail is not null)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "Email in use";
                return serviceResponse;
            }

            var userName = await GetByUsername(user.Username);
            if (userName is not null)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "Username taken";
                return serviceResponse;
            }

            user.PasswordHash = _passwordHasher.HashPassword(password);
            serviceResponse.Data = user.Id;

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return serviceResponse;
        }

        public async Task<ServiceResponse<AuthenticatedUserResponse>> Login(LoginRequestDto loginRequestDto)
        {
            var serviceResponse = new ServiceResponse<AuthenticatedUserResponse>();

            var userExists = await GetByUsername(loginRequestDto.Username);

            if (userExists is null)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "User Not Found";

                return serviceResponse;
            }

            var isCorrectPassword = _passwordHasher.VerifyPassword(loginRequestDto.Password, userExists.PasswordHash);

            if (isCorrectPassword is false)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "Incorrect Password";

                return serviceResponse;
            }

            else
            {
                serviceResponse.Data = await _authenticator.Authenticate(userExists);
            }
            // this returns a jwt access token. Not a user.
            return serviceResponse;
        }



        public async Task<ServiceResponse<List<ShowUsersDto>>> GetAllUsers()
        {
            var serviceResponse = new ServiceResponse<List<ShowUsersDto>>();

            var allUsers = _context.Users;

            if (allUsers is null)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "No Users Found";

                return serviceResponse;
            }

            var allUsersDto = await allUsers.Select(u => new ShowUsersDto
            {
                Email = u.Email,
                Username = u.Username,
                BlogPosts = u.BlogPosts,
                Role = u.Role
            }).ToListAsync();

            serviceResponse.Data = allUsersDto;

            return serviceResponse;
        }

        public async Task<User> GetByEmail(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email.ToLower().Equals(email.ToLower()));

            return user;
        }

        public async Task<User> GetByUsername(string username)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Username.ToLower().Equals(username.ToLower()));

            return user;
        }

        public async Task<User> GetUserById(int userId)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }
    }
}