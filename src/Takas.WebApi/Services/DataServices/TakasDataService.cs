using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Takas.Core.Model.Entities;
using Takas.Core.Services.Interfaces;
using Takas.WebApi.Dto;
using Takas.WebApi.Models;
using Takas.WebApi.Services.Interfaces;

namespace Takas.WebApi.Services.DataServices
{
    public class TakasDataService : ITakasDataServices
    {
        private readonly IProductService _productService;
        private readonly IUserService _userService;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IProductImageService _productImageService;
        private readonly ICategoryService _categoryService;
        private readonly IChatService _chatService;
        public TakasDataService(
            IProductService productService,
            IUserService userService,
            IMapper mapper,
            UserManager<User> userManager,
            IProductImageService productImageService,
            ICategoryService categoryService,
            IChatService chatService
            )
        {
            _productService = productService;
            _userService = userService;
            _mapper = mapper;
            _userManager = userManager;
            _productImageService = productImageService;
            _categoryService = categoryService;
            _chatService = chatService;
        }
        public async Task<ProductResponse> AddProduct(AddProduct product)
        {
            try
            {
                var category = _categoryService.Find(x => x.Name == product.Category);
                var entity = new Product
                {
                    Price = product.Price,
                    Description = product.Description,
                    Title = product.Title,
                    Category = category,
                    OwnerId = _userService.GetCurrentUser(),
                };
                await _productService.AddAsync(entity);
                _productService.Save();
                foreach(var image in product.Images)
                {
                    await _productImageService.AddAsync(new ProductImage
                    {
                        ImageSource = image,
                        Product = entity
                    });
                }
                _productImageService.Save();
                return await GetProductById(entity.Id);

            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> DeleteProduct(int id)
        {
            try
            {
                var currentUserId = _userService.GetCurrentUser();
                var entity = await _productService.GetAsync(id);
                if (entity.OwnerId != currentUserId || entity == null)
                    return false;
                _productService.DeleteAsync(entity);
                _productService.Save();
                return true;

            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public async Task<List<ProductResponse>> GetAllProducts()
        {
            var entities = await _productService.GetAll().Include(x=>x.Images).Include(x=>x.Owner).Include(x=>x.Category).ToListAsync();
            var mappedModel = _mapper.Map<List<ProductResponse>>(entities);
            return mappedModel;

        }

        public async Task<UserResponse> GetCurrentUser()
        {
            try
            {
                var currentUserId = _userService.GetCurrentUser();
                var appUser = await _userManager.FindByIdAsync(currentUserId);
                var userToReturn = _mapper.Map<UserResponse>(appUser);
                return userToReturn;
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public async Task<ProductResponse> UpdateProduct(int id, AddProduct product)
        {
            try
            {
                var category = _categoryService.Find(x => x.Name == product.Category);
                var currentUserId = _userService.GetCurrentUser();
                var entity = await _productService.GetAll().Include(x=>x.Images).Where(x=>x.Id == id).SingleOrDefaultAsync();
                if (entity.OwnerId != currentUserId || entity == null)
                    return null;
                entity.Description = product.Description;
                entity.Price = product.Price;
                entity.Category = category;
                entity.Title = product.Title;
                if(product.Images.Count() > 0)
                {
                    var ss = product.Images.Select(x => new ProductImage { ImageSource = x, ProductId = entity.Id });
                    entity.Images.AddRange(ss);
                }
                await _productService.UpdateAsync(entity, entity.Id);
                _productService.Save();
                return await GetProductById(entity.Id);
            }
            catch(Exception ex)
            {
                return null;
            }
        }
        public async Task<ProductResponse> GetProductById(int id)
        {
            try
            {
                var entities = await _productService.GetAll().Include(x => x.Images).Include(x => x.Owner).Include(x => x.Category).Where(x=>x.Id==id).FirstOrDefaultAsync();
                var mappedModel = _mapper.Map<ProductResponse>(entities);
                return mappedModel;
            }
            catch(Exception ex)
            {
                return null;
            }
        }


        public async Task<bool> DeleteImage(string imgPath)
        {
            try
            {
                var img = await _productImageService.FindAsync(x=>x.ImageSource == imgPath);
                if (img != null)
                {
                    _productImageService.DeleteAsync(img);
                    _productImageService.Save();
                    return true;
                }
                return false;


            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<List<ProductResponse>> GetAllProductsByOwnerId()
        {
            var currentUserId = _userService.GetCurrentUser();
            var allProduct = await GetAllProducts();
            var specifiedProducts = allProduct.Where(x => x.OwnerId == currentUserId);
            var mappedData = _mapper.Map<List<ProductResponse>>(specifiedProducts);
            return mappedData;
        }

        public async Task<UserResponse> UpdateUser(string id, UpdateUserRequest userToUpdate)
        {
            var currentUserId = _userService.GetCurrentUser();
            var appUser = await _userManager.FindByIdAsync(currentUserId);
            appUser.DisplayName = userToUpdate.DisplayName;
            appUser.PhotoUrl = userToUpdate.PhotoUrl;
            var updateProcess = await _userManager.UpdateAsync(appUser);
            if (updateProcess.Succeeded)
            {
                return _mapper.Map<UserResponse>(appUser);
            }

            return null;
        }

        public async Task<List<ChatResponseModel>> GetChatsByUser()
        {
            try
            {
                var currentUserId = _userService.GetCurrentUser();
                var usersAllChats = await _chatService.GetAll().Include(x=>x.Messages).Where(x => x.FromId == currentUserId || x.ToId == currentUserId).ToListAsync();
                var _mappedData = _mapper.Map<List<ChatResponseModel>>(usersAllChats);
                return _mappedData;
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public async Task<ChatResponseModel> GetSingleChat(int targetProductId)
        {
            try
            {
                var currentUserId = _userService.GetCurrentUser();
                var usersAllChats = await _chatService.GetAll().Include(x => x.Messages).Where(x => (x.FromId == currentUserId || x.ToId==currentUserId) && x.TargetProductId == targetProductId).FirstOrDefaultAsync();
                var _mappedData = _mapper.Map<ChatResponseModel>(usersAllChats);
                return _mappedData;
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public async Task<ProductResponse> GetProduct(int id)
        {
            try
            {
                var entities = await _productService.GetAll().Include(x => x.Images).Include(x => x.Owner).Include(x => x.Category).Where(x=>x.Id==id).FirstOrDefaultAsync();
                var mappedModel = _mapper.Map<ProductResponse>(entities);
                return mappedModel;
            }
            catch(Exception ex)
            {
                return null;
            }
        }
    }
}
