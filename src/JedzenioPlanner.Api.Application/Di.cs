using System.Linq;
using System.Reflection;
using FluentValidation;
using JedzenioPlanner.Api.Application.Common;
using JedzenioPlanner.Api.Application.Common.Behaviors;
using JedzenioPlanner.Api.Application.Common.Attributes;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace JedzenioPlanner.Api.Application
{
    public static class Di
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMappers();
            
            ValidatorOptions.Global.LanguageManager.Enabled = false;
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            
            services.AddMediatR(Assembly.GetExecutingAssembly());
            return services;
        }

        private static IServiceCollection AddMappers(this IServiceCollection services)
        {
            var types = Assembly
                .GetExecutingAssembly()
                .ExportedTypes
                .Where(x => x.IsClass)
                .Where(x=>x.GetCustomAttribute<MapperAttribute>() != null);
            foreach (var type in types) services.AddTransient(type);

            return services;
        }
    }
}