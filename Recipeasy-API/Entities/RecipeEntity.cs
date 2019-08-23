using Microsoft.WindowsAzure.Storage.Table;

namespace Recipeasy_API.Entities
{
    public class RecipeEntity : TableEntity
    {
        public RecipeEntity(string userName, string recipeName)
        {
            this.PartitionKey = userName;
            this.RowKey = recipeName;
        }

        public string Ingredients { get; set; }

        public string Notes { get; set; }

    }
}
