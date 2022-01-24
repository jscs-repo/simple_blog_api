using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnet_blog_api.Models;

namespace dotnet_blog_api.Dtos
{
    public class ShowUsersDto
    {
        public string Email { get; set; }
        public string Username { get; set; }
        public List<BlogPost> BlogPosts { get; set; }
        public string Role { get; set; }
    }
}