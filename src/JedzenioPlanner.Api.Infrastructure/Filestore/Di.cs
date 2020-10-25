using JedzenioPlanner.Api.Application.Common.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace JedzenioPlanner.Api.Infrastructure.Filestore
{
    public static class Di
    {
        public static IServiceCollection AddFilestore(this IServiceCollection services)
        {
            services.AddTransient<IFileStore, FileStore>();

            return services;
        }
    }
}