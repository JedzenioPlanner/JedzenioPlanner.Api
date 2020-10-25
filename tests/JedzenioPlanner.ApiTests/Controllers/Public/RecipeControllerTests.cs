using System;
using System.Threading;
using DeepEqual.Syntax;
using JedzenioPlanner.Api.Application.Recipes.Commands.CreateRecipe;
using JedzenioPlanner.Api.Application.Recipes.Commands.DeleteRecipe;
using JedzenioPlanner.Api.Application.Recipes.Commands.UpdateRecipe;
using JedzenioPlanner.Api.Application.Recipes.Queries.GetAllRecipes;
using JedzenioPlanner.Api.Application.Recipes.Queries.GetRecipeById;
using JedzenioPlanner.Api.Controllers.Public;
using JedzenioPlanner.Api.Domain.Recipes.Enums;
using JedzenioPlanner.Api.Models.Recipe;
using MediatR;
using Moq;
using Xunit;

namespace JedzenioPlanner.ApiTests.Controllers.Public
{
    public class RecipeControllerTests
    {
        [Fact]
        public async void ShouldGetAllCorrectly()
        {
            var mediatorMock = new Mock<IMediator>();
            var controller = new RecipeController(mediatorMock.Object);

            await controller.GetAll();

            mediatorMock.Verify(x => x.Send(It.IsAny<GetRecipes>(), It.IsAny<CancellationToken>()), Times.Once);
            mediatorMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldGetByIdCorrectly()
        {
            var mediatorMock = new Mock<IMediator>();
            var controller = new RecipeController(mediatorMock.Object);

            await controller.GetById(new Guid("F3583BB5-DCD4-4161-990F-CF97D7156B97"));

            mediatorMock.Verify(x => x.Send(It.Is<GetRecipeById>(y => y.Id == new Guid("F3583BB5-DCD4-4161-990F-CF97D7156B97") && y.StripMetadata == true), It.IsAny<CancellationToken>()), Times.Once);
            mediatorMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldAddCorrectly()
        {
            var mediatorMock = new Mock<IMediator>();
            var controller = new RecipeController(mediatorMock.Object);

            var model = new CreateRecipeModel
            {
                Name = "sample-name",
                Description = "sample-description",
                PictureUrl = "https://example.com/sample-picture.png",
                Calories = 1234,
                MealTypes = new[] {MealType.Snack},
                Steps = new[] {"sample-step"},
                Ingredients = new[] {"sample-ingredient"}
            };
            await controller.Add(model);

            mediatorMock.Verify(x => 
                x.Send(
                    It.Is<CreateRecipe>(y => y.IsDeepEqual(model.Map())), 
                    It.IsAny<CancellationToken>()
                    ), Times.Once);
            mediatorMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldUpdateCorrectly()
        {
            var mediatorMock = new Mock<IMediator>();
            var controller = new RecipeController(mediatorMock.Object);

            var model = new UpdateRecipeModel
            {
                Name = "sample-name",
                Description = "sample-description",
                PictureUrl = "https://example.com/sample-picture.png",
                Calories = 1234,
                MealTypes = new[] {MealType.Snack},
                Steps = new[]{"sample-step"},
                Ingredients = new[]{"sample-ingredient"}
            };
            await controller.Update(new Guid("F3583BB5-DCD4-4161-990F-CF97D7156B97"), model);

            mediatorMock.Verify(
                x => x.Send(
                    It.Is<UpdateRecipe>(y 
                        => y.WithDeepEqual(model.Map(new Guid("F3583BB5-DCD4-4161-990F-CF97D7156B97")))
                            .IgnoreSourceProperty(z => z.EntityId)
                            .Compare()
                        ),
                    It.IsAny<CancellationToken>()), Times.Once);

            mediatorMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldDeleteCorrectly()
        {
            var mediatorMock = new Mock<IMediator>();
            var controller = new RecipeController(mediatorMock.Object);

            await controller.Delete(new Guid("F3583BB5-DCD4-4161-990F-CF97D7156B97"));

            mediatorMock.Verify(x => x.Send(It.Is<DeleteRecipe>(y => y.EntityId == new Guid("F3583BB5-DCD4-4161-990F-CF97D7156B97")), It.IsAny<CancellationToken>()), Times.Once);
            mediatorMock.VerifyNoOtherCalls();
        }
    }
} 