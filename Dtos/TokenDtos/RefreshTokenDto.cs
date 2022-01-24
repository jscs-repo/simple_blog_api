namespace dotnet_blog_api.Dtos.TokenDtos
{
    public class RefreshTokenDto
    {
        public string Token { get; set; }
        public int UserId { get; set; }
    }
}