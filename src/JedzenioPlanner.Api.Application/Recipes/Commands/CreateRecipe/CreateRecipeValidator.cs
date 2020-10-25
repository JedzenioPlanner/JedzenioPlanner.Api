using System;
using FluentValidation;

namespace JedzenioPlanner.Api.Application.Recipes.Commands.CreateRecipe
{
    public class CreateRecipeValidator : AbstractValidator<CreateRecipe>
    {
        public CreateRecipeValidator()
        {
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
                .Must(IsDefaultString).WithMessage("Steps[] must not be a placeholder.");

            RuleFor(x => x.MealTypes)
                .NotEmpty();
        }
        
        private bool IsValidUrl(string url)
            => string.IsNullOrEmpty(url) || Uri.TryCreate(url, UriKind.Absolute, out _);
        
        private static bool IsDefaultString(string str)
            => str != "string";
    }
}