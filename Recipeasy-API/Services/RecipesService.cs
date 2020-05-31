using Recipeasy_API.Entities;
using Recipeasy_API.Interfaces.Services;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using Recipeasy_API.Models;
using System.Linq;

namespace Recipeasy_API.Services
{
    public class RecipesService : IRecipesService
{
        private readonly IDatabaseService databaseService;

        public RecipesService(IDatabaseService databaseService)
        {
            this.databaseService = databaseService;
        }

        public async Task<Recipe> AddRecipe(Recipe recipePayload, string email)
        {
            var recipeGuid = Guid.NewGuid().ToString();
            var recipeEntity = new RecipeEntity(email, recipeGuid)
            {
                RecipeName = recipePayload.RecipeName,
                Notes = recipePayload.Notes
            };

            await databaseService.Add(recipeEntity);

            recipePayload.Ingredients.ForEach(async x => 
                await databaseService.Add(
                    new IngredientEntity(
                        recipeGuid, 
                        Guid.NewGuid().ToString())
                    {
                        IngredientName = x.IngredientName,
                        Quantity = x.Quantity
                    }));

            //TODO: Create method to only retrieve one recipe using email and recipeId
            var recipes = await GetRecipes(email);

            return recipes.FirstOrDefault(x => x.RecipeId == recipeGuid);

        }

        public async Task<List<Recipe>> GetRecipes(string email)
        {
            var recipeEntities = await databaseService.Get<RecipeEntity>(email);

            return recipeEntities.Select(x => new Recipe
            {
                RecipeId = x.RowKey,
                RecipeName = x.RecipeName,
                Notes = x.Notes,
                Ingredients = databaseService.Get<IngredientEntity>(x.RowKey).Result.Select(y => new Ingredient
                {
                    IngredientId = y.RowKey,
                    IngredientName = y.IngredientName,
                    Quantity = y.Quantity
                }).ToList()
            }).ToList();
        }

        public async Task DeleteRecipe(string email, string recipeId)
        {
            await databaseService.Delete<RecipeEntity>(email, recipeId);
        }
    }
}
