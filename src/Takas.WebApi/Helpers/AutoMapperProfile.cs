using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Takas.Core.Model.Entities;
using Takas.WebApi.Dto;
using Takas.WebApi.Models;

namespace Takas.WebApi.Helpers
{
    public class AutoMapperProfile:Profile
    {
        public AutoMapperProfile()
        {
            AllowNullCollections = true;
            AllowNullDestinationValues = true;

            CreateMap<UserRegisterDto, User>(MemberList.None);
            CreateMap<User, UserDto>();
            CreateMap<User, UserResponse>();
            CreateMap<Product, ProductResponse>()
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images.Select(x => x.ImageSource)))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt.ToString()))
                .ForMember(dest => dest.Latitude, opt => opt.MapFrom(src => src.Owner.Latitude))
                .ForMember(dest => dest.Longitude, opt => opt.MapFrom(src => src.Owner.Longitude));
            CreateMap<Chat, ChatResponseModel>()
                .ForMember(dest => dest.Messages, opt => opt.MapFrom(src => src.Messages.Select(x=> new {x.Timestamp,x.Text,x.Name })))
                 ;
        }
    }
}
