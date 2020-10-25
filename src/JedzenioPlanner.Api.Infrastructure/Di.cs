using Hangfire;
using Hangfire.MemoryStorage;
using JedzenioPlanner.Api.Application.Common.Interfaces;
using JedzenioPlanner.Api.Domain.Common.Persistence;
using JedzenioPlanner.Api.Domain.Recipes;
using JedzenioPlanner.Api.Infrastructure.Common;
using JedzenioPlanner.Api.Infrastructure.Filestore;
using JedzenioPlanner.Api.Infrastructure.MenuGenerator;
using JedzenioPlanner.Api.Infrastructure.Persistence;
using JedzenioPlanner.Api.Infrastructure.Persistence.EventStore;
using JedzenioPlanner.Api.Infrastructure.Persistence.Projector;
using JedzenioPlanner.Api.Infrastructure.Persistence.Repositories.Recipes;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace JedzenioPlanner.Api.Infrastructure
{
    public static class Di
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<EventsDbContext>(options =>
            {
                options.UseSqlite(configuration.GetConnectionString("Events"));
            });
            services.AddDbContext<RecipesDbContext>(options =>
            {
                options.UseSqlite(configuration.GetConnectionString("Recipes"));
            });
            services.AddTransient<IAggregatesRepository<Recipe>, RecipesDbContext>();
            services.AddTransient<IEventProjectorTarget<Recipe>, RecipesDbContext>();
            services.AddTransient(typeof(IEventStore<>), typeof(EntityFrameworkEventStore<>));
            services.AddTransient(typeof(IEventProjector<>), typeof(EntityFrameworkEventsProjector<>));
            services.AddTransient<IDateTimeService, DateTimeService>();
            services.AddHangfire(conf => conf
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseMemoryStorage());
            services.AddHangfireServer();
            services.AddFilestore();
            services.AddMenuGenerator();
            return services;
        }

        public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app)
        {
            var env = app.ApplicationServices.GetService<IHostEnvironment>();
            if (env.IsEnvironment("docker"))
            {
                app.ApplicationServices.GetService<EventsDbContext>().Database.Migrate();
                app.ApplicationServices.GetService<RecipesDbContext>().Database.Migrate();
            }
            app.UseHangfireDashboard();
            RecurringJob.AddOrUpdate<HangfirePersistenceJobs>("catch-up-all", x=>x.CatchUp(), Cron.Hourly);
            RecurringJob.Trigger("catch-up-all");
            return app;
        }
    }
}