using Takas.Core.Model.Entities;
using Takas.Core.Services.Interfaces;
using Takas.Infrastructure.Data;
using Takas.Infrastructure.Data.Repositories;

namespace Takas.Infrastructure.Services
{
    public class CategoryService:Repository<Category>,ICategoryService
    {
        public CategoryService(TakasDbContext context) : base(context)
        {

        }
    }
}
