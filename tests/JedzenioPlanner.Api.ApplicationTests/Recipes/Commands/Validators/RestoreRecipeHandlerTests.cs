using System;
using FluentAssertions;
using JedzenioPlanner.Api.Application.Recipes.Commands.DeleteRecipe;
using JedzenioPlanner.Api.Application.Recipes.Commands.RestoreRecipe;
using JedzenioPlanner.Api.Domain.Common.Persistence;
using Xunit;

namespace JedzenioPlanner.Api.ApplicationTests.Recipes.Commands.Validators
{
    public class RestoreRecipeCommandTests
    {
        [Fact]
        public async void DoesValidatorAllowCorrectRequest()
        {
            var request = new RestoreRecipe {EntityId = new Guid("0F04E4D4-F9B8-45E8-8661-496730E8B455")};
            var validator = new RestoreRecipeValidator(MockBuilder.BuildFakeRepository());

            var result = await validator.ValidateAsync(request);

            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public async void DoesValidatorPreventFromRequestWithEmptyId()
        {
            var request = new RestoreRecipe();
            var validator = new RestoreRecipeValidator(MockBuilder.BuildFakeRepository());

            var result = await validator.ValidateAsync(request);

            result.IsValid.Should().BeFalse();
        }

        [Fact]
        public async void DoesValidatorPreventFromRequestWithExistingId()
        {
            var request = new RestoreRecipe{EntityId = new Guid("447EA0EF-F828-486A-91A9-0EDBC01D0B89")};
            var validator = new RestoreRecipeValidator(MockBuilder.BuildFakeRepository());

            var result = await validator.ValidateAsync(request);

            result.IsValid.Should().BeFalse();
        }
    }
}