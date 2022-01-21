using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnet_blog_api.Models;

namespace dotnet_blog_api.Dtos
{
    public class GetPostDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime Updated { get; set; } = DateTime.Now;
        public string Username { get; set; }
    }
}