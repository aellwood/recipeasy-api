using Microsoft.WindowsAzure.Storage.Table;

namespace Recipeasy.Api.Entities
{
    public class RecipeEntity : TableEntity
    {
        public string RecipeName { get; set; }

        public string Notes { get; set; }
    }
}
