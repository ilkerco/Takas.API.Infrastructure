using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Takas.Core.Model.Entities;
using Takas.WebApi.Models;
using Takas.WebApi.Services.Interfaces;
using Takas.WebApi.Services.Interfaces.External;

namespace Takas.WebApi.Services.DataServices.External
{
    public class LoginService : ILoginService
    {
        private readonly UserManager<User> _userManager;
        private readonly IFacebookAuthService _facebookAuthService;
        private readonly IGoogleAuthService _googleAuthService;
        private readonly IAuthHelper _authHelper;

        public LoginService(
            UserManager<User> userManager,
            IFacebookAuthService facebookAuthService,
            IGoogleAuthService googleAuthService,
            IAuthHelper authHelper
            )
        {
            _userManager = userManager;
            _facebookAuthService = facebookAuthService;
            _googleAuthService = googleAuthService;
            _authHelper = authHelper;
        }
        public async Task<AuthenticationResult> LoginWithFacebookAsync(SocialLoginRequest request)
        {
            var userInfofb = await _facebookAuthService.GetUserInfoAsync(request.accessToken);
            var user = await _userManager.FindByEmailAsync(userInfofb.Email);
            if (user == null)
            {
                var appUser = new User
                {
                    Id = Guid.NewGuid().ToString(),
                    Email = userInfofb.Email,
                    UserName = userInfofb.Id,
                    Boost = 1,
                    Coin = 200,
                    Country = request.Country,
                    DisplayName = userInfofb.Name,
                    Latitude = request.Latitude,
                    Longitude = request.Longitude,
                    PhotoUrl = userInfofb.Picture.Data.Url.ToString(),
                    SubAdminArea = request.SubAdminArea

                };
                var createdResult = await _userManager.CreateAsync(appUser);
                if (!createdResult.Succeeded)
                {
                    return new AuthenticationResult
                    {
                        Errors = new[] { "Somethings went wrong" }
                    };
                }
                return new AuthenticationResult
                {
                    Success = true,
                    Token = _authHelper.GenerateJwtToken(appUser).Result
                };
                //return GenerateAuthenticationResultForUser(applicationUser);
            }

            return new AuthenticationResult
            {
                Success = true,
                Token = _authHelper.GenerateJwtToken(user).Result
            };
        }

        public async Task<AuthenticationResult> LoginWithGoogleAsync(SocialLoginRequest request)
        {
            var userInfoGoogle = await _googleAuthService.GetUserInfoAsync(request.accessToken);
            var user = await _userManager.FindByEmailAsync(userInfoGoogle.Email);
            if (user == null)
            {
                var applicationUser = new User
                {
                    Id = Guid.NewGuid().ToString(),
                    Email = userInfoGoogle.Email,
                    UserName = userInfoGoogle.Id,
                    Boost = 1,
                    Coin = 200,
                    Country = request.Country,
                    DisplayName = userInfoGoogle.Name,
                    Latitude = request.Latitude,
                    Longitude = request.Longitude,
                    PhotoUrl = userInfoGoogle.Picture.ToString(),
                    SubAdminArea = request.SubAdminArea

                };
                

                var createdResult = await _userManager.CreateAsync(applicationUser);
                if (!createdResult.Succeeded)
                {
                    return new AuthenticationResult
                    {
                        Errors = new[] { "Somethings went wrong" }
                    };
                }
                return new AuthenticationResult
                {
                    Success = true,
                    Token = _authHelper.GenerateJwtToken(applicationUser).Result
                };
            }

            return new AuthenticationResult
            {
                Success = true,
                Token = _authHelper.GenerateJwtToken(user).Result
            }; 
        }
        /*private AuthenticationResult GenerateAuthenticationResultForUser(ApplicationUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes("");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim("id", user.Id)
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return new AuthenticationResult
            {
                Success = true,
                Token = tokenHandler.WriteToken(token)
            };
        }*/
    }
}
