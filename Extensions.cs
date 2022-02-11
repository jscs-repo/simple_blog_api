using dotnet_blog_api.Dtos.BlogPostDtos;
using dotnet_blog_api.Models;

namespace dotnet_blog_api
{
    public static class Extensions
    {
        public static GetPostDto AsDto(this BlogPost blogPost)
        {
            return new GetPostDto
            {
                Id = blogPost.Id,
                Title = blogPost.Title,
                Content = blogPost.Content,
                Created = blogPost.Created,
                Updated = blogPost.Updated,
                Username = blogPost.User.Username
            };
        }
    }
}