using System.ComponentModel.DataAnnotations;

namespace dotnet_blog_api.Dtos.BlogPostDtos
{
    public class AddPostDto
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }
    }
}