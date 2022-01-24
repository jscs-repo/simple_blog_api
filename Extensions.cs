using dotnet_blog_api.Dtos.BlogPostDtos;
using dotnet_blog_api.Models;

namespace dotnet_blog_api
{
    public static class Extensions
    {
        public static BlogPostDto AsDto(this BlogPost blogPost)
        {
            return new BlogPostDto
            {
                Id = blogPost.Id,
                Title = blogPost.Title,
                Content = blogPost.Content
            };
        }
    }
}