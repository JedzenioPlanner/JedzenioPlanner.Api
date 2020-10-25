using System.Collections.Generic;
using JedzenioPlanner.Api.Domain.Common;
using JedzenioPlanner.Api.Domain.Recipes.Enums;

namespace JedzenioPlanner.Api.Domain.Recipes.Events
{
    public class RecipeUpdated : Event<Recipe>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string PictureUrl { get; set; }
        public int? Calories { get; set; }
        public IEnumerable<string> Ingredients { get; set; }
        public IEnumerable<string> Steps { get; set; }
        public IEnumerable<MealType> MealTypes { get; set; }

        protected internal override Recipe Apply(Recipe entity)
        {
            entity.Name = Name ?? entity.Name;
            entity.Description = Description ?? entity.Description;
            entity.PictureUrl = PictureUrl ?? entity.PictureUrl;
            entity.Calories = Calories ?? entity.Calories;
            entity.Ingredients = Ingredients ?? entity.Ingredients;
            entity.Steps = Steps ?? entity.Steps;
            entity.MealTypes = MealTypes ?? entity.MealTypes;
            return entity;
        }
    }
}