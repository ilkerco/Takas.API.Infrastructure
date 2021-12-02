using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Takas.Core.Model.Entities;
using Takas.Core.Model.Interfaces;

namespace Takas.Infrastructure.Data
{
    public class TakasDbContext : IdentityDbContext<User>
    {
        private static IHttpContextAccessor _httpContextAccessor;
        public static HttpContext CurrentHttpContext => _httpContextAccessor.HttpContext;
        public TakasDbContext(DbContextOptions<TakasDbContext> options) : base(options) { }
        public TakasDbContext(DbContextOptions<TakasDbContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }


        #region ConfigureDb
        public DbSet<Product> Product { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<ProductImage> ProductImage { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<Message> Messages { get; set; }
        #endregion
        #region ConfigureFluentApi
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            ConfigureEntities(builder);
        }
        public void ConfigureEntities(ModelBuilder builder)
        {
            builder.Entity<Product>(ConfigureProduct);
            //builder.Entity<Image>(ConfigureProduct);
            //builder.Entity<UserInfo>(ConfigureProduct);
        }
        public void ConfigureProduct(EntityTypeBuilder<Product> builder)
        {
            builder.HasOne(x => x.Owner);
            builder.HasOne(x => x.Category);
            //builder.HasIndex(prop => prop.Title).IsUnique();
        }
        #endregion

        public Func<DateTime> TimestampProvider { get; set; } = () => DateTime.Now;
        public Func<string> UserProvider = () => GetCurrentUser(_httpContextAccessor.HttpContext);
        public static string GetCurrentUser(HttpContext ctx)
        {
            var claim = ctx?.User?.FindFirst(ClaimTypes.Name);
            if (claim != null) return claim.Value;
            return Environment.UserName;
        }
        public override int SaveChanges()
        {
            TrackChanges();
            return base.SaveChanges();
        }
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                TrackChanges();
                var result = await base.SaveChangesAsync(cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

        }
        private void TrackChanges()
        {
            try
            {
                foreach (var entry in ChangeTracker.Entries().Where(e =>
                e.State == EntityState.Added || e.State == EntityState.Modified))
                {

                    if (entry.Entity is IBaseEntity audible)
                    {
                        if (entry.State == EntityState.Added)
                        {
                            audible.CreatedAt = TimestampProvider();
                            audible.CreatedBy = UserProvider.Invoke();
                        }
                        else
                        {
                            audible.UpdatedAt = TimestampProvider();
                            audible.UpdatedBy = UserProvider.Invoke();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
