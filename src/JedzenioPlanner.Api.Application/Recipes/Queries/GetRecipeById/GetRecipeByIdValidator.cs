using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using JedzenioPlanner.Api.Domain.Common.Persistence;
using JedzenioPlanner.Api.Domain.Recipes;

namespace JedzenioPlanner.Api.Application.Recipes.Queries.GetRecipeById
{
    public class GetRecipeByIdValidator : AbstractValidator<GetRecipeById>
    {
        private readonly IAggregatesRepository<Recipe> _repository;
        
        public GetRecipeByIdValidator(IAggregatesRepository<Recipe> repository)
        {
            _repository = repository;
            
            RuleFor(x => x.Id)
                .NotEmpty()
                .MustAsync(Exist);
        }

        private async Task<bool> Exist(Guid id, CancellationToken cancellationToken) 
            => await _repository.GetById(id) != null;
    }
}