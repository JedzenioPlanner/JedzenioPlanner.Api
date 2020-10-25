using System.Collections.Generic;
using System.Threading.Tasks;
using JedzenioPlanner.Api.Domain.MealPlan;
using JedzenioPlanner.Api.Domain.Recipes;
using JedzenioPlanner.Api.Domain.Recipes.Enums;

namespace JedzenioPlanner.Api.Application.Common.Interfaces
{
    public interface IMenuGenerator
    {
        public Task<Recipe> FindRecipe(MealType type, int caloriesTarget);
        public Task<MealPlan> GenerateMenu(MealPlan plan);
    }
}