using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Takas.Core.Model.Entities;
using Takas.WebApi.Dto;
using Takas.WebApi.Models;
using Takas.WebApi.Services.Interfaces;

namespace Takas.WebApi.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    public class AuthController:Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IAuthHelper _authHelper;
        private readonly IMapper _mapper;
        private readonly ILoginService _loginService;
        private readonly IUserService _userService;

        public AuthController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IAuthHelper authHelper,
            IMapper mapper,
            ILoginService loginService,
            IUserService userService
            )
        {
            _authHelper = authHelper;
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _loginService = loginService;
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] LoginWithEmailRequest request)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(request.Email);
                if(user == null)
                {
                    var appUser = new User
                    {
                        Id = Guid.NewGuid().ToString(),
                        Email = request.Email,
                        UserName = request.Email.Split('@')[0],
                        Boost = 1,
                        Latitude = 39.9333,
                        Longitude = 32.8597,
                        PhotoUrl = "https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png",
                        SubAdminArea = "Ankara",
                        Country = "Türkiye",
                        
                        Coin = 200,
                        DisplayName = request.Email.Split('@')[0],

                    };
                    var createdResult = await _userManager.CreateAsync(appUser,request.Password);
                    if (!createdResult.Succeeded)
                    {
                        return BadRequest(createdResult.Errors.First().Code);
                    }
                    return Ok(_authHelper.GenerateJwtToken(appUser).Result);
                }
                else
                {
                    return BadRequest("Bu e-posta zaten kayıtlı. Şifrenizi hatırlamıyorsanız şifremi unuttum kısmına tıklayın.");
                }
                
            }
            catch(Exception ex)
            {
                return BadRequest("Kayıt olurken bilinmeyen hata.");
            }
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginWithEmailRequest request)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(request.Email);
                if (user == null)
                {
                    return BadRequest("E-posta adresiniz ve/veya şifreniz hatalı.");
                }
                var loginResult = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
                if (!loginResult.Succeeded)
                {
                    return BadRequest("E-posta adresiniz ve/veya şifreniz hatalı.");
                }
                
                var userToReturn = _mapper.Map<UserDto>(user);
                //await _signInManager.SignInAsync(user, true);
                return Ok(_authHelper.GenerateJwtToken(user).Result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("google")]
        public async Task<IActionResult> GoogleLogin([FromBody] SocialLoginRequest request)
        {
            var authResponse = await _loginService.LoginWithGoogleAsync(request);
            if (!authResponse.Success)
            {
                return BadRequest(authResponse.Errors);
            }


            return Ok(authResponse.Token);
        }
        [HttpPost("facebook")]
        public async Task<IActionResult> FaceebookLogin([FromBody] SocialLoginRequest request)
        {
            var authResponse = await _loginService.LoginWithFacebookAsync(request);
            if (!authResponse.Success)
            {
                return BadRequest(authResponse.Errors);
            }


            return Ok(authResponse.Token);
        }

        [HttpPost("ForgetPassword")]
        public async Task<IActionResult> ForgetPassword(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return NotFound();
            }
            var result = await _userService.ForgetPasswordAsync(email);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpPost("resetpassword")]
        public async Task<IActionResult> ResetPassword([FromForm]ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.ResetPasswordAsync(model);
                if (result.IsSuccess)
                {
                    return Ok(result);
                }
                return BadRequest(result);
            }
            return BadRequest("Yanlış parametreler.");
        }

    }
}
