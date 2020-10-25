using System;
using System.Collections.Generic;
using JedzenioPlanner.Api.Application.Recipes.Commands.UpdateRecipe;
using JedzenioPlanner.Api.Domain.Recipes.Enums;

namespace JedzenioPlanner.Api.Models.Recipe
{
    public class UpdateRecipeModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string PictureUrl { get; set; }
        public int Calories { get; set; }
        public IEnumerable<string> Ingredients { get; set; }
        public IEnumerable<string> Steps { get; set; }
        public IEnumerable<MealType> MealTypes { get; set; }
            
        public UpdateRecipe Map(Guid id)
        {
            return new UpdateRecipe
            {
                EntityId = id,
                Name = this.Name,
                Description = this.Description,
                PictureUrl = this.PictureUrl,
                Calories = this.Calories,
                Ingredients = this.Ingredients,
                Steps = this.Steps,
                MealTypes = this.MealTypes
            };
        }
    }
}