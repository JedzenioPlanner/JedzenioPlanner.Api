using FluentValidation;

namespace JedzenioPlanner.Api.Application.Recipes.Queries.GetAllRecipes
{
    public class GetRecipesValidator : AbstractValidator<GetRecipes>
    {
        public GetRecipesValidator()
        {
            RuleFor(x => x.Amount)
                .InclusiveBetween(0, 100);
        }
    }
}