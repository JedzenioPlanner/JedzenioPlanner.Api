using Microsoft.EntityFrameworkCore;

namespace JedzenioPlanner.Api.Infrastructure.Persistence.EventStore
{
    public class EventsDbContext : DbContext
    {
        public EventsDbContext(DbContextOptions<EventsDbContext> options) : base(options)
        {
            
        }

        public DbSet<EventDatabaseEntity> Events { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EventDatabaseEntity>(options =>
            {
                options.HasKey(x => new {x.EntityId, x.EntityVersion});
            });
        }
    }
}