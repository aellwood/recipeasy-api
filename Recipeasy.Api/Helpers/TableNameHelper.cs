using System;

namespace Recipeasy_API.Helpers
{
    public static class TableNameHelper
    {
        public static string GetTableName(string entityName)
        {
            switch (entityName)
            {
                case "RecipeEntity":
                    return "recipeTable";
                case "IngredientEntity":
                    return "ingredientTable";
                default:
                    throw new ArgumentOutOfRangeException($"{entityName} is an unknown entity type - cannot find associated table name.");
            }
        }
    }
}
