using Newtonsoft.Json;
using System.Collections.Generic;

namespace Recipeasy.Api.Models
{
    public class Recipe
    {
        [JsonProperty("recipeId")]
        public string RecipeId { get; set; }

        [JsonProperty("recipeName")]
        public string RecipeName { get; set; }

        [JsonProperty("ingredients")]
        public List<Ingredient> Ingredients { get; set; }

        [JsonProperty("notes")]
        public string Notes { get; set; }
    }
}
