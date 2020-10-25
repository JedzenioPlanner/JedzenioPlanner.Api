using System;
using System.Collections.Generic;
using JedzenioPlanner.Api.Domain.Common.SubElements;
using JedzenioPlanner.Api.Domain.Recipes;
using JedzenioPlanner.Api.Domain.Recipes.Enums;
using Newtonsoft.Json;

namespace JedzenioPlanner.Api.Infrastructure.Persistence.Repositories.Recipes
{
    public class RecipeDatabaseEntity
    {
        public Guid Id { get; set; }
        public double Version { get; set; }
        public bool Removed { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PictureUrl { get; set; }
        public int Calories { get; set; }
        /// <summary>
        /// Json
        /// </summary>
        public string Ingredients { get; set; }
        /// <summary>
        /// Json
        /// </summary>
        public string Steps { get; set; }
        /// <summary>
        /// Json
        /// </summary>
        public string MealTypes { get; set; }
        public string CreatorId { get; set; }
        public DateTime CreationDateTime { get; set; }
        /// <summary>
        /// Json
        /// </summary>
        public string Updates { get; set; }
        
        public Recipe ToRecipe()
        {
            return new Recipe
            {
                Name = Name,
                Description = Description,
                PictureUrl = PictureUrl,
                Calories = Calories,
                Ingredients = JsonConvert.DeserializeObject<IEnumerable<string>>(Ingredients),
                Steps = JsonConvert.DeserializeObject<IEnumerable<string>>(Steps),
                MealTypes = JsonConvert.DeserializeObject<IEnumerable<MealType>>(MealTypes),
                Id = Id,
                Metadata = new Metadata
                {
                    Creation = new Creation
                    {
                        AuthorId = CreatorId,
                        Published = CreationDateTime
                    },
                    Removed = Removed,
                    Version = Version,
                    Updates = JsonConvert.DeserializeObject<IEnumerable<Update>>(Updates)
                }
            };
        }
        
        public static RecipeDatabaseEntity FromRecipe(Recipe recipe)
        {
            return new RecipeDatabaseEntity
            {
                Id = recipe.Id,
                Name = recipe.Name,
                Description = recipe.Description,
                PictureUrl = recipe.PictureUrl,
                Calories = recipe.Calories,
                Ingredients = JsonConvert.SerializeObject(recipe.Ingredients),
                Steps = JsonConvert.SerializeObject(recipe.Steps),
                MealTypes = JsonConvert.SerializeObject(recipe.MealTypes),
                CreatorId = recipe.Metadata.Creation.AuthorId,
                CreationDateTime = recipe.Metadata.Creation.Published,
                Removed = recipe.Metadata.Removed,
                Version = recipe.Metadata.Version,
                Updates = JsonConvert.SerializeObject(recipe.Metadata.Updates)
            };
        }
    }
}