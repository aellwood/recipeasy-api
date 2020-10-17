using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Recipeasy.Api.Entities;
using Recipeasy.Api.Interfaces.Services;
using Recipeasy.Api.Models;

namespace Recipeasy.Api.Services.V1
{
    public class RecipesV1Service : IRecipesService
{
        private readonly IDatabaseService _databaseService;
        private readonly IMapper _mapper;

        public RecipesV1Service(IDatabaseService databaseService, IMapper mapper)
        {
            _databaseService = databaseService;
            _mapper = mapper;
        }

        public async Task<Recipe> AddRecipe(Recipe recipePayload, string userId)
        {
            var recipeEntity = _mapper.Map<RecipeEntity>((recipePayload, userId));

            await _databaseService.Add(recipeEntity);

            foreach (var ingredient in recipePayload.Ingredients)
            {
                var ingredientEntity = _mapper.Map<IngredientEntity>((ingredient, recipeEntity.RowKey));
                await _databaseService.Add(ingredientEntity);
            }

            return await GetRecipe(userId, recipeEntity.RowKey);
        }

        public async Task<Recipe> GetRecipe(string userId, string recipeId)
        {
            var recipeEntity = await _databaseService.Get<RecipeEntity>(userId, recipeId);
            var recipe = _mapper.Map<Recipe>(recipeEntity);

            if (recipe != null)
            {
                var ingredientEntity = await _databaseService.Get<IngredientEntity>(recipeId);
                recipe.Ingredients = _mapper.Map<List<Ingredient>>(ingredientEntity);
            }

            return recipe;
        }

        public async Task<List<Recipe>> GetRecipes(string userId)
        {
            var recipeEntities = await _databaseService.Get<RecipeEntity>(userId);

            var recipes = _mapper.Map<List<Recipe>>(recipeEntities);

            foreach (var recipe in recipes)
            {
                var ingredients = await _databaseService.Get<IngredientEntity>(recipe.RecipeId);
                recipe.Ingredients = _mapper.Map<List<Ingredient>>(ingredients);
            }

            return recipes;
        }

        public async Task<Recipe> DeleteRecipe(string userId, string recipeId)
        {
            var recipe = await GetRecipe(userId, recipeId);

            if (recipe == null)
            {
                return null;
            }

            await _databaseService.Delete<RecipeEntity>(userId, recipeId);

            foreach (var ingredient in recipe.Ingredients)
            {
                await _databaseService.Delete<IngredientEntity>(recipeId, ingredient.IngredientId);
            }

            return recipe;
        }
    }
}
