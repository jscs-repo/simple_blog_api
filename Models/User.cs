using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_blog_api.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public List<BlogPost> BlogPosts { get; set; }
        public string Role { get; set; } = "User";


    }
}