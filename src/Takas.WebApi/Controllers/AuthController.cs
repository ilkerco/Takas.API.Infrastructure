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

        public AuthController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IAuthHelper authHelper,
            IMapper mapper,
            ILoginService loginService
            )
        {
            _authHelper = authHelper;
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _loginService = loginService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto userRegisterDto)
        {
            try
            {
                var userToCreate = _mapper.Map<User>(userRegisterDto);
                var result = await _userManager.CreateAsync(userToCreate, userRegisterDto.Password);
                if (result.Succeeded)
                {
                    var user =  _userManager.FindByNameAsync(userToCreate.UserName).Result;
                    return Ok(user);
                }
                return BadRequest();
            }
            catch(Exception ex)
            {
                return BadRequest();
            }
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto userLoginDto)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(userLoginDto.UserName);
                if (user == null)
                {
                    return BadRequest("There is no user with name " + userLoginDto.UserName);
                }
                var loginResult = await _signInManager.CheckPasswordSignInAsync(user, userLoginDto.Password, false);
                if (!loginResult.Succeeded)
                {
                    return BadRequest("Wrong Password");
                }
                var appUser = await _userManager.Users.FirstOrDefaultAsync(
                    u => u.NormalizedUserName == userLoginDto.UserName.ToUpper(CultureInfo.InvariantCulture));
                var userToReturn = _mapper.Map<UserDto>(appUser);
                //await _signInManager.SignInAsync(appUser, true);
                return Ok(new
                {
                    token = _authHelper.GenerateJwtToken(appUser).Result,
                    user =userToReturn,
                });
            }
            catch (Exception ex)
            {
                return BadRequest();
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
    }
}
