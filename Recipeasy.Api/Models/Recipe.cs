using System;
using System.Collections.Generic;

namespace Recipeasy.Api.Models
{
    public class Recipe
    {
        public string RecipeId { get; set; }
        public string RecipeName { get; set; }
        public List<Ingredient> Ingredients { get; set; }
        public string Notes { get; set; }
        public DateTime DateAdded { get; set; }
        public string UserId { get; set; }
    }
}
