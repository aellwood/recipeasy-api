using AutoMapper;
using Recipeasy_API.Entities;
using Recipeasy_API.Models;

namespace Recipeasy_API.AutoMapperProfiles
{
    public class RecipeasyProfile : Profile
    {
        public RecipeasyProfile()
        {
            CreateMap<RecipeEntity, Recipe>()
                .ForMember(dest => dest.RecipeId, opt => opt.MapFrom(src => src.RowKey));
            CreateMap<IngredientEntity, Ingredient>()
                .ForMember(dest => dest.IngredientId, opt => opt.MapFrom(src => src.RowKey));
        }
    }
}
