using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Takas.Core.Model.Entities;
using Takas.WebApi.Dto;
using Takas.WebApi.Models;

namespace Takas.WebApi.Services.Interfaces
{
    public interface ITakasDataServices
    {
        Task<List<ChatResponseModel>> GetChatsByUser();
        Task<ChatResponseModel> GetSingleChat(int targetProductId);
        Task<ProductResponse> AddProduct(AddProduct product);
        Task<ProductResponse> UpdateProduct(int id,AddProduct product);
        Task<List<ProductResponse>> GetAllProductsByOwnerId();
        Task<ProductResponse> GetProduct(int id);
        Task<bool> DeleteProduct(int id);
        Task<List<ProductResponse>> GetAllProducts();
        Task<UserResponse> GetCurrentUser();
        Task<UserResponse> UpdateUser(string id, UpdateUserRequest userToUpdate);
        Task<bool> DeleteImage(string imgPath);
    }
}
