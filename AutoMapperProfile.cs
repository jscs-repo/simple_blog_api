using AutoMapper;
using dotnet_blog_api.Dtos;
using dotnet_blog_api.Models;

namespace dotnet_blog_api
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<BlogPost, GetPostDto>()
                .ForMember(dto => dto.Username, p => p.MapFrom(p => p.User.Username));

            CreateMap<AddPostDto, BlogPost>();
        }
    }
}