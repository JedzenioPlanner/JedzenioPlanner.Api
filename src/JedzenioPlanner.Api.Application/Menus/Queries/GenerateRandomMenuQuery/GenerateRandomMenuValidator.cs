using FluentValidation;

namespace JedzenioPlanner.Api.Application.Menus.Queries.GenerateRandomMenuQuery
{
    public class GenerateRandomMenuValidator : AbstractValidator<GenerateRandomMenu>
    {
        public GenerateRandomMenuValidator()
        {
            RuleFor(x => x.CaloriesTarget)
                .NotEmpty()
                .InclusiveBetween(200, 20700);

            RuleFor(x => x.MealsAmount)
                .NotEmpty()
                .InclusiveBetween(2, 9);

            RuleFor(x => x)
                .Must(BeReal).WithMessage("Supplied values must be real.")
                .Must(SumUpTo100Percent).WithMessage("Meals share in calories target must sum up to 100%.");
        }

        private static bool BeReal(GenerateRandomMenu query)
        {
            return query.MealsAmount switch
            {
                2 => query.CaloriesTarget >= 200 && query.CaloriesTarget <= 4600,
                3 => query.CaloriesTarget >= 300 && query.CaloriesTarget <= 4600,
                4 => query.CaloriesTarget >= 400 && query.CaloriesTarget <= 9200,
                5 => query.CaloriesTarget >= 500 && query.CaloriesTarget <= 11500,
                6 => query.CaloriesTarget >= 600 && query.CaloriesTarget <= 13800,
                7 => query.CaloriesTarget >= 700 && query.CaloriesTarget <= 16100,
                8 => query.CaloriesTarget >= 800 && query.CaloriesTarget <= 18400,
                9 => query.CaloriesTarget >= 900 && query.CaloriesTarget <= 20700,
                _ => false
            };
        }

        private static bool SumUpTo100Percent(GenerateRandomMenu query)
        {
            if (query.BreakfastPercentage != 0 && query.DinnerPercentage != 0 && query.LunchPercentage != 0 &&
                query.SnackPercentage != 0)
                return query.MealsAmount switch
                {
                    2 => query.BreakfastPercentage + query.DinnerPercentage == 100,
                    3 => query.BreakfastPercentage + query.LunchPercentage + query.DinnerPercentage == 100,
                    _ => query.BreakfastPercentage + query.LunchPercentage + query.DinnerPercentage +
                        (query.MealsAmount - 3) * query.SnackPercentage == 100
                };

            return true;
        }
    }
}