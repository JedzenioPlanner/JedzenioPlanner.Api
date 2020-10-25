using JedzenioPlanner.Api.Application.Common.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace JedzenioPlanner.Api.Infrastructure.MenuGenerator
{
    public static class Di
    {
        public static IServiceCollection AddMenuGenerator(this IServiceCollection services)
        {
            services.AddTransient<IMenuGenerator, MenuGenerator>();

            return services;
        }
    }
}