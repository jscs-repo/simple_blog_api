using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_blog_api.Models
{
    public class User
    {
        // public User()
        // {
        //     Role = "User";
        // }

        public int Id { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public List<BlogPost> BlogPosts { get; set; }
        public string Role { get; set; } = "User";


    }
}