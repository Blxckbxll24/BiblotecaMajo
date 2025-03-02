using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bibloteca.Models.Domain
{
    public class Usuario
    {
        [Key]
        public int PkUsuario { get; set; }
        public string Nombre { get; set;}
        public string UserName { get; set;}
        public string Password { get; set;}
        [ForeignKey(nameof(Rol))]
        public int FkRol { get; set; }
        public Rol Rol { get; set; }

    }
}
