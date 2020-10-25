using System.Threading;
using System.Threading.Tasks;
using JedzenioPlanner.Api.Application.Common.Interfaces;
using JedzenioPlanner.Api.Domain.Recipes;
using MediatR;

namespace JedzenioPlanner.Api.Application.Menus.Queries.FindRecipeQuery
{
    public class FindRecipeHandler : IRequestHandler<FindRecipe, Recipe>
    {
        private readonly IMenuGenerator _generator;

        public FindRecipeHandler(IMenuGenerator generator)
        {
            _generator = generator;
        }

        public async Task<Recipe> Handle(FindRecipe request, CancellationToken cancellationToken)
        {
            var recipe = await _generator.FindRecipe(request.Type, request.CaloriesTarget);
            recipe.Metadata = null;

            return recipe;
        }
    }
}