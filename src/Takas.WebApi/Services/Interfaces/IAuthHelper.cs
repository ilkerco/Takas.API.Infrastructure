using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Takas.Core.Model.Entities;

namespace Takas.WebApi.Services.Interfaces
{
    public interface IAuthHelper
    {
        Task<string> GenerateJwtToken(User user);
    }
}
