using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Takas.WebApi.Dto;
using Takas.WebApi.Models;

namespace Takas.WebApi.Services.Interfaces
{
    public interface IUserService
    {
        string GetCurrentUser();
        Task<UserResponse> GetUserById(string userId);
        Task<UserManagerResponse> ForgetPasswordAsync(string email);
        Task<UserManagerResponse> ResetPasswordAsync(ResetPasswordViewModel model);
    }
}
