using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JedzenioPlanner.Api.Domain.Common.Persistence;
using JedzenioPlanner.Api.Domain.Recipes;
using MediatR;

namespace JedzenioPlanner.Api.Application.Recipes.Queries.GetRandomRecipes
{
    public class GetRandomRecipesHandler : IRequestHandler<GetRandomRecipes, IEnumerable<Guid>>
    {
        private readonly IAggregatesRepository<Recipe> _repository;
        private readonly Random _random;

        public GetRandomRecipesHandler(IAggregatesRepository<Recipe> repository)
        {
            _repository = repository;
            _random = new Random();
        }
        
        public async Task<IEnumerable<Guid>> Handle(GetRandomRecipes request, CancellationToken cancellationToken)
        {
            var recipes = await _repository.GetAll();
            return recipes
                .Select(x => x.Id)
                .OrderBy(x => Guid.NewGuid())
                .AsEnumerable();
        }
    }
}