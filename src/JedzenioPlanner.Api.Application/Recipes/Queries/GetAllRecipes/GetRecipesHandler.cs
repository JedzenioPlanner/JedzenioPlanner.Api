using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JedzenioPlanner.Api.Domain.Common.Persistence;
using JedzenioPlanner.Api.Domain.Recipes;
using MediatR;

namespace JedzenioPlanner.Api.Application.Recipes.Queries.GetAllRecipes
{
    public class GetRecipesHandler : IRequestHandler<GetRecipes, IEnumerable<Recipe>>
    {
        private readonly IAggregatesRepository<Recipe> _repository;
        private readonly Random _random;

        public GetRecipesHandler(IAggregatesRepository<Recipe> repository)
        {
            _repository = repository;
            _random = new Random();
        }

        public async Task<IEnumerable<Recipe>> Handle(GetRecipes request, CancellationToken cancellationToken)
        {
            var recipes = await _repository.GetAll();
            return recipes
                .Select(x => { x.Metadata = null; return x; })
                .Skip(request.Page * request.Amount)
                .Take(request.Amount)
                .AsEnumerable();
        }
    }
}