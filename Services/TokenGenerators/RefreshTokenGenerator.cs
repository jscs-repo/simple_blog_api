using dotnet_blog_api.Dtos;
using Microsoft.Extensions.Configuration;

namespace dotnet_blog_api.Services.TokenGenerators
{
    public class RefreshTokenGenerator
    {
        private IConfiguration _configuration;
        private readonly TokenGenerator _tokenGenerator;

        public RefreshTokenGenerator(IConfiguration configuration, TokenGenerator tokenGenerator)
        {
            _configuration = configuration;
            _tokenGenerator = tokenGenerator;
        }

        public string GenerateToken()
        {
            var tokenConfig = _configuration.GetSection("AppSettings").Get<TokenDto>();
            var _refreshTokenValue = tokenConfig.RefreshToken;

            return _tokenGenerator.GenerateToken(
                // _configuration.GetSection("AppSettings:RefreshToken").Value,
                _refreshTokenValue,
                "https://localhost:5001",
                "https://localhost:5001",
                131400
            );
        }
    }
}