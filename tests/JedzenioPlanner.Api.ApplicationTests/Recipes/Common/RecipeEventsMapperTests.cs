using System;
using FluentAssertions;
using JedzenioPlanner.Api.Application.Recipes.Commands.CreateRecipe;
using JedzenioPlanner.Api.Application.Recipes.Commands.DeleteRecipe;
using JedzenioPlanner.Api.Application.Recipes.Commands.RestoreRecipe;
using JedzenioPlanner.Api.Application.Recipes.Commands.UpdateRecipe;
using JedzenioPlanner.Api.Application.Recipes.Common;
using JedzenioPlanner.Api.Domain.Recipes.Enums;
using JedzenioPlanner.Api.Domain.Recipes.Events;
using Xunit;

namespace JedzenioPlanner.Api.ApplicationTests.Recipes.Common
{
    public class RecipeEventsMapperTests
    {
        private readonly RecipeEventsMapper _mapper = new RecipeEventsMapper();
    
        [Fact]
        public void Does_Map_Create_Recipe_Work_Correctly()
        {
            var dto = new CreateRecipe
            {
                Name = "Pizza Hawajska",
                Calories = 1200,
                Description = "Smaczna i zdrowa!",
                MealTypes = new[] {MealType.Snack, MealType.Lunch},
                PictureUrl = "https://localhost:5001/img/pizza_hawajska.png",
                Steps = new[] {"Go into website and order it via phone!", "Wait for pizza boy to come and collect it from him."},
                Ingredients = new[] {"1x Phone"}
            };
            var entityId = Guid.Parse("be3dc63e-8f22-4d05-84b1-5fa6f4f652c4");
            var expected = new RecipeCreated
            {
                EntityId = Guid.Parse("be3dc63e-8f22-4d05-84b1-5fa6f4f652c4"),
                Name = "Pizza Hawajska",
                Calories = 1200,
                Description = "Smaczna i zdrowa!",
                MealTypes = new[] {MealType.Snack, MealType.Lunch},
                PictureUrl = "https://localhost:5001/img/pizza_hawajska.png",
                Steps = new[] {"Go into website and order it via phone!", "Wait for pizza boy to come and collect it from him."},
                Ingredients = new[] {"1x Phone"},
                AuthorId = "7f7dfc41-3b52-4c43-aef9-b82099a7beb2",
                Published = new DateTime(2020, 10,10,5,2,1),
                Version = 0
            };
            const string userId = "7f7dfc41-3b52-4c43-aef9-b82099a7beb2";
            var date = new DateTime(2020, 10,10,5,2,1);
    
            var result = _mapper.Map(dto, entityId, date, userId);
            
            result.Should().BeEquivalentTo(expected);
        }
        
        [Fact]
        public void Does_Map_Create_Not_Throw_Exception_When_No_Steps_Are_Null()
        {
            var dto = new CreateRecipe
            {
                Name = "Pizza Hawajska",
                Calories = 1200,
                Description = "Smaczna i zdrowa!",
                MealTypes = new[] {MealType.Snack, MealType.Lunch},
                PictureUrl = "https://localhost:5001/img/pizza_hawajska.png"
                //No Steps -> Steps = null
            };
            var entityId = Guid.Parse("be3dc63e-8f22-4d05-84b1-5fa6f4f652c4");
            var userId = "7f7dfc41-3b52-4c43-aef9-b82099a7beb2";
            var date = new DateTime(2020, 10,10,5,2,1);
    
            Action a = () => _mapper.Map(dto, entityId, date, userId);
            
            a.Should().NotThrow();
        }
    
        [Fact]
        public void Does_Map_Create_Not_Throw_Exception_When_Ingredients_Are_Null()
        {
            var dto = new CreateRecipe
            {
                Name = "Pizza Hawajska",
                Calories = 1200,
                Description = "Smaczna i zdrowa!",
                MealTypes = new[] {MealType.Snack, MealType.Lunch},
                PictureUrl = "https://localhost:5001/img/pizza_hawajska.png",
                Steps = new[] {"Go into website and order it via phone!", "Wait for pizza boy to come and collect it from him."}
            };
            var entityId = Guid.Parse("be3dc63e-8f22-4d05-84b1-5fa6f4f652c4");
            var userId = "7f7dfc41-3b52-4c43-aef9-b82099a7beb2";
            var date = new DateTime(2020, 10,10,5,2,1);
    
            Action a =  () => _mapper.Map(dto, entityId, date, userId);
            
            a.Should().NotThrow();
        }
    
        [Fact]
        public void Does_Map_Delete_Work_Correctly()
        {
            var entityId = Guid.Parse("6f9d8b35-481a-4bb4-ad69-44cf00e55717");
            var dto = new DeleteRecipe(entityId);
            var userId = "7f7dfc41-3b52-4c43-aef9-b82099a7beb2";
            var date = new DateTime(2020, 10,10,5,2,1);
            var expected = new RecipeRemoved
            {
                AuthorId = userId,
                Published = date,
                EntityId = entityId,
            };
    
            var result = _mapper.Map(dto, date, userId);
            
            result.Should().BeEquivalentTo(expected);
        }
    
        [Fact]
        public void Does_Map_Update_Recipe_Work_Correctly()
        {
            var dto = new UpdateRecipe
            {
                EntityId = Guid.Parse("be3dc63e-8f22-4d05-84b1-5fa6f4f652c4"),
                Name = "Pizza Hawajska",
                Calories = 1200,
                Description = "Smaczna i zdrowa!",
                MealTypes = new[] {MealType.Snack, MealType.Lunch},
                PictureUrl = "https://localhost:5001/img/pizza_hawajska.png",
                Steps = new[] {"Go into website and order it via phone!", "Wait for pizza boy to come and collect it from him."},
                Ingredients = new[] {"1x Phone"}
            };
            var expected = new RecipeUpdated
            {
                EntityId = Guid.Parse("be3dc63e-8f22-4d05-84b1-5fa6f4f652c4"),
                Name = "Pizza Hawajska",
                Calories = 1200,
                Description = "Smaczna i zdrowa!",
                MealTypes = new[] {MealType.Snack, MealType.Lunch},
                PictureUrl = "https://localhost:5001/img/pizza_hawajska.png",
                Steps = new[] {"Go into website and order it via phone!", "Wait for pizza boy to come and collect it from him."},
                Ingredients = new[] {"1x Phone"},
                AuthorId = "7f7dfc41-3b52-4c43-aef9-b82099a7beb2",
                Published = new DateTime(2020, 10,10,5,2,1),
            };
            var userId = "7f7dfc41-3b52-4c43-aef9-b82099a7beb2";
            var date = new DateTime(2020, 10,10,5,2,1);
    
            var result = _mapper.Map(dto, date, userId);
            
            result.Should().BeEquivalentTo(expected);
        }
        
        [Fact]
        public void Does_Map_Update_Not_Throw_Exception_When_No_Steps_Are_Null()
        {
            var dto = new UpdateRecipe()
            {
                EntityId = Guid.Parse("be3dc63e-8f22-4d05-84b1-5fa6f4f652c4"),
                Name = "Pizza Hawajska",
                Calories = 1200,
                Description = "Smaczna i zdrowa!",
                MealTypes = new[] {MealType.Snack, MealType.Lunch},
                PictureUrl = "https://localhost:5001/img/pizza_hawajska.png"
                //No Steps -> Steps = null
            };
            var userId = "7f7dfc41-3b52-4c43-aef9-b82099a7beb2";
            var date = new DateTime(2020, 10,10,5,2,1);
    
            Action a = () => _mapper.Map(dto, date, userId);
            
            a.Should().NotThrow();
        }
    
        [Fact]
        public void Does_Map_Update_Not_Throw_Exception_When_Ingredients_Are_Null()
        {
            var dto = new UpdateRecipe
            {
                EntityId = Guid.Parse("be3dc63e-8f22-4d05-84b1-5fa6f4f652c4"),
                Name = "Pizza Hawajska",
                Calories = 1200,
                Description = "Smaczna i zdrowa!",
                MealTypes = new[] {MealType.Snack, MealType.Lunch},
                PictureUrl = "https://localhost:5001/img/pizza_hawajska.png",
                Steps = new[] {"Go into website and order it via phone!", "Wait for pizza boy to come and collect it from him."}
            };
            var userId = "7f7dfc41-3b52-4c43-aef9-b82099a7beb2";
            var date = new DateTime(2020, 10,10,5,2,1);
    
            Action a =  () => _mapper.Map(dto, date, userId);
            
            a.Should().NotThrow();
        }
    
        [Fact]
        public void Does_Map_Restore_Recipe_Work_Correctly()
        {
            var entityId = Guid.Parse("6f9d8b35-481a-4bb4-ad69-44cf00e55717");
            var dto = new RestoreRecipe
            {
                EntityId = entityId
            };
            var userId = "7f7dfc41-3b52-4c43-aef9-b82099a7beb2";
            var date = new DateTime(2020, 10,10,5,2,1);
            var expected = new RecipeRestored
            {
                AuthorId = userId,
                Published = date,
                EntityId = entityId,
            };
    
            var result = _mapper.Map(dto, date, userId);
            
            result.Should().BeEquivalentTo(expected);
        }
    }
}