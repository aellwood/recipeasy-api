using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Recipeasy_API.Payload
{
    public class RecipePayload
    {
        [JsonProperty("recipeName")]
        public string RecipeName { get; set; }

        [JsonProperty("ingredients")]
        public JArray Ingredients { get; set; }

        [JsonProperty("notes")]
        public string Notes { get; set; }
    }
}
