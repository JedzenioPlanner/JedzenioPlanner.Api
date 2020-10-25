using System;
using JedzenioPlanner.Api.Domain.Common.SubElements;
using JedzenioPlanner.Api.Domain.Recipes;
using JedzenioPlanner.Api.Domain.Recipes.Enums;
using JedzenioPlanner.Api.Infrastructure.Persistence.Repositories.Recipes;
using Microsoft.EntityFrameworkCore;

namespace JedzenioPlanner.Api.InfrastructureTests.Persistence.Repositories
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
            var recipe = new Recipe
            {
                Id = new Guid("0104838E-ABDF-4FB4-9B20-12B97833AF5D"),
                Metadata = new Metadata
                {
                    Version = 1,
                    Removed = false,
                    Creation = new Creation
                    {
                        AuthorId = "F8B5EF68-2526-4635-8C91-A5AB98465D0F",
                        Published = new DateTime(2010, 1, 1)
                    },
                    Updates = new Update[0]
                },
                Name = "sample-name",
                Description = "sample-description",
                PictureUrl = "https://example.com/sample-picture.png",
                Calories = 1234,
                MealTypes = new[] {MealType.Snack},
                Steps = new[] {"1. Sample first step.", "2. Sample second step."},
                Ingredients = new[] {"Sample first ingredient", "Sample second ingredient"}
            };
    
            modelBuilder.Entity<RecipeDatabaseEntity>(x =>
            {
                x.HasKey(y => y.Id);
                x.HasData(RecipeDatabaseEntity.FromRecipe(recipe));
            });
            
            base.OnModelCreating(modelBuilder);
        }
    }
}