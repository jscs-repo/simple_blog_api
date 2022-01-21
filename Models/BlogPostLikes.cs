using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_blog_api.Models
{
    public class BlogPostLikes
    {
        public int Id { get; set; }
        public int NumberOfLikes { get; set; }
        public int BlogPostId { get; set; }
        // public BlogPost BlogPost { get; set; }
    }
}