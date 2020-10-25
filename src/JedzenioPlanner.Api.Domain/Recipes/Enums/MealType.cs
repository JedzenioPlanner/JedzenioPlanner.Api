using System.Text.Json.Serialization;

namespace JedzenioPlanner.Api.Domain.Recipes.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum MealType
    {
        Breakfast,
        Lunch,
        Dinner,
        Snack
    }
}