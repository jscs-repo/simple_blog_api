using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_blog_api.Dtos
{
    public class TokenDto
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}