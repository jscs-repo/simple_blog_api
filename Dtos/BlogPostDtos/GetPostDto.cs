using System;

namespace dotnet_blog_api.Dtos.BlogPostDtos
{
    public class GetPostDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Created { get; set; }
        public string Updated { get; set; }
        public string Username { get; set; }
    }
}