using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using JedzenioPlanner.Api.Application.Common.Interfaces;
using JedzenioPlanner.Api.Domain.Common.Persistence;
using JedzenioPlanner.Api.Domain.Recipes;

namespace JedzenioPlanner.Api.Application.Recipes.Commands.DeleteRecipe
{
    public class DeleteRecipeValidator : AbstractValidator<DeleteRecipe>
    {
        private readonly IAggregatesRepository<Recipe> _repository;
        private readonly ICurrentUserService _currentUserService;
        
        public DeleteRecipeValidator(IAggregatesRepository<Recipe> repository, ICurrentUserService currentUserService)
        {
            _repository = repository;
            _currentUserService = currentUserService;

            RuleFor(x => x.EntityId)
                .NotEmpty()
                .MustAsync(Exist).WithMessage("Provided EntityId must already exist.")
                .MustAsync(MustBeCreatedByGivenUser).WithMessage("Provided Entity must be created by authenticated user.");
        }

        private async Task<bool> Exist(Guid id, CancellationToken cancellationToken)
            => await _repository.GetById(id) != null;
        
        private async Task<bool> MustBeCreatedByGivenUser(Guid entityId, CancellationToken cancellationToken)
        {
            var recipe = await _repository.GetById(entityId);
            return recipe?.Metadata?.Creation?.AuthorId == _currentUserService.GetUserId();
        }
    }
}