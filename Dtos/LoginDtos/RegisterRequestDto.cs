using System.ComponentModel.DataAnnotations;

namespace dotnet_blog_api.Dtos.LoginDtos
{
    public class RegisterRequestDto
    {
        [Required(ErrorMessage = "Please enter email address")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter username")]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string ConfirmPassword { get; set; }

    }
}