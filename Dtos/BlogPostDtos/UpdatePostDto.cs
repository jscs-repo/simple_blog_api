using System;

namespace dotnet_blog_api.Dtos.BlogPostDtos
{
    public class UpdatePostDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime Updated { get; set; } = DateTime.Now;
        public bool Public { get; set; } = false;
    }
}