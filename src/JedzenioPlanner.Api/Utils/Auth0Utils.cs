using System;
using System.IO;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace JedzenioPlanner.Api.Utils
{
    public static class Auth0Utils
    {
        public static IServiceCollection AddAuth0(this IServiceCollection services, IConfiguration configuration)
        {
            var authority = configuration["Auth0:Authority"];
            var audience = configuration["Auth0:Audience"];
            var rsaLocation = configuration["Auth0:RsaXml"];
            Validate(authority, audience, rsaLocation);
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Authority = authority;
                options.Audience = audience;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = "azp",
                    RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role",
                    IssuerSigningKey = GetRsaSecurityKey(rsaLocation)
                };
            });
            services.AddAuthorization();
            return services;
        }

        private static void Validate(string authority, string audience, string rsaLocation)
        {
            if (string.IsNullOrEmpty(audience) || string.IsNullOrEmpty(authority))
            {
                throw new Exception("Authority and audience not supplied. Please set them up in Auth0:Authority and Auth0:Audience in appsettings.json.");
            }
            if (!File.Exists(rsaLocation))
            {
                throw new FileNotFoundException("rsa.xml file must be supplied in order to verify jwt from auth0 correctly.");
            }
        }
        
        private static RsaSecurityKey GetRsaSecurityKey(string location)
        {
            var rsa = RSA.Create();
            rsa.FromXmlString(File.ReadAllText(location));
            return new RsaSecurityKey(rsa);
        }
    }
}