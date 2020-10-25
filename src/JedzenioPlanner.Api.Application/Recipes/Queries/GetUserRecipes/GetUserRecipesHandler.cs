using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JedzenioPlanner.Api.Domain.Common.Persistence;
using JedzenioPlanner.Api.Domain.Recipes;
using MediatR;

namespace JedzenioPlanner.Api.Application.Recipes.Queries.GetUserRecipes
{
    public class GetUserRecipesHandler : IRequestHandler<GetUserRecipes, IEnumerable<Recipe>>
    {
        private readonly IAggregatesRepository<Recipe> _repository;

        public GetUserRecipesHandler(IAggregatesRepository<Recipe> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Recipe>> Handle(GetUserRecipes request, CancellationToken cancellationToken)
        {
            var recipes = await _repository.GetAll();
            return recipes
                .Where(x => x.Metadata.Creation.AuthorId == request.UserId)
                .Skip(request.Amount * request.Page)
                .Take(request.Amount);
        }
    }
}