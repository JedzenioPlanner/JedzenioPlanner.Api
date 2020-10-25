using System;
using System.Linq;
using FluentAssertions;
using JedzenioPlanner.Api.Application.Recipes.Commands.UpdateRecipe;
using JedzenioPlanner.Api.Domain.Common.Persistence;
using JedzenioPlanner.Api.Domain.Recipes;
using JedzenioPlanner.Api.Domain.Recipes.Enums;
using Xunit;

namespace JedzenioPlanner.Api.ApplicationTests.Recipes.Commands.Validators
{
    public class UpdateRecipeCommandTests
    {
        [Fact]
        public async void DoesValidatorAllowCorrectRequest()
        {
            var request = new UpdateRecipe
            {
                EntityId = new Guid("447EA0EF-F828-486A-91A9-0EDBC01D0B89"),
                Name = "sample-name",
                Description = "sample-description",
                PictureUrl = "https://example.com/sample-image.png",
                Calories = 1234,
                MealTypes = new[] {MealType.Snack},
                Steps = new[] {"1. Sample first step.", "2. Sample second step."},
                Ingredients = new[] {"Sample first ingredient", "Sample second ingredient"}
            };
            var validator = new UpdateRecipeValidator(MockBuilder.BuildFakeRepository(), MockBuilder.BuildFakeCurrentUserService());
    
            var result = await validator.ValidateAsync(request);
    
            result.IsValid.Should().BeTrue();
        }
    
        [Fact]
        public async void DoesValidatorPreventFromUpdatingNotExistingRecipe()
        {
            var request = new UpdateRecipe
            {
                EntityId = new Guid("1FED1A46-D5FF-4259-B838-08CA7C95F264"),
                Name = "sample-name",
                Description = "sample-description",
                PictureUrl = "https://example.com/sample-image.png",
                Calories = 1234,
                MealTypes = new[] {MealType.Snack},
                Steps = new[] {"1. Sample first step.", "2. Sample second step."},
                Ingredients = new[] {"Sample first ingredient", "Sample second ingredient"}
            };
            var validator = new UpdateRecipeValidator(MockBuilder.BuildFakeRepository(), MockBuilder.BuildFakeCurrentUserService());
    
            var result = await validator.ValidateAsync(request);
    
            result.IsValid.Should().BeFalse();
        }
    
        [Fact]
        public async void DoesValidatorPreventFromUpdatingRecipeWithNoName()
        {
            var request = new UpdateRecipe
            {
                EntityId = new Guid("447EA0EF-F828-486A-91A9-0EDBC01D0B89"),
                Name = string.Empty,
                Description = "sample-description",
                PictureUrl = "https://example.com/sample-image.png",
                Calories = 1234,
                MealTypes = new[] {MealType.Snack},
                Steps = new[] {"1. Sample first step.", "2. Sample second step."},
                Ingredients = new[] {"Sample first ingredient", "Sample second ingredient"}
            };
            var validator = new UpdateRecipeValidator(MockBuilder.BuildFakeRepository(), MockBuilder.BuildFakeCurrentUserService());
    
            var result = await validator.ValidateAsync(request);
    
            result.IsValid.Should().BeFalse();
        }
    
        [Fact]
        public async void DoesValidatorPreventFromUpdatingRecipeWithTooLongName()
        {
            var request = new UpdateRecipe
            {
                EntityId = new Guid("447EA0EF-F828-486A-91A9-0EDBC01D0B89"),
                Name = string.Concat(Enumerable.Repeat("a", 300)),
                Description = "sample-description",
                PictureUrl = "https://example.com/sample-image.png",
                Calories = 1234,
                MealTypes = new[] {MealType.Snack},
                Steps = new[] {"1. Sample first step.", "2. Sample second step."},
                Ingredients = new[] {"Sample first ingredient", "Sample second ingredient"}
            };
            var validator = new UpdateRecipeValidator(MockBuilder.BuildFakeRepository(), MockBuilder.BuildFakeCurrentUserService());
    
            var result = await validator.ValidateAsync(request);
    
            result.IsValid.Should().BeFalse();
        }
        
        [Fact]
        public async void DoesValidatorPreventFromUpdatingRecipeWithNoDescription()
        {
            var request = new UpdateRecipe
            {
                EntityId = new Guid("447EA0EF-F828-486A-91A9-0EDBC01D0B89"),
                Name = "sample-name",
                Description = string.Empty,
                PictureUrl = "https://example.com/sample-image.png",
                Calories = 1234,
                MealTypes = new[] {MealType.Snack},
                Steps = new[] {"1. Sample first step.", "2. Sample second step."},
                Ingredients = new[] {"Sample first ingredient", "Sample second ingredient"}
            };
            var validator = new UpdateRecipeValidator(MockBuilder.BuildFakeRepository(), MockBuilder.BuildFakeCurrentUserService());
    
            var result = await validator.ValidateAsync(request);
    
            result.IsValid.Should().BeFalse();
        }
    
        [Fact]
        public async void DoesValidatorPreventFromUpdatingRecipeWithTooLongDescription()
        {
            var request = new UpdateRecipe
            {
                EntityId = new Guid("447EA0EF-F828-486A-91A9-0EDBC01D0B89"),
                Name = "sample-name",
                Description = string.Concat(Enumerable.Repeat("a", 70000)),
                PictureUrl = "https://example.com/sample-image.png",
                Calories = 1234,
                MealTypes = new[] {MealType.Snack},
                Steps = new[] {"1. Sample first step.", "2. Sample second step."},
                Ingredients = new[] {"Sample first ingredient", "Sample second ingredient"}
            };
            var validator = new UpdateRecipeValidator(MockBuilder.BuildFakeRepository(), MockBuilder.BuildFakeCurrentUserService());
    
            var result = await validator.ValidateAsync(request);
    
            result.IsValid.Should().BeFalse();
        }
    
        [Fact]
        public async void DoesValidatorPreventFromUpdatingRecipeWithTooLongPictureUrl()
        {
            var request = new UpdateRecipe
            {
                EntityId = new Guid("447EA0EF-F828-486A-91A9-0EDBC01D0B89"),
                Name = "sample-name",
                Description = "sample-description",
                PictureUrl = "https://example.com/cdn?pid=" + string.Concat(Enumerable.Repeat("a", 2200)),
                Calories = 1234,
                MealTypes = new[] {MealType.Snack},
                Steps = new[] {"1. Sample first step.", "2. Sample second step."},
                Ingredients = new[] {"Sample first ingredient", "Sample second ingredient"}
            };
            var validator = new UpdateRecipeValidator(MockBuilder.BuildFakeRepository(), MockBuilder.BuildFakeCurrentUserService());
    
            var result = await validator.ValidateAsync(request);
    
            result.IsValid.Should().BeFalse();
        }
    
        [Fact]
        public async void DoesValidatorPreventFromUpdatingRecipeWithInvalidPictureUrl()
        {
            var request = new UpdateRecipe
            {
                EntityId = new Guid("447EA0EF-F828-486A-91A9-0EDBC01D0B89"),
                Name = "sample-name",
                Description = "sample-description",
                PictureUrl = "this-is-not-a-uri",
                Calories = 1234,
                MealTypes = new[] {MealType.Snack},
                Steps = new[] {"1. Sample first step.", "2. Sample second step."},
                Ingredients = new[] {"Sample first ingredient", "Sample second ingredient"}
            };
            var validator = new UpdateRecipeValidator(MockBuilder.BuildFakeRepository(), MockBuilder.BuildFakeCurrentUserService());
    
            var result = await validator.ValidateAsync(request);
    
            result.IsValid.Should().BeFalse();
        }
    
        [Fact]
        public async void DoesValidatorPreventFromUpdatingRecipeWithoutCalories()
        {
            var request = new UpdateRecipe
            {               
                EntityId = new Guid("447EA0EF-F828-486A-91A9-0EDBC01D0B89"),
                Name = "sample-name",
                Description = "sample-description",
                PictureUrl = "https://example.com/sample-image.png",
                Calories = 0,
                MealTypes = new[] {MealType.Snack},
                Steps = new[] {"1. Sample first step.", "2. Sample second step."},
                Ingredients = new[] {"Sample first ingredient", "Sample second ingredient"}
            };
            var validator = new UpdateRecipeValidator(MockBuilder.BuildFakeRepository(), MockBuilder.BuildFakeCurrentUserService());
    
            var result = await validator.ValidateAsync(request);
    
            result.IsValid.Should().BeFalse();
        }
    
        [Fact]
        public async void DoesValidatorPreventFromUpdatingRecipeWithoutSteps()
        {
            var request = new UpdateRecipe
            {
                EntityId = new Guid("447EA0EF-F828-486A-91A9-0EDBC01D0B89"),
                Name = "sample-name",
                Description = "sample-description",
                PictureUrl = "https://example.com/sample-image.png",
                Calories = 1234,
                MealTypes = new[] {MealType.Snack}
            };
            var validator = new UpdateRecipeValidator(MockBuilder.BuildFakeRepository(), MockBuilder.BuildFakeCurrentUserService());
    
            var result = await validator.ValidateAsync(request);
    
            result.IsValid.Should().BeFalse();
        }
    
        [Fact]
        public async void DoesValidatorPreventFromUpdatingRecipeWithoutMealType()
        {
            var request = new UpdateRecipe
            {
                EntityId = new Guid("447EA0EF-F828-486A-91A9-0EDBC01D0B89"),
                Name = "sample-name",
                Description = "sample-description",
                PictureUrl = "https://example.com/sample-image.png",
                Calories = 1234,
                MealTypes = new MealType[0],
                Steps = new[] {"1. Sample first step.", "2. Sample second step."},
                Ingredients = new[] {"Sample first ingredient", "Sample second ingredient"}
            };
            var validator = new UpdateRecipeValidator(MockBuilder.BuildFakeRepository(), MockBuilder.BuildFakeCurrentUserService());
    
            var result = await validator.ValidateAsync(request);
    
            result.IsValid.Should().BeFalse();
        }
    
        [Fact]
        public async void DoesValidatorPreventFromUpdatingRecipeWithInvalidStep()
        {
            var request = new UpdateRecipe
            {
                EntityId = new Guid("447EA0EF-F828-486A-91A9-0EDBC01D0B89"),
                Name = "sample-name",
                Description = "sample-description",
                PictureUrl = "https://example.com/sample-image.png",
                Calories = 1234,
                MealTypes = new[] {MealType.Snack},
                Steps = new[] {""},
                Ingredients = new[] {"Sample first ingredient", "Sample second ingredient"}
            };
            var validator = new UpdateRecipeValidator(MockBuilder.BuildFakeRepository(), MockBuilder.BuildFakeCurrentUserService());
    
            var result = await validator.ValidateAsync(request);
    
            result.IsValid.Should().BeFalse();
        }

        [Fact]
        public async void DoesValidatorPreventFromCreatingRecipeWithoutIngredients()
        {
            var request = new UpdateRecipe
            {
                EntityId = new Guid("447EA0EF-F828-486A-91A9-0EDBC01D0B89"),
                Name = "sample-name",
                Description = "sample-description",
                PictureUrl = "https://example.com/sample-image.png",
                Calories = 1234,
                MealTypes = new[] {MealType.Snack},
                Steps = new[] {"Sample first step", "Sample second step"}
            };
            var validator = new UpdateRecipeValidator(MockBuilder.BuildFakeRepository(), MockBuilder.BuildFakeCurrentUserService());
    
            var result = await validator.ValidateAsync(request);
    
            result.IsValid.Should().BeFalse();
        }
        
        [Fact]
        public async void DoesValidatorPreventFromCreatingRecipeWithInvalidIngredient()
        {
            var request = new UpdateRecipe
            {
                EntityId = new Guid("447EA0EF-F828-486A-91A9-0EDBC01D0B89"),
                Name = "sample-name",
                Description = "sample-description",
                PictureUrl = "https://example.com/sample-image.png",
                Calories = 1234,
                MealTypes = new[] {MealType.Snack},
                Steps = new[] {"Sample first step", "Sample second step"},
                Ingredients = new[] {string.Empty, "Sample second ingredient"}
            };
            var validator = new UpdateRecipeValidator(MockBuilder.BuildFakeRepository(), MockBuilder.BuildFakeCurrentUserService());
    
            var result = await validator.ValidateAsync(request);
    
            result.IsValid.Should().BeFalse();
        }
    }
}