using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Text;
using Takas.Infrastructure.Data;

namespace Takas.Infrastructure
{
    public class DesingTimeDbContextFactory : IDesignTimeDbContextFactory<TakasDbContext>
    {
        public TakasDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<TakasDbContext>();
            const string connectionString = "Server=DESKTOP-12403TV\\SQLEXPRESS;Database=Takas;Trusted_Connection=True;MultipleActiveResultSets=true";
            builder.UseSqlServer(connectionString);
            return new TakasDbContext(builder.Options);
        }
    }
}
