using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using JedzenioPlanner.Api.Domain.Common.Persistence;
using JedzenioPlanner.Api.Domain.Recipes;

namespace JedzenioPlanner.Api.Application.Recipes.Commands.RestoreRecipe
{
    public class RestoreRecipeValidator : AbstractValidator<RestoreRecipe>
    {
        private readonly IAggregatesRepository<Recipe> _repository;
        public RestoreRecipeValidator(IAggregatesRepository<Recipe> repository)
        {
            _repository = repository;

            RuleFor(x => x.EntityId)
                .NotEmpty()
                .MustAsync(DoesExist).WithMessage("Provided EntityId must not exist.");
        }

        private async Task<bool> DoesExist(Guid id, CancellationToken cancellationToken)
            => await _repository.GetById(id) == null;
    }
}