using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Takas.WebApi.Dto;

namespace Takas.WebApi.Services.Interfaces
{
    public interface IUserService
    {
        string GetCurrentUser();
        Task<UserResponse> GetUserById(string userId);
    }
}
