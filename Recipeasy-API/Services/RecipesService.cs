using Recipeasy_API.Entities;
using Recipeasy_API.Interfaces.Services;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using Recipeasy_API.Models;
using AutoMapper;

namespace Recipeasy_API.Services
{
    public class RecipesService : IRecipesService
{
        private readonly IDatabaseService databaseService;
        private readonly IMapper mapper;

        public RecipesService(IDatabaseService databaseService, IMapper mapper)
        {
            this.databaseService = databaseService;
            this.mapper = mapper;
        }

        public async Task<Recipe> AddRecipe(Recipe recipePayload, string userId)
        {
            var recipeId = Guid.NewGuid().ToString();
            var recipeEntity = new RecipeEntity
            {
                PartitionKey = userId,
                RowKey = recipeId,
                RecipeName = recipePayload.RecipeName,
                Notes = recipePayload.Notes
            };

            await databaseService.Add(recipeEntity);

            recipePayload.Ingredients.ForEach(async x => 
                await databaseService.Add(
                    new IngredientEntity
                    {
                        PartitionKey = recipeId,
                        RowKey = Guid.NewGuid().ToString(),
                        IngredientName = x.IngredientName,
                        Quantity = x.Quantity
                    }));

            return await GetRecipe(userId, recipeId);

        }

        public async Task<Recipe> GetRecipe(string userId, string recipeId)
        {
            var recipeEntity = await databaseService.Get<RecipeEntity>(userId, recipeId);
            var recipe = mapper.Map<Recipe>(recipeEntity);

            if (recipe != null)
            {
                var ingredientEntity = await databaseService.Get<IngredientEntity>(recipeId);
                recipe.Ingredients = mapper.Map<List<Ingredient>>(ingredientEntity);
            }

            return recipe;
        }

        public async Task<List<Recipe>> GetRecipes(string userId)
        {
            var recipeEntities = await databaseService.Get<RecipeEntity>(userId);

            var recipes = mapper.Map<List<Recipe>>(recipeEntities);

            foreach (var recipe in recipes)
            {
                var ingredients = await databaseService.Get<IngredientEntity>(recipe.RecipeId);
                recipe.Ingredients = mapper.Map<List<Ingredient>>(ingredients);
            }

            return recipes;
        }

        public async Task DeleteRecipe(string userId, string recipeId)
        {
            var recipe = await GetRecipe(userId, recipeId);

            await databaseService.Delete<RecipeEntity>(userId, recipeId);

            foreach (var ingredient in recipe.Ingredients)
            {
                await databaseService.Delete<IngredientEntity>(recipeId, ingredient.IngredientId);
            }
        }
    }
}
