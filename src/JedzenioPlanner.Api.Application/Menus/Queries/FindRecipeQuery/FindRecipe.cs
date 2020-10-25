using JedzenioPlanner.Api.Domain.Recipes;
using JedzenioPlanner.Api.Domain.Recipes.Enums;
using MediatR;

namespace JedzenioPlanner.Api.Application.Menus.Queries.FindRecipeQuery
{
    public class FindRecipe : IRequest<Recipe>
    {
        public FindRecipe(MealType type, int target)
        {
            Type = type;
            CaloriesTarget = target;
        }

        public MealType Type { get; set; }
        public int CaloriesTarget { get; set; }
    }
}