using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_blog_api.Dtos
{
    public class RefreshTokenDto
    {
        public string Token { get; set; }
        public int UserId { get; set; }
    }
}