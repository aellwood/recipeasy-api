using AutoMapper;
using Recipeasy.Api.Entities;
using Recipeasy.Api.Models;
using System;

namespace Recipeasy.Api.AutoMapperProfiles
{
    public class RecipeasyProfile : Profile
    {
        public RecipeasyProfile()
        {
            CreateMap<RecipeEntity, Recipe>()
                .ForMember(dest => dest.RecipeId, opt => opt.MapFrom(src => src.RowKey));

            CreateMap<IngredientEntity, Ingredient>()
                .ForMember(dest => dest.IngredientId, opt => opt.MapFrom(src => src.RowKey));

            CreateMap<(Recipe, string), RecipeEntity>()
                .ForMember(dest => dest.PartitionKey, opt => opt.MapFrom(src => src.Item2))
                .ForMember(dest => dest.RowKey, opt => opt.MapFrom(src => Guid.NewGuid().ToString()))
                .ForMember(dest => dest.RecipeName, opt => opt.MapFrom(src => src.Item1.RecipeName))
                .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.Item1.Notes));

            CreateMap<(Ingredient, string), IngredientEntity>()
                .ForMember(dest => dest.PartitionKey, opt => opt.MapFrom(src => src.Item2))
                .ForMember(dest => dest.RowKey, opt => opt.MapFrom(src => Guid.NewGuid().ToString()))
                .ForMember(dest => dest.IngredientName, opt => opt.MapFrom(src => src.Item1.IngredientName))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Item1.Quantity));
        }
    }
}
