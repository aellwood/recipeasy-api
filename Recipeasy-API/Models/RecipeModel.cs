using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Recipeasy_API.Payload
{
    public class RecipeModel
    {
        [JsonProperty("recipeId")]
        public string RecipeId { get; set; }

        [JsonProperty("recipeName")]
        public string RecipeName { get; set; }

        [JsonProperty("ingredients")]
        public JArray Ingredients { get; set; }

        [JsonProperty("notes")]
        public string Notes { get; set; }
    }
}
