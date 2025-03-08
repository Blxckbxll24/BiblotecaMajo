using Bibloteca.Models.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bibloteca.Models;

namespace Bibloteca.Services.Domain
{
    public interface IRecipeService
    {
        Task<IEnumerable<Recipe>> GetAllRecipes();
        Task<Recipe> GetRecipeById(int id);
        Task<Recipe> CreateRecipe(Recipe recipe);
        Task<Recipe> UpdateRecipe(int id, Recipe updatedRecipe);
        Task<bool> DeleteRecipe(int id);
    }
}
