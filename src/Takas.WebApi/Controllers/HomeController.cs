using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Takas.Core.Model.Entities;
using Takas.WebApi.Dto;
using Takas.WebApi.Models;
using Takas.WebApi.Services.Interfaces;

namespace Takas.WebApi.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    public class HomeController:Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITakasDataServices _takasDataService;
        private readonly IUserService _userService;
        public HomeController(
            IHttpContextAccessor httpContextAccessor,
            ITakasDataServices takasDataService,
            IUserService userService,
            IHostingEnvironment hostingEnvironment
            )
        {
            _hostingEnvironment = hostingEnvironment;
            _userService = userService;
            _takasDataService = takasDataService;
            _httpContextAccessor = httpContextAccessor;
        }
        [HttpGet("isAlive")]
        public IActionResult IsAlive()
        {
            var currentUserId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return Ok();
        }
        [HttpGet("getCurrentUser")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var data = await _takasDataService.GetCurrentUser();
            return Ok(data);
        }
        [HttpGet("getUserById/{userId}")]
        public async Task<IActionResult> GetUserById([FromRoute] string userId)
        {
            var user = await _userService.GetUserById(userId);
            
            return Ok(user);
        }
        [HttpGet("getAllProducts")]
        public async Task<IActionResult> GetAllProducts()
        {
            try
            {
                var data = await _takasDataService.GetAllProducts();
                return Ok(data);
            }
            catch(Exception ex)
            {
                return NoContent();
            }
        }
        [HttpGet("getCurrenUsersProducts")]
        public async Task<IActionResult> GetCurrenUsersProducts()
        {
            try
            {
                var data = await _takasDataService.GetAllProductsByOwnerId();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return NoContent();
            }
        }
        [HttpPost("addProduct")]
        public async Task<IActionResult> AddProduct([FromBody]AddProduct product)
        {
            try
            {
                var data = await _takasDataService.AddProduct(product);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return NoContent();
            }
        }
        [HttpPut("updateProduct/{id}")]
        public async Task<IActionResult> UpdateProduct(int id,[FromBody] AddProduct product)
        {
            try
            {
                var data = await _takasDataService.UpdateProduct(id,product);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return NoContent();
            }
        }
        [HttpPut("updateUser/{id}")]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] UpdateUserRequest userToUpdate)
        {
            try
            {
                var data = await _takasDataService.UpdateUser(id, userToUpdate);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return NoContent();
            }
        }
        [HttpDelete("deleteProduct/{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                var data = await _takasDataService.DeleteProduct(id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return NoContent();
            }
        }
        [HttpDelete("deleteSingleImage")]
        public async Task<IActionResult> DeleteSingleImage([FromQuery] string imageName)
        {
            if (await _takasDataService.DeleteImage(imageName))
            {
                if ((System.IO.File.Exists(imageName)))
                {
                    System.IO.File.Delete(imageName);
                }

                return Ok();
            }
            return BadRequest("Something went wrong!");
        }
        [HttpPost]
        [Route("uploadImage")]
        public async Task<IActionResult> ImageUpload(List<IFormFile> files)
        {
            string imgUrl = "https://ilkersargin.xyz/images/";
            List<string> urlList = new List<string>();
            if (files == null)
            {
                return BadRequest();
            }
            try
            {
                foreach (var file in files)
                {
                    string fName = file.FileName;
                    string path = Path.Combine(_hostingEnvironment.ContentRootPath, "wwwroot\\images\\" + file.FileName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                        //imgUrl += file.FileName;
                        urlList.Add(imgUrl + file.FileName);
                    }

                }
                var jsonImages = new JsonImages() { images = urlList };
                return Ok(jsonImages);

            }
            catch (Exception e)
            {
                return StatusCode(500, $"Internal server error : {e.Message}");
            }

        }
    }
}
