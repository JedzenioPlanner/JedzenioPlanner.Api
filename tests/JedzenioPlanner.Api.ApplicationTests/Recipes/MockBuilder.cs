using System;
using System.Linq;
using System.Threading.Tasks;
using JedzenioPlanner.Api.Application.Common.Interfaces;
using JedzenioPlanner.Api.Application.Recipes.Commands.CreateRecipe;
using JedzenioPlanner.Api.Application.Recipes.Commands.DeleteRecipe;
using JedzenioPlanner.Api.Application.Recipes.Commands.RestoreRecipe;
using JedzenioPlanner.Api.Application.Recipes.Commands.UpdateRecipe;
using JedzenioPlanner.Api.Application.Recipes.Common;
using JedzenioPlanner.Api.Domain.Common.Persistence;
using JedzenioPlanner.Api.Domain.Common.SubElements;
using JedzenioPlanner.Api.Domain.Recipes;
using JedzenioPlanner.Api.Domain.Recipes.Enums;
using JedzenioPlanner.Api.Domain.Recipes.Events;
using Moq;

namespace JedzenioPlanner.Api.ApplicationTests.Recipes
{
    public static class MockBuilder
    {
        public static IAggregatesRepository<Recipe> BuildFakeRepository()
        {
            var mock = new Mock<IAggregatesRepository<Recipe>>();
    
            mock
                .Setup(x => x.GetById(It.Is<Guid>(y => y.Equals(new Guid("447EA0EF-F828-486A-91A9-0EDBC01D0B89")))))
                .Returns(Task.FromResult(new Recipe
                {
                    Id = new Guid("447EA0EF-F828-486A-91A9-0EDBC01D0B89"),
                    Metadata = new Metadata
                    {
                        Version = 0,
                        Removed = false,
                        Creation = new Creation {AuthorId = "9E09950B-47DE-4BAB-AA79-C29414312ECB", Published = new DateTime(2010, 1, 1) },
                        Updates = new Update[0]
                    },
                    Name = "sample-name",
                    Description = "sample-description",
                    PictureUrl = "https://example.com/sample-image.png",
                    Calories = 1234,
                    Ingredients = new[] {"Sample first ingredient", "Sample second ingredient"},
                    MealTypes = new[] {MealType.Snack},
                    Steps = new[] {"1. Sample first step.", "2. Sample second step."}
                }));
            
            mock
                .Setup(x => x.GetAll())
                .Returns(Task.FromResult(new[]
                {
                    new Recipe
                    {
                        Id = new Guid("447EA0EF-F828-486A-91A9-0EDBC01D0B89"),
                        Metadata = new Metadata
                        {
                            Version = 0,
                            Removed = false,
                            Creation = new Creation {AuthorId = "9E09950B-47DE-4BAB-AA79-C29414312ECB", Published = new DateTime(2010, 1, 1) },
                            Updates = new Update[0]
                        },
                        Name = "sample-name",
                        Description = "sample-description",
                        PictureUrl = "https://example.com/sample-image.png",
                        Calories = 1234,
                        MealTypes = new[] {MealType.Snack},
                        Steps = new[] {"1. Sample first step.", "2. Sample second step."},
                        Ingredients = new[] {"Sample first ingredient", "Sample second ingredient"}
                    },
                    new Recipe
                    {
                        Id = new Guid("B8DBCACE-35A9-4D48-8A71-827E37D782A8"),
                        Metadata = new Metadata
                        {
                            Version = 0,
                            Removed = false,
                            Creation = new Creation {AuthorId = "9E09950B-47DE-4BAB-AA79-C29414312ECB", Published = new DateTime(2010, 1, 1) },
                            Updates = new Update[0]
                        },
                        Name = "sample-name-2",
                        Description = "sample-description-2",
                        PictureUrl = "https://example.com/sample-image-2.png",
                        Calories = 4321,
                        MealTypes = new[] {MealType.Lunch},
                        Steps = new[] {"1. Sample first step.", "2. Sample second step."},
                        Ingredients = new[] {"Sample first ingredient", "Sample second ingredient"}
                    }
                }.AsEnumerable()));
            
            return mock.Object;
        }
                
        public static RecipeEventsMapper BuildFakeRecipeEventsMapper()
        {
            var mock = new Mock<RecipeEventsMapper>();
    
            mock
                .Setup(x => x.Map(It.IsAny<CreateRecipe>(), It.IsAny<Guid>(), It.IsAny<DateTime>(), It.IsAny<string>()))
                .Returns((CreateRecipe dto, Guid entityId, DateTime publishTime, string userId) =>
                {
                    return new RecipeCreated
                    {
                        Published = new DateTime(2010,1,1),
                        EntityId = new Guid("7ADC9EF0-6A2A-4DE5-9C27-D4C2A4D9D5B6"),
                        AuthorId = "9E09950B-47DE-4BAB-AA79-C29414312ECB",
                        Name = "sample-name",
                        Description = "sample-description",
                        PictureUrl = "https://example.com/sample-image.png",
                        Calories = 1234,
                        Steps = new[] {"1. Sample first step.", "2. Sample second step."},
                        Ingredients = new[] {"Sample first ingredient", "Sample second ingredient"},
                        MealTypes = new[] {MealType.Snack}
                    };
                });
    
            mock
                .Setup(x => x.Map(It.IsAny<DeleteRecipe>(), It.IsAny<DateTime>(), It.IsAny<string>()))
                .Returns((DeleteRecipe dto, DateTime publishTime, string userId) =>
                {
                    return new RecipeRemoved
                    {
                        EntityId = new Guid("62CB0EE2-0CF7-4C72-9C51-80A90E8E420E"),
                        Published = new DateTime(2010,1,1),
                        AuthorId = "9E09950B-47DE-4BAB-AA79-C29414312ECB"
                    };
                });
    
            mock
                .Setup(x => x.Map(It.IsAny<UpdateRecipe>(), It.IsAny<DateTime>(), It.IsAny<string>()))
                .Returns((UpdateRecipe dto, DateTime publishTime, string userId) =>
                {
                    return new RecipeUpdated
                    {
                        Name = "sample-name",
                        EntityId = new Guid("051F13A0-4796-4B1C-9797-EC99F08CF25E"),
                        Published = new DateTime(2010,1,1),
                        AuthorId = "9E09950B-47DE-4BAB-AA79-C29414312ECB",
                        Description = "sample-description",
                        PictureUrl = "https://example.com/sample-picture.png",
                        Calories = 1234,
                        Steps = new[] {"1. Sample first step.", "2. Sample second step."},
                        Ingredients = new[] {"Sample first ingredient", "Sample second ingredient"},
                        MealTypes = new[] {MealType.Snack}
                    };
                });
            
            mock
                .Setup(x => x.Map(It.IsAny<RestoreRecipe>(), It.IsAny<DateTime>(), It.IsAny<string>()))
                .Returns((RestoreRecipe dto, DateTime publishTime, string userId) =>
                {
                    return new RecipeRestored
                    {
                        EntityId = Guid.Parse("21552e39-3c5a-42e6-a116-e9f54643e57c"),
                        Published = new DateTime(2010,1,1),
                        AuthorId = "edb4a387-260e-43c1-aed8-eec4dbd0fc31"
                    };
                });
    
            return mock.Object;
        }
    
        public static IDateTimeService BuildFakeDateTimeService()
        {
            var dateTimeMock = new Mock<IDateTimeService>();
            dateTimeMock.Setup(x => x.GetCurrentTime()).Returns(new DateTime(2010,1,1));
            return dateTimeMock.Object;
        }
    
        public static ICurrentUserService BuildFakeCurrentUserService()
        {
            var currentUserMock = new Mock<ICurrentUserService>();
            currentUserMock.Setup(x => x.GetUserId()).Returns("9E09950B-47DE-4BAB-AA79-C29414312ECB");
            return currentUserMock.Object;
        }
        
        public static Mock<IFileStore> BuildFakeFileStore()
        {
            var fileStoreMock = new Mock<IFileStore>();
            fileStoreMock
                .Setup(x => x.ReadFile(new Guid("F7D85930-4CF4-4B70-A686-18F94ABF4302")))
                .Returns(Task.FromResult(Convert.FromBase64String("iVBORw0KGgoAAAANSUhEUgAAAAoAAAAKCAIAAAACUFjqAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAAOSURBVChTYxgFJAMGBgABNgABY8OiGAAAAABJRU5ErkJggg==")));

            return fileStoreMock;
        }
    }
}