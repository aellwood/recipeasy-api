using Microsoft.EntityFrameworkCore;
using Recipeasy.Api.Models;

namespace Recipeasy.Api.Contexts
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }
        
        public DbSet<Recipe> Recipes { get; set; }
    }
}