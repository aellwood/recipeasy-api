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

        public async Task<Recipe> AddRecipe(Recipe recipePayload, string userId)
        {
            var recipeGuid = Guid.NewGuid().ToString();
            var recipeEntity = new RecipeEntity
            {
                PartitionKey = userId,
                RowKey = recipeGuid,
                RecipeName = recipePayload.RecipeName,
                Notes = recipePayload.Notes
            };

            await databaseService.Add(recipeEntity);

            recipePayload.Ingredients.ForEach(async x => 
                await databaseService.Add(
                    new IngredientEntity
                    {
                        PartitionKey = recipeGuid,
                        RowKey = Guid.NewGuid().ToString(),
                        IngredientName = x.IngredientName,
                        Quantity = x.Quantity
                    }));

            return await GetRecipe(userId, recipeGuid);

        }

        public async Task<Recipe> GetRecipe(string userId, string recipeId)
        {
            var recipeEntity = await databaseService.Get<RecipeEntity>(userId, recipeId);

            return new Recipe
            {
                RecipeId = recipeEntity.RowKey,
                RecipeName = recipeEntity.RecipeName,
                Notes = recipeEntity.Notes,
                Ingredients = databaseService.Get<IngredientEntity>(recipeId).Result.Select(y => new Ingredient
                {
                    IngredientId = y.RowKey,
                    IngredientName = y.IngredientName,
                    Quantity = y.Quantity
                }).ToList()
            };
        }

        public async Task<List<Recipe>> GetRecipes(string userId)
        {
            var recipeEntities = await databaseService.Get<RecipeEntity>(userId);

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

        public async Task DeleteRecipe(string userId, string recipeId)
        {
            var recipe = await GetRecipe(userId, recipeId);

            await databaseService.Delete<RecipeEntity>(userId, recipeId);

            recipe.Ingredients.ForEach(async x => await databaseService.Delete<IngredientEntity>(recipeId, x.IngredientId));
        }
    }
}
