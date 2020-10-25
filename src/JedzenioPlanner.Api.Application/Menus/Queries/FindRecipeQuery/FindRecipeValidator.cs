using FluentValidation;

namespace JedzenioPlanner.Api.Application.Menus.Queries.FindRecipeQuery
{
    public class FindRecipeValidator : AbstractValidator<FindRecipe>
    {
        public FindRecipeValidator()
        {
            RuleFor(x => x.Type)
                .NotNull();

            RuleFor(x => x.CaloriesTarget)
                .NotEmpty()
                .InclusiveBetween(100, 2300);
        }
    }
}