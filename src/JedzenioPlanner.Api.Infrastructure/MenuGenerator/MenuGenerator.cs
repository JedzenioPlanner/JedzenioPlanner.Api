using System;
using System.Linq;
using System.Threading.Tasks;
using JedzenioPlanner.Api.Application.Common.Interfaces;
using JedzenioPlanner.Api.Domain.MealPlan;
using JedzenioPlanner.Api.Domain.Recipes;
using JedzenioPlanner.Api.Domain.Recipes.Enums;
using JedzenioPlanner.Api.Infrastructure.Persistence.Repositories.Recipes;
using Microsoft.EntityFrameworkCore;

namespace JedzenioPlanner.Api.Infrastructure.MenuGenerator
{
    public class MenuGenerator : IMenuGenerator
    {
        private readonly RecipesDbContext _context;
        private readonly Random _random;

        public MenuGenerator(RecipesDbContext context)
        {
            _context = context;
            _random = new Random();
        }

        public async Task<Recipe> FindRecipe(MealType type, int caloriesTarget)
        {
            var count = _context
                .Recipes
                .Where(x => x.MealTypes.Contains(((int) type).ToString()))
                .Count(x => x.Calories >= caloriesTarget * 0.8 && x.Calories <= caloriesTarget * 1.2);

            if (count > 0)
            {
                var recipe = await _context
                    .Recipes
                    .Where(x => x.MealTypes.Contains(((int) type).ToString()))
                    .Where(x => x.Calories >= caloriesTarget * 0.8 && x.Calories <= caloriesTarget * 1.2)
                    .Skip(_random.Next(0, count))
                    .FirstAsync();

                return recipe.ToRecipe();
            }
            else
            {
                var recipe = await _context
                    .Recipes
                    .Where(x => x.MealTypes.Contains(((int) type).ToString()))
                    .OrderBy(x => Math.Abs(x.Calories - caloriesTarget))
                    .FirstOrDefaultAsync();

                return recipe.ToRecipe();
            }
        }

        public async Task<MealPlan> GenerateMenu(MealPlan plan)
        {
            foreach (var row in plan.MealPlanRows)
            {
                var count = _context
                    .Recipes
                    .Where(x => x.MealTypes.Contains(((int) row.Type).ToString()))
                    .Count(x => x.Calories >= row.Calories * 0.8 && x.Calories <= row.Calories * 1.2);

                if (count > 0)
                {
                    var recipe = await _context
                        .Recipes
                        .Where(x => x.MealTypes.Contains(((int) row.Type).ToString()))
                        .Where(x => x.Calories >= row.Calories * 0.8 && x.Calories <= row.Calories * 1.2)
                        .Skip(_random.Next(0, count))
                        .FirstAsync();

                    row.Recipe = recipe.ToRecipe();
                    plan.Recalculate();
                }
                else
                {
                    var recipe = await _context
                        .Recipes
                        .Where(x => x.MealTypes.Contains(((int) row.Type).ToString()))
                        .OrderBy(x => Math.Abs(x.Calories - Convert.ToInt32(row.Calories)))
                        .FirstOrDefaultAsync();

                    row.Recipe = recipe.ToRecipe();
                    plan.Recalculate();
                }
            }

            return plan;
        }
    }
}