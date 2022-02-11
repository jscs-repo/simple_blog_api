using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_blog_api.Models
{
    public class BlogPost
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Created { get; set; } = DateTime.Now.ToString("D");
        public string Updated { get; set; } = DateTime.Now.ToString("D");
        public bool Public { get; set; } = false;
        // public List<BlogPostLikes> Likes { get; set; }

        public User User { get; set; }
        public int UserId { get; set; }
    }
}