using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Recipeasy.Api.Contexts;
using Recipeasy.Api.Interfaces.Services;
using Recipeasy.Api.Models;

namespace Recipeasy.Api.Services
{
    public class RecipesService : IRecipesService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public RecipesService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public Recipe AddRecipe(Recipe recipe, string userId)
        {
            recipe.UserId = userId;
            recipe.RecipeId = Guid.NewGuid().ToString();
            recipe.DateAdded = DateTime.UtcNow;
            
            recipe.Ingredients.ForEach(x => x.IngredientId = Guid.NewGuid().ToString());
            
            _context.Recipes.Add(recipe);
            _context.SaveChanges();

            return recipe;
        }

        public Recipe GetRecipe(string userId, string recipeId)
        {
            return _context.Recipes.Include(x => x.Ingredients).FirstOrDefault(x => x.UserId == userId && x.RecipeId == recipeId);
        }

        public List<Recipe> GetRecipes(string userId)
        {
            return _context.Recipes.Include(x => x.Ingredients).Where(x => x.UserId == userId).ToList();
        }

        public Recipe DeleteRecipe(string userId, string recipeId)
        {
            var recipe = _context.Recipes.Include(x => x.Ingredients).FirstOrDefault(x => x.UserId == userId && x.RecipeId == recipeId);

            if (recipe == null)
            {
                return null;
            }

            _context.Recipes.Remove(recipe);
            _context.SaveChanges();
            
            return recipe;
        }
    }
}