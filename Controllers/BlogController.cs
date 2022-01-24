using System.Collections.Generic;
using System.Threading.Tasks;
using dotnet_blog_api.Dtos.BlogPostDtos;
using dotnet_blog_api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_blog_api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class BlogController : ControllerBase
    {
        private readonly IBlogPostService _blogPostService;

        public BlogController(IBlogPostService blogPostService)
        {
            _blogPostService = blogPostService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllPostsAsync()
        {
            // need to change this so only adminstrators can see all posts
            ServiceResponse<List<GetPostDto>> response = await _blogPostService.GetAllPosts();

            if (response.Data is null)
            {
                response.Success = false;
                response.Message = "No Posts Found";
                return NotFound(response);
            }

            return Ok(response);
        }


        [HttpGet("userblog")]
        public async Task<IActionResult> GetUsersPostsAsync()
        {
            ServiceResponse<List<GetPostDto>> response = await _blogPostService.GetUserPosts();

            if (response.Data is null)
            {
                response.Success = false;
                response.Message = "No Posts Found";

                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpGet("{id}", Name = "GetPostByIdAsync")]
        public async Task<IActionResult> GetPostByIdAsync(int id)
        {
            ServiceResponse<GetPostDto> response = await _blogPostService.GetPostById(id);

            if (response.Data is null)
            {
                response.Success = false;
                response.Message = "Post not found";
                return NotFound(response);
            }

            return Ok(response);
        }


        [HttpPost]
        public async Task<IActionResult> AddPostAsync(AddPostDto addPostDto)
        {
            var newPost = await _blogPostService.AddPost(addPostDto);

            return CreatedAtRoute(nameof(GetPostByIdAsync), new { id = newPost.Id }, newPost.AsDto());
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePostAsync(int id)
        {
            ServiceResponse<GetPostDto> response = await _blogPostService.GetPostById(id);

            if (response.Data is null)
            {
                response.Success = false;
                response.Message = "Post not found";
                return NotFound(response);
            }

            await _blogPostService.DeletePostById(id);

            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePostAsync(int id, UpdatePostDto updatedPostDto)
        {
            ServiceResponse<GetPostDto> response = await _blogPostService.GetPostById(id);

            if (response.Data is null)
            {
                response.Success = false;
                response.Message = "Post not found";
                return NotFound(response);
            }

            await _blogPostService.UpdatePost(id, updatedPostDto);

            return NoContent();
        }
    }
}