using Microsoft.WindowsAzure.Storage.Table;

namespace Recipeasy_API.Entities
{
    public class RecipeEntity : TableEntity
    {
        public string RecipeName { get; set; }

        public string Notes { get; set; }
    }
}
