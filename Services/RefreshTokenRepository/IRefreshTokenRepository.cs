using System.Threading.Tasks;
using dotnet_blog_api.Dtos.TokenDtos;
using dotnet_blog_api.Models;

namespace dotnet_blog_api.Services.RefreshTokenRepository
{
    public interface IRefreshTokenRepository
    {
        Task Create(RefreshTokenDto refreshTokenDto);
        Task<RefreshToken> GetByToken(string token);
        Task DeleteById(int id);

        Task DeleteAll(int? userId);
    }
}