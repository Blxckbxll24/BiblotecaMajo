using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Bibloteca.Models.Domain
{
    public class Usuario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PkUsuario { get; set; }

        public string Nombre { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string Password { get; set; }

        [ForeignKey(nameof(Rol))]
        public int FkRol { get; set; }
        public Rol Rol { get; set; }


        [JsonIgnore] 
        [Required] 
        public List<Recipe>? Recipes { get; set; } = new();


    }
}
