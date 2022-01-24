using System.Linq;
using System.Threading.Tasks;
using dotnet_blog_api.Data;
using dotnet_blog_api.Dtos.TokenDtos;
using dotnet_blog_api.Models;
using Microsoft.EntityFrameworkCore;

namespace dotnet_blog_api.Services.RefreshTokenRepository
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly DataContext _context;

        public RefreshTokenRepository(DataContext context)
        {
            _context = context;
        }

        public async Task Create(RefreshTokenDto refreshTokenDto)
        {
            var refreshToken = new RefreshToken
            {
                Token = refreshTokenDto.Token,
                UserId = refreshTokenDto.UserId
            };

            await _context.RefreshTokens.AddAsync(refreshToken);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAll(int? userId)
        {
            var refreshTokens = await _context.RefreshTokens.Where(t => t.UserId == userId).ToListAsync();
            _context.RefreshTokens.RemoveRange(refreshTokens);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteById(int id)
        {
            var tokenToDelete = await _context.RefreshTokens.FindAsync(id);
            _context.RefreshTokens.Remove(tokenToDelete);
            await _context.SaveChangesAsync();
        }

        public async Task<RefreshToken> GetByToken(string token)
        {
            RefreshToken refreshToken = await _context.RefreshTokens.FirstOrDefaultAsync(r => r.Token == token);

            return refreshToken;
        }
    }
}