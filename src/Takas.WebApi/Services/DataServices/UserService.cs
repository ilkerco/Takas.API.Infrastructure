using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Takas.Core.Model.Entities;
using Takas.WebApi.Dto;
using Takas.WebApi.Models;
using Takas.WebApi.Services.Interfaces;

namespace Takas.WebApi.Services.DataServices
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IMailService _mailService;
        public UserService(IHttpContextAccessor httpContextAccessor,UserManager<User> userManager,IMapper mapper,IMailService mailService)
        {
            _userManager = userManager;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _mailService = mailService;
        }

        public async Task<UserManagerResponse> ForgetPasswordAsync(string email)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    return new UserManagerResponse
                    {
                        IsSuccess = false,
                        Message = "Bu e-postaya ait bir kullanıcı yok."
                    };
                }
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                var encodedToken = Encoding.UTF8.GetBytes(token);
                var validToken = WebEncoders.Base64UrlEncode(encodedToken);
                //string hostUrl = _httpContextAccessor.HttpContext.Request.Host.Value;
                string url = $"https://www.ilkersargin.site/ResetPassword?email={email}&token={validToken}";
                string htmlString = $"<html><body><h1>Şifrenizi değiştirmek için <a href='{url}'>tıklayın</a></h1></body></html>";
                await _mailService.SendEmailAsync(email, "Reset Password",htmlString);
                return new UserManagerResponse
                {
                    IsSuccess = true,
                    Message = "Şifre sıfırlama bağlantısı e-posta adresinize gönderildi!"
                };
            }
            catch(Exception ex)
            {
                return new UserManagerResponse
                {
                    IsSuccess = false,
                    Message = "Bir şeyler ters gitti."+ex.Message
                };
            }
            
        }

        public string GetCurrentUser()
        {
            return _httpContextAccessor?.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }

        public async Task<UserResponse> GetUserById(string userId)
        {
            var user = await _userManager.Users.Where(x => x.Id == userId).FirstOrDefaultAsync();
            var userToReturn = _mapper.Map<UserResponse>(user);
            return userToReturn;
        }

        public async Task<UserManagerResponse> ResetPasswordAsync(ResetPasswordViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return new UserManagerResponse
                {
                    IsSuccess = false,
                    Message = "Bu e-postaya ait bir kullanıcı yok."
                };
            }
            if (model.NewPassword != model.ConfirmPassword)
            {
                return new UserManagerResponse
                {
                    IsSuccess = false,
                    Message = "Şifreler eşleşmiyor."
                };
            }
            var decodedToken = WebEncoders.Base64UrlDecode(model.Token);
            var normalToken = Encoding.UTF8.GetString(decodedToken);
            var result = await _userManager.ResetPasswordAsync(user, normalToken, model.NewPassword);
            if (result.Succeeded)
            {
                return new UserManagerResponse
                {
                    IsSuccess = true,
                    Message = "Şifre başarıyla değiştirildi."
                };
            }
            return new UserManagerResponse
            {
                IsSuccess = false,
                Message = result.Errors.First().Description
            };
        }
    }
}
