using Microsoft.WindowsAzure.Storage.Table;

namespace Recipeasy_API.Entities
{
    public class IngredientEntity : TableEntity
    {
        public string IngredientName { get; set; }

        public int Quantity { get; set; }
    }
}
