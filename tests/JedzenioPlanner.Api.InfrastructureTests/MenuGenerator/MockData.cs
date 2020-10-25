using System;
using System.Collections.Generic;
using JedzenioPlanner.Api.Domain.Common.SubElements;
using JedzenioPlanner.Api.Domain.Recipes;
using JedzenioPlanner.Api.Domain.Recipes.Enums;

namespace JedzenioPlanner.Api.InfrastructureTests.MenuGenerator
{
    internal static class MockData
    {
        public static Recipe SampleBreakfast { get; } = new Recipe
        {
            Id = new Guid("36B63FE1-4E5A-458D-ADF6-8D02A1800BC7"),
            Metadata = new Metadata
            {
                Version = 1,
                Removed = false,
                Creation = new Creation
                {
                    AuthorId = "CEF1AF73-5CD6-4226-82F9-E4F070F92B6C",
                    Published = new DateTime(2010, 1, 1)
                },
                Updates = new List<Update>()
            },
            Name = "Sample breakfast.",
            Description = "Sample description.",
            PictureUrl = "https://example.com/sample-picture.png",
            Calories = 701,
            Ingredients = new[] {"1. Sample ingredient"},
            Steps = new[] {"1. Sample step."},
            MealTypes = new[] {MealType.Breakfast}
        };

        public static Recipe SampleDinner { get; } = new Recipe
        {
            Id = new Guid("DD896F73-6AF7-4A79-A2C0-0576BD0506E1"),
            Metadata = new Metadata
            {
                Version = 1,
                Removed = false,
                Creation = new Creation
                {
                    AuthorId = "CEF1AF73-5CD6-4226-82F9-E4F070F92B6C",
                    Published = new DateTime(2010, 1, 1)
                },
                Updates = new List<Update>()
            },
            Name = "Sample dinner.",
            Description = "Sample description.",
            PictureUrl = "https://example.com/sample-picture.png",
            Calories = 1020,
            Ingredients = new[] {"1. Sample ingredient"},
            Steps = new[] {"1. Sample step."},
            MealTypes = new[] {MealType.Dinner}
        };

        public static Recipe SampleLunch { get; } = new Recipe
        {
            Id = new Guid("93877BB3-2330-4425-9B43-7955EDF2EFCE"),
            Metadata = new Metadata
            {
                Version = 1,
                Removed = false,
                Creation = new Creation
                {
                    AuthorId = "CEF1AF73-5CD6-4226-82F9-E4F070F92B6C",
                    Published = new DateTime(2010, 1, 1)
                },
                Updates = new List<Update>()
            },
            Name = "Sample lunch.",
            Description = "Sample description.",
            PictureUrl = "https://example.com/sample-picture.png",
            Calories = 828,
            Ingredients = new[] {"1. Sample ingredient"},
            Steps = new[] {"1. Sample step."},
            MealTypes = new[] {MealType.Lunch}
        };

        public static Recipe SampleSnack { get; } = new Recipe
        {
            Id = new Guid("3323DD0B-D706-4971-BDB2-12704E4DE370"),
            Metadata = new Metadata
            {
                Version = 1,
                Removed = false,
                Creation = new Creation
                {
                    AuthorId = "CEF1AF73-5CD6-4226-82F9-E4F070F92B6C",
                    Published = new DateTime(2010, 1, 1)
                },
                Updates = new List<Update>()
            },
            Name = "Sample snack.",
            Description = "Sample description.",
            PictureUrl = "https://example.com/sample-picture.png",
            Calories = 350,
            Ingredients = new[] {"1. Sample ingredient"},
            Steps = new[] {"1. Sample step."},
            MealTypes = new[] {MealType.Snack}
        };
    }
}