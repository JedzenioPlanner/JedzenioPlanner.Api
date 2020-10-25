using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;
using JedzenioPlanner.Api.Application;
using JedzenioPlanner.Api.Application.Common;
using JedzenioPlanner.Api.Application.Common.Interfaces;
using JedzenioPlanner.Api.Filters;
using JedzenioPlanner.Api.Infrastructure;
using JedzenioPlanner.Api.Services;
using JedzenioPlanner.Api.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

namespace JedzenioPlanner.Api
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddTransient<ICurrentUserService, CurrentUserService>();
            services.AddApplication();
            services.AddInfrastructure(_configuration);
            services.AddControllers(options =>
            {
                options.Filters.Add(new ApiExceptionFilterAttribute());
            }).AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.IgnoreNullValues = true;
            });
            services.AddAppSwagger();
            services.AddAuth0(_configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseInfrastructure();
            
            app.UseAppSwagger();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UsePathBase(new PathString(_configuration["WebAppPathBase"]));

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}