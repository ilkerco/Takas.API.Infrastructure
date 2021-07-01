using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Takas.WebApi.Models;

namespace Takas.WebApi.Services.Interfaces.External
{
    public interface IFacebookAuthService
    {
        Task<FacebookUserInfoResult> GetUserInfoAsync(string accessToken);
    }
}
