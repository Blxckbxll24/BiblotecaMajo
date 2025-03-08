using Bibloteca.Models.Domain;
using Bibloteca.Servicios.IServices;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Bibloteca.Controllers
{
    [Route("api/[controller]")] // Enrutamiento base: /api/rol
    [ApiController] // Habilitar comportamientos específicos de API
    public class RolController : ControllerBase
    {
        private readonly IRolServices _rolService;

        public RolController(IRolServices rolService)
        {
            _rolService = rolService;
        }

        // GET: api/rol
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var roles = await _rolService.GetAll();
            return Ok(roles); // Devuelve 200 OK con la lista de roles
        }

        // GET: api/rol/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var rol = await _rolService.GetById(id);
            if (rol == null)
            {
                return NotFound(); // Devuelve 404 Not Found si el rol no existe
            }
            return Ok(rol); // Devuelve 200 OK con el rol encontrado
        }

        // POST: api/rol
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Rol rol)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Devuelve 400 Bad Request si el modelo no es válido
            }

            await _rolService.Add(rol);
            return CreatedAtAction(nameof(GetById), new { id = rol.PkRol }, rol); // Devuelve 201 Created con la ubicación del nuevo recurso
        }

        // PUT: api/rol/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Rol rol)
        {
            if (id != rol.PkRol)
            {
                return BadRequest("El ID del rol no coincide."); // Devuelve 400 Bad Request si los IDs no coinciden
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Devuelve 400 Bad Request si el modelo no es válido
            }

            await _rolService.Update(rol);
            return NoContent(); // Devuelve 204 No Content (actualización exitosa)
        }

        // DELETE: api/rol/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            bool eliminado = await _rolService.Delete(id);
            if (!eliminado)
            {
                return NotFound(); // Devuelve 404 Not Found si el rol no existe
            }

            return NoContent(); // Devuelve 204 No Content (eliminación exitosa)
        }
    }
}