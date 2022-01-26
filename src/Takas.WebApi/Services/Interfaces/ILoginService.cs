using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Takas.WebApi.Models;

namespace Takas.WebApi.Services.Interfaces
{
    public interface ILoginService
    {
        Task<AuthenticationResult> LoginWithEmailAndPassword(LoginWithEmailRequest request);
        Task<AuthenticationResult> LoginWithFacebookAsync(SocialLoginRequest request);
        Task<AuthenticationResult> LoginWithGoogleAsync(SocialLoginRequest request);
    }
}
