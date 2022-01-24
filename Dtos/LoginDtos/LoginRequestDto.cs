using System.ComponentModel.DataAnnotations;

namespace dotnet_blog_api.Dtos.LoginDtos
{
    public class LoginRequestDto
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}