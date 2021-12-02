using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Takas.Core.Model.Entities;
using Takas.Core.Services.Interfaces;
using Takas.Infrastructure.Data;
using Takas.Infrastructure.Services;
using Takas.WebApi.Helpers;
using Takas.WebApi.Hubs;
using Takas.WebApi.Services.DataServices;
using Takas.WebApi.Services.DataServices.External;
using Takas.WebApi.Services.Interfaces;
using Takas.WebApi.Services.Interfaces.External;

namespace Takas.WebApi
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddControllers();
            services.AddHttpClient();
            
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1",
                    new Microsoft.OpenApi.Models.OpenApiInfo
                    {
                        Title = "Takas Swagger Api",
                        Description = "Takas Demo Swagger",
                        Version = "v1"
                    });
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}

                    }
                });
            });
            services.AddAutoMapper(typeof(Startup));
            services.AddIdentityCore<User>(opt =>
            {
                opt.Password.RequireDigit = false;
                opt.Password.RequiredLength = 5;
            }).AddEntityFrameworkStores<TakasDbContext>().AddSignInManager<SignInManager<User>>();
            services.TryAddScoped<UserManager<User>>();
            services.TryAddScoped<SignInManager<User>>();
            services.AddDbContext<TakasDbContext>(options =>
                options.UseSqlServer("Server = 45.158.14.59; Database = TakasDB; User ID = ilker8118; Password = i.S07051997352435; MultipleActiveResultSets = True"));
             //options.UseSqlServer("Server=DESKTOP-12403TV\\SQLEXPRESS;Database=Takas;Trusted_Connection=True;MultipleActiveResultSets=true"));
            
            services.TryAddSingleton<ISystemClock, SystemClock>();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("super secret ilker key")),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
            services.AddSignalR();


            var builder = new ContainerBuilder();

            builder.RegisterType<AuthHelper>().As<IAuthHelper>();
            builder.RegisterType<ProductService>().As<IProductService>();
            builder.RegisterType<UserService>().As<IUserService>();
            builder.RegisterType<MessageService>().As<IMessageService>();
            builder.RegisterType<ChatService>().As<IChatService>();
            builder.RegisterType<TakasDataService>().As<ITakasDataServices>();
            builder.RegisterType<CategoryService>().As<ICategoryService>();
            builder.RegisterType<ProductImageService>().As<IProductImageService>();
            builder.RegisterType<LoginService>().As<ILoginService>();
            builder.RegisterType<FacebookAuthService>().As<IFacebookAuthService>();
            builder.RegisterType<GoogleAuthService>().As<IGoogleAuthService>();
            
            builder.Populate(services);

            var appContainer = builder.Build();

            return new AutofacServiceProvider(appContainer);
            //services.StartContainer();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors(x => x
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
            app.UseSignalR(routes =>
            {
                routes.MapHub<ChatHub>("/chatHub");
            });
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Swagger Demo Api");
            });
            


        }
    }
}
