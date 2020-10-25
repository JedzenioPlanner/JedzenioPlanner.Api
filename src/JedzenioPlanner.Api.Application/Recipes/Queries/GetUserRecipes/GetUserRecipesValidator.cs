using FluentValidation;

namespace JedzenioPlanner.Api.Application.Recipes.Queries.GetUserRecipes
{
    public class GetUserRecipesValidator : AbstractValidator<GetUserRecipes>
    {
        public GetUserRecipesValidator()
        {
            RuleFor(x => x.Amount)
                .InclusiveBetween(0, 100);
        }
    }
}