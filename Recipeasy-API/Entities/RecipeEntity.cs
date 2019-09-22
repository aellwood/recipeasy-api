using Microsoft.WindowsAzure.Storage.Table;

namespace Recipeasy_API.Entities
{
    public class RecipeEntity : TableEntity
    {
        public RecipeEntity(string userName, string recipeId)
        {
            this.PartitionKey = userName;
            this.RowKey = recipeId;
        }

        public RecipeEntity() { }

        public string RecipeName { get; set; }

        public string Ingredients { get; set; }

        public string Notes { get; set; }
    }
}
