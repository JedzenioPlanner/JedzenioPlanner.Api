using System.Threading;
using System.Threading.Tasks;
using JedzenioPlanner.Api.Domain.Common.Persistence;
using JedzenioPlanner.Api.Domain.Recipes;
using MediatR;

namespace JedzenioPlanner.Api.Application.Recipes.Queries.GetRecipeById
{
    public class GetRecipeByIdHandler : IRequestHandler<GetRecipeById, Recipe>
    {
        private readonly IAggregatesRepository<Recipe> _repository;

        public GetRecipeByIdHandler(IAggregatesRepository<Recipe> repository)
        {
            _repository = repository;
        }

        public async Task<Recipe> Handle(GetRecipeById request, CancellationToken cancellationToken)
        { 
            var recipe = await _repository.GetById(request.Id);
            if (request.StripMetadata) recipe.Metadata = null;
            return recipe;
        }
    }
}