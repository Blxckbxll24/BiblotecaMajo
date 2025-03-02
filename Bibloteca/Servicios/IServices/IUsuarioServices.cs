using Bibloteca.Models.Domain;

namespace Bibloteca.Servicios.IServices
{
    public interface IUsuarioServices
    {
        public List<Usuario> ObtenerUsuario();
        public Task<bool> CrearUsuario(Usuario request);

        public Task<Usuario> ObtenerUsuario(int id);

        public Task<bool> ActualizarUsuario(Usuario request);

        public Task<bool> EliminarUsuarios(int id);
    }
}
