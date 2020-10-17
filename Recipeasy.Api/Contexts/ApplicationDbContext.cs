using Microsoft.EntityFrameworkCore;
using Recipeasy.Api.Models;

namespace Recipeasy.Api.Contexts
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }
        
        public DbSet<Recipe> Recipes { get; set; }
    }
}