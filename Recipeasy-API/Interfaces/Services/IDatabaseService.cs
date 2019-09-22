using Microsoft.WindowsAzure.Storage.Table;
using Recipeasy_API.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Recipeasy_API.Interfaces.Services
{
    public interface IDatabaseService
    {
        Task<CloudTable> AccessDb();

        Task<IEnumerable<RecipeEntity>> GetRecipes(CloudTable recipeTable, string email);
    }
}
