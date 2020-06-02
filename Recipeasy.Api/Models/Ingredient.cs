using Newtonsoft.Json;

namespace Recipeasy.Api.Models
{
    public class Ingredient
    {
        [JsonProperty("ingredientId")]
        public string IngredientId { get; set; }

        [JsonProperty("ingredientName")]
        public string IngredientName { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }
    }
}
