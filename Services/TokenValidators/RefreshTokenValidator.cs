using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace dotnet_blog_api.Services.TokenValidators
{
    public class RefreshTokenValidator
    {
        private readonly IConfiguration _configuration;

        public RefreshTokenValidator(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public bool Validate(string refreshToken)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            TokenValidationParameters validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:RefreshToken").Value)),
                ValidAudience = "https://localhost:5001",
                ValidIssuer = "https://localhost:5001",
                ValidateAudience = true,
                ValidateIssuer = true,
                ClockSkew = TimeSpan.Zero
            };

            try
            {
                tokenHandler.ValidateToken(refreshToken, validationParameters, out SecurityToken validatedToken);
                return true;

            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}