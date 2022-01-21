using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_blog_api.Dtos
{
    public class AddPostDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
    }
}