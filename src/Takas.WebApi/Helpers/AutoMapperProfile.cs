using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Takas.Core.Model.Entities;
using Takas.WebApi.Dto;

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
        }
    }
}
