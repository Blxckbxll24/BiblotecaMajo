using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Bibloteca.Models.Domain
{
    public class Recipe
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        public string ImageUrl { get; set; } = string.Empty;

        [ForeignKey(nameof(Category))] // Relación con la categoría
        public int CategoryId { get; set; } // ID de la categoría

        [JsonIgnore] // Evita la serialización recursiva
        public Category? Category { get; set; } // Propiedad de navegación

        [Range(1, 5)]
        public int Rating { get; set; } = 1;

        public string Description { get; set; } = string.Empty;

        [ForeignKey(nameof(Usuario))] // Relación con el usuario
        public int UserId { get; set; } // Clave foránea

        [JsonIgnore] // Evita la serialización recursiva
        public Usuario? Usuario { get; set; } // Propiedad de navegación

        // Serialización en JSON para evitar errores en la BD
        [Required]
        public string IngredientsJson { get; set; } = "[]";

        [Required]
        public string StepsJson { get; set; } = "[]";

        // Constructor para inicializar valores predeterminados
        public Recipe()
        {
            Ingredients = new List<string>();
            Steps = new List<string>();
        }

        [NotMapped]
        public List<string> Ingredients
        {
            get => DeserializeJson(IngredientsJson);
            set => IngredientsJson = JsonSerializer.Serialize(value);
        }

        [NotMapped]
        public List<string> Steps
        {
            get => DeserializeJson(StepsJson);
            set => StepsJson = JsonSerializer.Serialize(value);
        }

        // Método auxiliar para evitar excepciones al deserializar
        private static List<string> DeserializeJson(string json)
        {
            try
            {
                return string.IsNullOrWhiteSpace(json) ? new List<string>() : JsonSerializer.Deserialize<List<string>>(json) ?? new List<string>();
            }
            catch
            {
                return new List<string>(); // Evita fallos si hay datos corruptos
            }
        }
    }
}