using System.Linq;
using FluentAssertions;
using JedzenioPlanner.Api.Application.Recipes.Commands.CreateRecipe;
using JedzenioPlanner.Api.Domain.Recipes.Enums;
using Xunit;

namespace JedzenioPlanner.Api.ApplicationTests.Recipes.Commands.Validators
{
    public class CreateRecipeCommandTests
    {
        [Fact]
        public async void DoesValidatorAllowCorrectRequest()
        {
            var request = new CreateRecipe
            {
                Name = "sample-name",
                Description = "sample-description",
                PictureUrl = "https://example.com/sample-image.png",
                Calories = 1234,
                MealTypes = new[] {MealType.Snack},
                Steps = new[] {"1. Sample first step.", "2. Sample second step."},
                Ingredients = new[] {"Sample first ingredient", "Sample second ingredient"}
            };
            var validator = new CreateRecipeValidator();
    
            var result = await validator.ValidateAsync(request);
    
            result.IsValid.Should().BeTrue();
        }
        
        [Fact]
        public async void DoesValidatorPreventFromCreatingRecipeWithNoName()
        {
            var request = new CreateRecipe
            {
                Name = string.Empty,
                Description = "sample-description",
                PictureUrl = "https://example.com/sample-image.png",
                Calories = 1234,
                MealTypes = new[] {MealType.Snack},
                Steps = new[] {"1. Sample first step.", "2. Sample second step."},
                Ingredients = new[] {"Sample first ingredient", "Sample second ingredient"}
            };
            var validator = new CreateRecipeValidator();
    
            var result = await validator.ValidateAsync(request);
    
            result.IsValid.Should().BeFalse();
        }
    
        [Fact]
        public async void DoesValidatorPreventFromCreatingRecipeWithTooLongName()
        {
            var request = new CreateRecipe
            {
                Name = string.Concat(Enumerable.Repeat("a", 300)),
                Description = "sample-description",
                PictureUrl = "https://example.com/sample-image.png",
                Calories = 1234,
                MealTypes = new[] {MealType.Snack},
                Steps = new[] {"1. Sample first step.", "2. Sample second step."},
                Ingredients = new[] {"Sample first ingredient", "Sample second ingredient"}
            };
            var validator = new CreateRecipeValidator();
    
            var result = await validator.ValidateAsync(request);
    
            result.IsValid.Should().BeFalse();
        }
        
        [Fact]
        public async void DoesValidatorPreventFromCreatingRecipeWithNoDescription()
        {
            var request = new CreateRecipe
            {
                Name = "sample-name",
                Description = string.Empty,
                PictureUrl = "https://example.com/sample-image.png",
                Calories = 1234,
                MealTypes = new[] {MealType.Snack},
                Steps = new[] {"1. Sample first step.", "2. Sample second step."},
                Ingredients = new[] {"Sample first ingredient", "Sample second ingredient"}
            };
            var validator = new CreateRecipeValidator();
    
            var result = await validator.ValidateAsync(request);
    
            result.IsValid.Should().BeFalse();
        }
    
        [Fact]
        public async void DoesValidatorPreventFromCreatingRecipeWithTooLongDescription()
        {
            var request = new CreateRecipe
            {
                Name = "sample-name",
                Description = string.Concat(Enumerable.Repeat("a", 70000)),
                PictureUrl = "https://example.com/sample-image.png",
                Calories = 1234,
                MealTypes = new[] {MealType.Snack},
                Steps = new[] {"1. Sample first step.", "2. Sample second step."},
                Ingredients = new[] {"Sample first ingredient", "Sample second ingredient"}
            };
            var validator = new CreateRecipeValidator();
    
            var result = await validator.ValidateAsync(request);
    
            result.IsValid.Should().BeFalse();
        }
    
        [Fact]
        public async void DoesValidatorPreventFromCreatingRecipeWithTooLongPictureUrl()
        {
            var request = new CreateRecipe
            {
                Name = "sample-name",
                Description = "sample-description",
                PictureUrl = "https://example.com/cdn?pid=" + string.Concat(Enumerable.Repeat("a", 2200)),
                Calories = 1234,
                MealTypes = new[] {MealType.Snack},
                Steps = new[] {"1. Sample first step.", "2. Sample second step."},
                Ingredients = new[] {"Sample first ingredient", "Sample second ingredient"}
            };
            var validator = new CreateRecipeValidator();
    
            var result = await validator.ValidateAsync(request);
    
            result.IsValid.Should().BeFalse();
        }
    
        [Fact]
        public async void DoesValidatorPreventFromCreatingRecipeWithInvalidPictureUrl()
        {
            var request = new CreateRecipe
            {
                Name = "sample-name",
                Description = "sample-description",
                PictureUrl = "this-is-not-a-uri",
                Calories = 1234,
                MealTypes = new[] {MealType.Snack},
                Steps = new[] {"1. Sample first step.", "2. Sample second step."},
                Ingredients = new[] {"Sample first ingredient", "Sample second ingredient"}
            };
            var validator = new CreateRecipeValidator();
    
            var result = await validator.ValidateAsync(request);
    
            result.IsValid.Should().BeFalse();
        }
    
        [Fact]
        public async void DoesValidatorPreventFromCreatingRecipeWithoutCalories()
        {
            var request = new CreateRecipe
            {
                Name = "sample-name",
                Description = "sample-description",
                PictureUrl = "https://example.com/sample-image.png",
                Calories = 0,
                MealTypes = new[] {MealType.Snack},
                Steps = new[] {"1. Sample first step.", "2. Sample second step."},
                Ingredients = new[] {"Sample first ingredient", "Sample second ingredient"}
            };
            var validator = new CreateRecipeValidator();
    
            var result = await validator.ValidateAsync(request);
    
            result.IsValid.Should().BeFalse();
        }
    
        [Fact]
        public async void DoesValidatorPreventFromCreatingRecipeWithoutSteps()
        {
            var request = new CreateRecipe
            {
                Name = "sample-name",
                Description = "sample-description",
                PictureUrl = "https://example.com/sample-image.png",
                Calories = 1234,
                MealTypes = new[] {MealType.Snack},
                Ingredients = new[] {"Sample first ingredient", "Sample second ingredient"}
            };
            var validator = new CreateRecipeValidator();
    
            var result = await validator.ValidateAsync(request);
    
            result.IsValid.Should().BeFalse();
        }
    
        [Fact]
        public async void DoesValidatorPreventFromCreatingRecipeWithoutMealType()
        {
            var request = new CreateRecipe
            {
                Name = "sample-name",
                Description = "sample-description",
                PictureUrl = "https://example.com/sample-image.png",
                Calories = 1234,
                MealTypes = new MealType[0],
                Steps = new[] {"1. Sample first step.", "2. Sample second step."},
                Ingredients = new[] {"Sample first ingredient", "Sample second ingredient"}
            };
            var validator = new CreateRecipeValidator();
    
            var result = await validator.ValidateAsync(request);
    
            result.IsValid.Should().BeFalse();
        }
    
        [Fact]
        public async void DoesValidatorPreventFromCreatingRecipeWithInvalidStep()
        {
            var request = new CreateRecipe
            {
                Name = "sample-name",
                Description = "sample-description",
                PictureUrl = "https://example.com/sample-image.png",
                Calories = 1234,
                MealTypes = new[] {MealType.Snack},
                Steps = new[] {string.Empty, "2. Sample second step."},
                Ingredients = new[] {"Sample first ingredient", "Sample second ingredient"}
            };
            var validator = new CreateRecipeValidator();
    
            var result = await validator.ValidateAsync(request);
    
            result.IsValid.Should().BeFalse();
        }
        
        [Fact]
        public async void DoesValidatorPreventFromCreatingRecipeWithoutIngredients()
        {
            var request = new CreateRecipe
            {
                Name = "sample-name",
                Description = "sample-description",
                PictureUrl = "https://example.com/sample-image.png",
                Calories = 1234,
                MealTypes = new[] {MealType.Snack},
                Steps = new[] {"Sample first step", "Sample second step"}
            };
            var validator = new CreateRecipeValidator();
    
            var result = await validator.ValidateAsync(request);
    
            result.IsValid.Should().BeFalse();
        }
        
        [Fact]
        public async void DoesValidatorPreventFromCreatingRecipeWithInvalidIngredient()
        {
            var request = new CreateRecipe
            {
                Name = "sample-name",
                Description = "sample-description",
                PictureUrl = "https://example.com/sample-image.png",
                Calories = 1234,
                MealTypes = new[] {MealType.Snack},
                Steps = new[] {"Sample first step", "Sample second step"},
                Ingredients = new[] {string.Empty, "Sample second ingredient"}
            };
            var validator = new CreateRecipeValidator();
    
            var result = await validator.ValidateAsync(request);
    
            result.IsValid.Should().BeFalse();
        }
    }
}