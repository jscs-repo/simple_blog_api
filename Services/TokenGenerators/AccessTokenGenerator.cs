using System;
using System.Collections.Generic;
using System.Security.Claims;
using dotnet_blog_api.Dtos;
using dotnet_blog_api.Models;
using Microsoft.Extensions.Configuration;

namespace dotnet_blog_api.Services.TokenGenerators
{
    public class AccessTokenGenerator
    {
        private readonly IConfiguration _configuration;
        private readonly TokenGenerator _tokenGenerator;

        public AccessTokenGenerator(IConfiguration configuration, TokenGenerator tokenGenerator)
        {
            _configuration = configuration;
            _tokenGenerator = tokenGenerator;
        }

        public string GenerateToken(User user)
        {
            var tokenConfig = _configuration.GetSection("AppSettings").Get<TokenDto>();
            var _tokenValue = tokenConfig.Token;

            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            };


            return _tokenGenerator.GenerateToken(
                // _configuration.GetSection("AppSettings:Token").Value,
                _tokenValue,
                "https://localhost:5001",
                "https://localhost:5001",
                50,
                claims
            );
        }
    }
}