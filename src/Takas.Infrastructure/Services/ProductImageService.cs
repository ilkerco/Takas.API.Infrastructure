using Takas.Core.Model.Entities;
using Takas.Core.Services.Interfaces;
using Takas.Infrastructure.Data;
using Takas.Infrastructure.Data.Repositories;

namespace Takas.Infrastructure.Services
{
    public class ProductImageService:Repository<ProductImage>,IProductImageService
    {
        public ProductImageService(TakasDbContext context) : base(context)
        {

        }
    }
}
