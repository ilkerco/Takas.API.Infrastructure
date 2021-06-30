using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Takas.Core.Model.Entities;
using Takas.Core.Repositories.Database;
using Takas.Infrastructure.Data;
using Takas.Infrastructure.Data.Repositories;
using Takas.WebApi.Services.Interfaces;

namespace Takas.WebApi.Helpers
{
    public static class BootStrapper
    {
        public static Autofac.IContainer ApplicationContainer { get; set; }
        public static void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>().SingleInstance();
            builder.RegisterType<AuthHelper>().As<IAuthHelper>().InstancePerLifetimeScope();
            builder.RegisterType<DbContext>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepository<>)).InstancePerDependency();
            builder.RegisterType<TakasDbContext>().As<DbContext>().InstancePerLifetimeScope();
        }
        public static IServiceProvider StartContainer(this IServiceCollection services)
        {
            var builder = new ContainerBuilder();
            builder.Populate(services);
            ConfigureContainer(builder);
            ApplicationContainer = builder.Build();
            return new AutofacServiceProvider(ApplicationContainer);
        }

    }
    /*private static IServiceCollection ConfigureIdentityServerr(this IServiceCollection services)
    {
        var builder = services.AddIdentityCore<User>(opt =>
        {
            opt.Password.RequireDigit = false;
            opt.Password.RequiredLength = 5;
        });
        builder = new Microsoft.AspNetCore.Identity.IdentityBuilder(builder.UserType, builder.Services);
        builder.AddEntityFrameworkStores<TakasDbContext>();
        builder.AddSignInManager<SignInManager<User>>();
        builder.AddDefaultTokenProviders();
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
        });
        return services;
    }*/
}
