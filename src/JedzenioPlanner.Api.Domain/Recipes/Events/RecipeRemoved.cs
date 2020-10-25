using JedzenioPlanner.Api.Domain.Common;

namespace JedzenioPlanner.Api.Domain.Recipes.Events
{
    public class RecipeRemoved : Event<Recipe>
    {
        protected internal override Recipe Apply(Recipe entity)
        {
            entity.Metadata.Removed = true;
            return entity;
        }
    }
}