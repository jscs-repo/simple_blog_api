using System.Collections.Generic;
using System.Threading.Tasks;
using dotnet_blog_api.Dtos.BlogPostDtos;
using dotnet_blog_api.Models;

namespace dotnet_blog_api.Services
{
    public interface IBlogPostService
    {
        Task<ServiceResponse<List<GetPostDto>>> GetAllPosts();

        Task<ServiceResponse<List<GetPostDto>>> GetUserPosts();
        Task<ServiceResponse<GetPostDto>> GetPostById(int id);
        Task DeletePostById(int id);
        Task<BlogPost> AddPost(AddPostDto addPostDto);
        Task UpdatePost(int id, UpdatePostDto updatePostDto);
    }
}