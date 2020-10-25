using System;
using System.Linq;
using DeepEqual.Syntax;
using FluentAssertions;
using JedzenioPlanner.Api.Domain.Common.SubElements;
using JedzenioPlanner.Api.Domain.Recipes;
using JedzenioPlanner.Api.Domain.Recipes.Enums;
using JedzenioPlanner.Api.Infrastructure.Persistence.EventStore;
using JedzenioPlanner.Api.Infrastructure.Persistence.Repositories.Recipes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Xunit;

namespace JedzenioPlanner.Api.InfrastructureTests.Persistence.Repositories
{
    public class RecipesDbContextTests
    {
        [Fact]
        public async void ShouldGetByIdCorrectly()
        {
            var context = new FakeRecipesDbContext();
            
            var result = await context.GetById(new Guid("0104838E-ABDF-4FB4-9B20-12B97833AF5D"));
            
            var expected = new Recipe
            {
                Id = new Guid("0104838E-ABDF-4FB4-9B20-12B97833AF5D"),
                Metadata = new Metadata
                {
                    Version = 1,
                    Removed = false,
                    Creation = new Creation{
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
            result.Should().BeEquivalentTo(expected);
        }
    
        [Fact]
        public async void ShouldGetAllCorrectly()
        {
            var context = new FakeRecipesDbContext();
            
            var result = await context.GetAll();
            
            var expected = new Recipe
            {
                Id = new Guid("0104838E-ABDF-4FB4-9B20-12B97833AF5D"),
                Metadata = new Metadata
                {
                    Version = 1,
                    Removed = false,
                    Creation = new Creation{
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
            var enumerable = result as Recipe[] ?? result.ToArray();
            
            enumerable.Should().HaveCount(1);
            enumerable.Should().ContainSingle(x => x.WithDeepEqual(expected).Compare());
        }
    
        [Fact]
        public async void ShouldAddCorrectly()
        {
            var context = new FakeRecipesDbContext();
            var recipe = new Recipe
            {
                Id = new Guid("0161C29D-45A7-412B-8232-BC598CBD7CA1"),
                Metadata = new Metadata
                {
                    Version = 1,
                    Removed = false,
                    Creation = new Creation{
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
            
            await context.Add(recipe);
            await context.SaveChanges();
    
            context.Recipes
                .Should().HaveCount(2)
                .And.ContainSingle(x => x.Id == Guid.Parse("0104838E-ABDF-4FB4-9B20-12B97833AF5D"));
        }
    
        [Fact]
        public async void ShouldUpdateCorrectly()
        {
            var context = new FakeRecipesDbContext();
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
                    Updates = new[]
                    {
                        new Update
                        {
                            AuthorId = "F8B5EF68-2526-4635-8C91-A5AB98465D0F",
                            Published = new DateTime(2010, 1, 1)
                        }
                    }
                },
                Name = "new-sample-name",
                Description = "sample-description",
                PictureUrl = "https://example.com/sample-picture.png",
                Calories = 1234,
                MealTypes = new[] {MealType.Snack},
                Steps = new[] {"1. Sample first step.", "2. Sample second step."},
                Ingredients = new[] {"Sample first ingredient", "Sample second ingredient"}
            };
    
            await context.Update(recipe);
            await context.SaveChanges();
    
            context.Recipes
                .Should().HaveCount(1)
                .And.ContainSingle(x => 
                    x.Id == Guid.Parse("0104838E-ABDF-4FB4-9B20-12B97833AF5D") && x.Name == "new-sample-name");
        }
    
        [Fact]
        public async void ShouldDeleteCorrectly()
        {
            var context = new FakeRecipesDbContext();
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
            
            await context.Delete(recipe);
            await context.SaveChanges();
    
            context.Recipes.Should().HaveCount(0);
        }
    }
}