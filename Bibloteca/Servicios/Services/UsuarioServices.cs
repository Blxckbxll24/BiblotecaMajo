using Bibloteca.Context;
using Bibloteca.Models.Domain;
using Bibloteca.Servicios.IServices;

namespace Bibloteca.Servicios.Services
{
    public class UsuarioServices : IUsuarioServices
    {
        private readonly ApplicationDBContext _context;
        public UsuarioServices(ApplicationDBContext context)
        {
        _context = context;
        }

        public List<Usuario> ObtenerUsuario()
        {
            try
            {
                var result =_context.Usuarios.ToList();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Sucedio un error: "+ ex);
            }
        }
    }
}
