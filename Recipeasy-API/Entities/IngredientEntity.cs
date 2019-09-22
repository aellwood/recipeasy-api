using Microsoft.WindowsAzure.Storage.Table;

namespace Recipeasy_API.Entities
{
    public class IngredientEntity : TableEntity
    {
        public IngredientEntity(string recipeGuid, string ingredientGuid)
        {
            PartitionKey = recipeGuid;
            RowKey = ingredientGuid;
        }

        public IngredientEntity() { }

        public string IngredientName { get; set; }

        public int Quantity { get; set; }
    }
}
