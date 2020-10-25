using System;
using JedzenioPlanner.Api.Application.Common.Attributes;
using JedzenioPlanner.Api.Application.Recipes.Commands.CreateRecipe;
using JedzenioPlanner.Api.Application.Recipes.Commands.DeleteRecipe;
using JedzenioPlanner.Api.Application.Recipes.Commands.RestoreRecipe;
using JedzenioPlanner.Api.Application.Recipes.Commands.UpdateRecipe;
using JedzenioPlanner.Api.Domain.Recipes.Events;

namespace JedzenioPlanner.Api.Application.Recipes.Common
{
    [Mapper]
    public class RecipeEventsMapper
    {
        public virtual RecipeCreated Map(CreateRecipe dto, Guid entityId, DateTime publishTime, string userId)
        {
            return new RecipeCreated
            {
                Published = publishTime,
                EntityId = entityId,
                AuthorId = userId,
                Name = dto.Name,
                Description = dto.Description,
                PictureUrl = dto.PictureUrl,
                Calories = dto.Calories,
                Steps = dto.Steps,
                Ingredients = dto.Ingredients,
                MealTypes = dto.MealTypes
            };
        }

        public virtual RecipeRemoved Map(DeleteRecipe dto, DateTime publishTime, string userId)
        {
            return new RecipeRemoved
            {
                EntityId = dto.EntityId,
                AuthorId = userId,
                Published = publishTime
            };
        }

        public virtual RecipeUpdated Map(UpdateRecipe dto, DateTime publishTime, string userId)
        {
            return new RecipeUpdated
            {
                Name = dto.Name,
                EntityId = dto.EntityId,
                Published = publishTime,
                AuthorId = userId,
                Description = dto.Description,
                PictureUrl = dto.PictureUrl,
                Calories = dto.Calories,
                Steps = dto.Steps,
                Ingredients = dto.Ingredients,
                MealTypes = dto.MealTypes
            };
        }
        
        public virtual RecipeRestored Map(RestoreRecipe dto, DateTime publishTime, string userId)
        {
            return new RecipeRestored
            {
                EntityId = dto.EntityId,
                AuthorId = userId,
                Published = publishTime
            };
        }
    }
}