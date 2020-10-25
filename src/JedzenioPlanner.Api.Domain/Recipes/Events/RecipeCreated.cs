using System.Collections.Generic;
using JedzenioPlanner.Api.Domain.Common;
using JedzenioPlanner.Api.Domain.Recipes.Enums;

namespace JedzenioPlanner.Api.Domain.Recipes.Events
{
    public class RecipeCreated : Event<Recipe>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string PictureUrl { get; set; }
        public int Calories { get; set; }
        public IEnumerable<string> Ingredients { get; set; }
        public IEnumerable<string> Steps { get; set; }
        public IEnumerable<MealType> MealTypes { get; set; }

        protected internal override Recipe Apply(Recipe entity)
        {
            entity.Name = Name;
            entity.Description = Description;
            entity.PictureUrl = PictureUrl;
            entity.Calories = Calories;
            entity.Ingredients = Ingredients;
            entity.Steps = Steps;
            entity.MealTypes = MealTypes;
            return entity;
        }
    }
}