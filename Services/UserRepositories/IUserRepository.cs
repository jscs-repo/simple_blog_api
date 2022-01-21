using System.Collections.Generic;
using System.Threading.Tasks;
using dotnet_blog_api.Dtos;
using dotnet_blog_api.Models;

namespace dotnet_blog_api.Services.UserRepositories
{
    public interface IUserRepository
    {
        Task<User> GetByEmail(string email);
        Task<User> GetByUsername(string username);
        Task<User> GetUserById(int userId);

        Task<ServiceResponse<int>> CreateUser(User user, string password);

        Task<ServiceResponse<List<ShowUsersDto>>> GetAllUsers();

        Task<ServiceResponse<AuthenticatedUserResponse>> Login(LoginRequestDto loginRequestDto);

    }
}