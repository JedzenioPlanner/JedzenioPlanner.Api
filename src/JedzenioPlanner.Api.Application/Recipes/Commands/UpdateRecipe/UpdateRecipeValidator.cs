using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using JedzenioPlanner.Api.Application.Common.Interfaces;
using JedzenioPlanner.Api.Domain.Common.Persistence;
using JedzenioPlanner.Api.Domain.Recipes;

namespace JedzenioPlanner.Api.Application.Recipes.Commands.UpdateRecipe
{
    public class UpdateRecipeValidator : AbstractValidator<UpdateRecipe>
    {
        private readonly IAggregatesRepository<Recipe> _repository;
        private readonly ICurrentUserService _currentUserService;
        
        public UpdateRecipeValidator(IAggregatesRepository<Recipe> repository, ICurrentUserService currentUserService)
        {
            _repository = repository;
            _currentUserService = currentUserService;

            RuleFor(x => x.EntityId)
                .NotEmpty()
                .MustAsync(Exist).WithMessage("Provided EntityId must already exist.")
                .MustAsync(MustBeCreatedByGivenUser).WithMessage("Provided Entity must be created by authenticated user.");

            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(256)
                .Must(IsDefaultString).WithMessage("Name must not be a placeholder.");

            RuleFor(x => x.Description)
                .NotEmpty()
                .MaximumLength(3072)
                .Must(IsDefaultString).WithMessage("Description must not be a placeholder.");

            RuleFor(x => x.PictureUrl)
                .MaximumLength(2048)
                .Must(IsValidUrl).WithMessage("PictureUrl must be a valid PictureUrl");
            
            RuleFor(x => x.Calories)
                .NotEmpty();

            RuleFor(x => x.Ingredients)
                .NotEmpty();
            
            RuleForEach(x => x.Ingredients)
                .NotEmpty()
                .MaximumLength(256)
                .Must(IsDefaultString).WithMessage("Ingredients[] must not be a placeholder.");

            RuleFor(x => x.Steps)
                .NotEmpty();
            
            RuleForEach(x => x.Steps)
                .NotEmpty()
                .MaximumLength(3072)
                .Must(IsDefaultString).WithMessage("Ingredients[] must not be a placeholder.");
            
            RuleFor(x => x.MealTypes)
                .NotEmpty();
        }

        private async Task<bool> Exist(Guid id, CancellationToken cancellationToken)
            => await _repository.GetById(id) != null;

        private bool IsValidUrl(string url)
            => string.IsNullOrEmpty(url) || Uri.TryCreate(url, UriKind.Absolute, out _);
        
        private static bool IsDefaultString(string str)
            => str != "string";

        private async Task<bool> MustBeCreatedByGivenUser(Guid entityId, CancellationToken cancellationToken)
        {
            var recipe = await _repository.GetById(entityId);
            return recipe?.Metadata?.Creation?.AuthorId == _currentUserService.GetUserId();
        }
    }
}