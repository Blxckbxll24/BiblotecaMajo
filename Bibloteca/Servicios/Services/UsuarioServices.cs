using Bibloteca.Context;
using Bibloteca.Models.Domain;
using Bibloteca.Servicios.IServices;
using Microsoft.EntityFrameworkCore;

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
                var result = _context.Usuarios
                    .Include(u => u.Rol) // Incluir la relación con Rol
                    .ToList();

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Sucedió un error: " + ex.Message);
            }
        }

        public async Task<bool> CrearUsuario(Usuario request)
        {
            try
            {
                Usuario usuario = new Usuario
                {
                    Nombre = request.Nombre,
                    UserName = request.UserName,
                    Password = request.Password,
                    //encriptar la contraseña
                    FkRol = request.FkRol,
                };
                _context.Usuarios.Add(usuario);
                var result = await _context.SaveChangesAsync();

                if (result > 1)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Sucedio un error" + ex.Message);
            }
        }

        public async Task<bool> ActualizarUsuario(Usuario request)
        {
            try
            {
                var usuario = await _context.Usuarios.FindAsync(request.PkUsuario);
                if (usuario == null)
                {
                    return false;
                }

                usuario.Nombre = request.Nombre;
                usuario.UserName = request.UserName;
                usuario.Password = request.Password;
                usuario.FkRol = request.FkRol;

                _context.Usuarios.Update(usuario);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrio un error" + ex.Message);
            }
        }

        public async Task<bool> EliminarUsuarios(int id)
        {
            try
            {
                Usuario user = await ObtenerUsuario(id);
                if (user != null)
                {
                    _context.Usuarios.Remove(user);
                    int rows = await _context.SaveChangesAsync();
                    return rows > 0;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error eliminando usuario: {ex.Message}");
                return false;
            }
        }


        public async Task<Usuario> ObtenerUsuario(int id)
        {
            try
            {
                Usuario usuario = await _context.Usuarios.FindAsync(id);
                //Usuario usuario = _context.Usuarios.FirstOrDefault(x =>x.PkUsuario ==  id);

                return usuario;
            }
            catch (Exception ex)
            {
                throw new Exception("Sucedio un error" + ex.Message);
            }
        }


    }
}
