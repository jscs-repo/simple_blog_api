using System.Threading.Tasks;
using dotnet_blog_api.Dtos;
using dotnet_blog_api.Models;
using dotnet_blog_api.Services.RefreshTokenRepository;
using dotnet_blog_api.Services.TokenGenerators;

namespace dotnet_blog_api.Services.Authenticators
{
    public class Authenticator
    {
        private readonly AccessTokenGenerator _accessTokenGenerator;
        private readonly RefreshTokenGenerator _refreshTokenGenerator;
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public Authenticator(AccessTokenGenerator accessTokenGenerator, RefreshTokenGenerator refreshTokenGenerator, IRefreshTokenRepository refreshTokenRepository)
        {
            _accessTokenGenerator = accessTokenGenerator;
            _refreshTokenGenerator = refreshTokenGenerator;
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task<AuthenticatedUserResponse> Authenticate(User user)
        {
            var accessToken = _accessTokenGenerator.GenerateToken(user);
            var refreshToken = _refreshTokenGenerator.GenerateToken();

            RefreshTokenDto refreshTokenDto = new RefreshTokenDto
            {
                Token = refreshToken,
                UserId = user.Id
            };

            await _refreshTokenRepository.Create(refreshTokenDto);

            return new AuthenticatedUserResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }
    }
}