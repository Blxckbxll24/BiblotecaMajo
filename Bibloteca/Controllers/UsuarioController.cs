using Bibloteca.Models.Domain;
using Bibloteca.Servicios.IServices;
using Bibloteca.Servicios.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bibloteca.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioServices _usuarioServices;
        private readonly IRolServices _rolServices;

        public UsuarioController(IUsuarioServices usuarioServices, IRolServices rolServices)
        {
            _usuarioServices = usuarioServices;
            _rolServices = rolServices;
        }

        [HttpGet("ObtenerUsuarios")]
        public IActionResult Index()
        {
            try
            {
                var result = _usuarioServices.ObtenerUsuario();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }

        [HttpGet("Crear")]
        public async Task<IActionResult> Crear()
        {
            try
            {
                List<Rol> result = await _rolServices.GetAll();
                var roles = result.Select(p => new SelectListItem()
                {
                    Text = p.Nombre,
                    Value = p.PkRol.ToString()
                });

                return Ok(new { roles });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }

        [HttpPost("Crear")]
        public async Task<IActionResult> Crear([FromBody] Usuario request)
        {
            try
            {
                await _usuarioServices.CrearUsuario(request);
                return CreatedAtAction(nameof(Index), new { id = request.PkUsuario }, request);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }

        [HttpGet("Editar/{id}")]
        public async Task<IActionResult> Editar(int id)
        {
            try
            {
                var usuario = await _usuarioServices.ObtenerUsuario(id);
                if (usuario == null)
                {
                    return NotFound(new { message = "Usuario no encontrado" });
                }

                List<Rol> roles = await _rolServices.GetAll();
                var rolesList = roles.Select(p => new SelectListItem()
                {
                    Text = p.Nombre,
                    Value = p.PkRol.ToString(),
                    Selected = p.PkRol == usuario.FkRol
                });

                return Ok(new { usuario, roles = rolesList });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }

        [HttpPut("Editar")]
        public async Task<IActionResult> Editar([FromBody] Usuario request)
        {
            try
            {
                bool actualizado = await _usuarioServices.ActualizarUsuario(request);
                if (!actualizado)
                {
                    return BadRequest(new { message = "No se pudo actualizar el usuario" });
                }
                return Ok(new { message = "Usuario actualizado correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }

        [HttpDelete("Eliminar/{PKUsuario}")]
        public async Task<IActionResult> Eliminar(int PKUsuario)
        {
            try
            {
                bool result = await _usuarioServices.EliminarUsuarios(PKUsuario);

                if (result)
                {
                    return Ok(new { success = true, message = "Usuario eliminado correctamente." });
                }
                else
                {
                    return BadRequest(new { success = false, message = "No se pudo eliminar el usuario." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }
    }
}
