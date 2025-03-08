using Bibloteca.Context;
using Bibloteca.Models.Domain;
using Bibloteca.Services.Domain;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;


namespace Bibloteca.Servicios.Services
{
    public class RecipeService : IRecipeService
    {
        private readonly ApplicationDBContext _context;

        public RecipeService(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Recipe>> GetAllRecipes()
        {
            return await _context.Recipes.Include(r => r.Usuario).ToListAsync();
        }

        public async Task<Recipe> GetRecipeById(int id)
        {
            return await _context.Recipes.Include(r => r.Usuario).FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<Recipe> CreateRecipe(Recipe recipe)
        {
            // Asegúrate de que IngredientsJson y StepsJson estén serializados
            recipe.IngredientsJson = JsonSerializer.Serialize(recipe.Ingredients);
            recipe.StepsJson = JsonSerializer.Serialize(recipe.Steps);

            _context.Recipes.Add(recipe);
            await _context.SaveChangesAsync();
            return recipe;
        }

        public async Task<Recipe> UpdateRecipe(int id, Recipe updatedRecipe)
        {
            var existingRecipe = await _context.Recipes.FindAsync(id);
            if (existingRecipe == null) return null;

            existingRecipe.Title = updatedRecipe.Title;
            existingRecipe.ImageUrl = updatedRecipe.ImageUrl;
            existingRecipe.CategoryId = updatedRecipe.CategoryId;
            existingRecipe.Rating = updatedRecipe.Rating;
            existingRecipe.Description = updatedRecipe.Description;
            existingRecipe.Ingredients = updatedRecipe.Ingredients;
            existingRecipe.Steps = updatedRecipe.Steps;

            _context.Recipes.Update(existingRecipe);
            await _context.SaveChangesAsync();
            return existingRecipe;
        }

        public async Task<bool> DeleteRecipe(int id)
        {
            var recipe = await _context.Recipes.FindAsync(id);
            if (recipe == null) return false;

            _context.Recipes.Remove(recipe);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
