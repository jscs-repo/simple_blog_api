using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using dotnet_blog_api.Data;
using dotnet_blog_api.Dtos.BlogPostDtos;
using dotnet_blog_api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace dotnet_blog_api.Services.BlogPostRepositories
{

    public class BlogPostService : IBlogPostService
    {
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public BlogPostService(DataContext context, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

        public async Task<ServiceResponse<List<GetPostDto>>> GetAllPosts()
        {
            ServiceResponse<List<GetPostDto>> serviceResponse = new ServiceResponse<List<GetPostDto>>();

            var allPosts = await _context.BlogPosts.Include(p => p.User).ToListAsync();

            serviceResponse.Data = (allPosts.Select(p => _mapper.Map<GetPostDto>(p))).ToList();

            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetPostDto>>> GetUserPosts()
        {
            ServiceResponse<List<GetPostDto>> serviceResponse = new ServiceResponse<List<GetPostDto>>();

            var userPosts = await _context.BlogPosts.Where(p => p.UserId == GetUserId()).Include(pp => pp.User).ToListAsync();

            serviceResponse.Data = (userPosts.Select(p => _mapper.Map<GetPostDto>(p))).ToList();

            return serviceResponse;
        }

        public async Task<ServiceResponse<GetPostDto>> GetPostById(int id)
        {
            ServiceResponse<GetPostDto> serviceResponse = new ServiceResponse<GetPostDto>();

            // var post = await _context.BlogPosts.Include(u => u.User).FirstOrDefaultAsync(
            //     x => x.Id == id && x.UserId == GetUserId());

            var post = await _context.BlogPosts.Include(x => x.User).FirstOrDefaultAsync(
                x => x.Id == id);

            serviceResponse.Data = _mapper.Map<GetPostDto>(post);

            return serviceResponse;
        }

        public async Task<BlogPost> AddPost(AddPostDto addPostDto)
        {
            // ServiceResponse<int> serviceResponse = new ServiceResponse<int>();
            var newPost = _mapper.Map<BlogPost>(addPostDto);

            newPost.UserId = GetUserId();
            newPost.User = await _context.Users.FirstOrDefaultAsync(u => u.Id == GetUserId());

            await _context.BlogPosts.AddAsync(newPost);
            await _context.SaveChangesAsync();

            return newPost;

        }

        public async Task DeletePostById(int id)
        {
            var postToDelete = await _context.BlogPosts.FirstOrDefaultAsync(
                x => x.Id == id && x.UserId == GetUserId());

            _context.BlogPosts.Remove(postToDelete);

            await _context.SaveChangesAsync();

        }

        public async Task UpdatePost(int id, UpdatePostDto updatePostDto)
        {
            var post = await _context.BlogPosts
                .FirstOrDefaultAsync(p => p.Id == id && p.UserId == GetUserId());

            post.Title = updatePostDto.Title;
            post.Content = updatePostDto.Content;

            _context.BlogPosts.Update(post);

            await _context.SaveChangesAsync();

        }
    }
}