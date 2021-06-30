using System;
using System.Collections.Generic;
using System.Text;
using Takas.Core.Model.Entities;
using Takas.Core.Services.Interfaces;
using Takas.Infrastructure.Data;
using Takas.Infrastructure.Data.Repositories;

namespace Takas.Infrastructure.Services
{
    public class ProductService:Repository<Product>,IProductService
    {
        public ProductService(TakasDbContext context) : base(context)
        {

        }
    }
}
