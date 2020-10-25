using FluentAssertions;
using JedzenioPlanner.Api.Domain.Recipes.Enums;
using JedzenioPlanner.Api.Models.Recipe;
using Xunit;

namespace JedzenioPlanner.ApiTests.Models.Recipe
{
    public class CreateRecipeModelMappingTests
    {
        [Fact]
        public void ShouldMapCorrectly()
        {
            var model = new CreateRecipeModel
            {
                Name = "sample-name",
                Description = "sample-description",
                PictureUrl = "https://example.com/sample-picture.png",
                Calories = 1234,
                Steps = new[]{"sample-step"},
                Ingredients = new[]{"sample-ingredient"}
            };

            var command = model.Map();

            command.Name.Should().BeEquivalentTo("sample-name");
            command.Description.Should().BeEquivalentTo("sample-description");
            command.PictureUrl.Should().BeEquivalentTo("https://example.com/sample-picture.png");
            command.Calories.Should().Be(1234);
            command.Steps
                .Should().HaveCount(1)
                .And.ContainSingle(x => x == "sample-step");
            command.Ingredients
                .Should().HaveCount(1)
                .And.ContainSingle(x => x == "sample-ingredient");
        }

        [Fact]
        public void ShouldMapCorrectlyWithoutPictureUrl()
        {
            var model = new CreateRecipeModel
            {
                Name = "sample-name",
                Description = "sample-description",
                Calories = 1234,
                Steps = new[]{"sample-step"},
                Ingredients = new[]{"sample-ingredient"}
            };

            var command = model.Map();

            command.Name.Should().BeEquivalentTo("sample-name");
            command.Description.Should().BeEquivalentTo("sample-description");
            command.PictureUrl.Should().BeNullOrEmpty();
            command.Calories.Should().Be(1234);
            command.Steps
                .Should().HaveCount(1)
                .And.ContainSingle(x => x == "sample-step");
            command.Ingredients
                .Should().HaveCount(1)
                .And.ContainSingle(x => x == "sample-ingredient");
        }
    }
} 