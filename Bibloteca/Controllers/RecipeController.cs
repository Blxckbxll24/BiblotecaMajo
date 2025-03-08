using Bibloteca.Models.Domain;
using Bibloteca.Services.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Bibloteca.Models;
using Bibloteca.Services;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;

namespace Bibloteca.Controllers
{
    [Route("api/recipes")]
    [ApiController]
    public class RecipeController : ControllerBase
    {
        private readonly IRecipeService _recipeService;

        public RecipeController(IRecipeService recipeService)
        {
            _recipeService = recipeService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Recipe>>> GetAllRecipes()
        {
            return Ok(await _recipeService.GetAllRecipes());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Recipe>> GetRecipeById(int id)
        {
            var recipe = await _recipeService.GetRecipeById(id);
            if (recipe == null) return NotFound("Receta no encontrada");
            return Ok(recipe);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Recipe>> CreateRecipe([FromBody] Recipe recipe)
        {
            var userId = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userId)) return Unauthorized("Usuario no autenticado");

            // Log para verificar los datos recibidos
            Console.WriteLine($"Datos recibidos: {JsonSerializer.Serialize(recipe)}");

            // Validar el objeto Recipe
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            recipe.UserId = int.Parse(userId);
            recipe.Usuario = null;

            var newRecipe = await _recipeService.CreateRecipe(recipe);
            return CreatedAtAction(nameof(GetRecipeById), new { id = newRecipe.Id }, newRecipe);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<Recipe>> UpdateRecipe(int id, [FromBody] Recipe updatedRecipe)
        {
            var userId = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userId)) return Unauthorized("Usuario no autenticado");

            var recipe = await _recipeService.GetRecipeById(id);
            if (recipe == null) return NotFound("Receta no encontrada");


            var updated = await _recipeService.UpdateRecipe(id, updatedRecipe);
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult> DeleteRecipe(int id)
        {
            var userId = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userId)) return Unauthorized("Usuario no autenticado");

            var recipe = await _recipeService.GetRecipeById(id);
            if (recipe == null) return NotFound("Receta no encontrada");


            bool deleted = await _recipeService.DeleteRecipe(id);
            return deleted ? NoContent() : StatusCode(500, "Error al eliminar la receta");
        }

        [HttpPost("upload-image")]
        public async Task<ActionResult<string>> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No se ha seleccionado ningún archivo.");
            }

            var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

            // Crear el directorio si no existe
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            var filePath = Path.Combine(directoryPath, Guid.NewGuid().ToString() + Path.GetExtension(file.FileName));

            try
            {
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var imageUrl = "/images/" + Path.GetFileName(filePath);
                return Ok(imageUrl);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al subir la imagen: {ex.Message}");
            }
        }
    }
}
