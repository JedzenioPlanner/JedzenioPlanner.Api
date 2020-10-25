using System;
using System.Threading.Tasks;
using FluentAssertions;
using JedzenioPlanner.Api.Application.Recipes.Commands.DeleteRecipe;
using JedzenioPlanner.Api.Domain.Common.Persistence;
using JedzenioPlanner.Api.Domain.Common.SubElements;
using JedzenioPlanner.Api.Domain.Recipes;
using JedzenioPlanner.Api.Domain.Recipes.Enums;
using Moq;
using Xunit;

namespace JedzenioPlanner.Api.ApplicationTests.Recipes.Commands.Validators
{
    public class DeleteRecipeCommandTests
    {
        [Fact]
        public async void DoesValidatorAllowCorrectRequest()
        {
            var request = new DeleteRecipe(new Guid("447EA0EF-F828-486A-91A9-0EDBC01D0B89"));
            var validator = new DeleteRecipeValidator(MockBuilder.BuildFakeRepository(), MockBuilder.BuildFakeCurrentUserService());
    
            var result = await validator.ValidateAsync(request);
            result.IsValid.Should().BeTrue();
        }
    
        [Fact]
        public async void DoesValidatorPreventFromDeletingNotExistingRecipe()
        {
            var request = new DeleteRecipe(new Guid("E9AFD20D-C83C-493B-827B-2220433E4D5E"));
            var validator = new DeleteRecipeValidator(MockBuilder.BuildFakeRepository(), MockBuilder.BuildFakeCurrentUserService());
    
            var result = await validator.ValidateAsync(request);
            result.IsValid.Should().BeFalse();
        }
    }
}