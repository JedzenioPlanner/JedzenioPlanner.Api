using System;
using JedzenioPlanner.Api.Infrastructure.Persistence.Repositories.Recipes;
using Microsoft.EntityFrameworkCore;

namespace JedzenioPlanner.Api.InfrastructureTests.MenuGenerator
{
    public sealed class FakeRecipesDbContext : RecipesDbContext
    {
        public FakeRecipesDbContext() : base(new DbContextOptions<RecipesDbContext>())
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RecipeDatabaseEntity>(x =>
            {
                x.HasKey(y => y.Id);

                x.HasData(RecipeDatabaseEntity.FromRecipe(MockData.SampleBreakfast));
                x.HasData(RecipeDatabaseEntity.FromRecipe(MockData.SampleLunch));
                x.HasData(RecipeDatabaseEntity.FromRecipe(MockData.SampleDinner));
                x.HasData(RecipeDatabaseEntity.FromRecipe(MockData.SampleSnack));
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}