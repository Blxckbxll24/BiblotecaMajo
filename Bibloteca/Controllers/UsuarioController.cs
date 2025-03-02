using Bibloteca.Models.Domain;
using Bibloteca.Servicios.IServices;
using Bibloteca.Servicios.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bibloteca.Controllers
{
    public class UsuarioController : Controller


    {
        private readonly IUsuarioServices _usuarioServices;
        private readonly IRolServices _rolServices;


        public UsuarioController(IUsuarioServices usuarioServices, IRolServices rolServices)
        {
            _usuarioServices = usuarioServices;
            _rolServices = rolServices;
        }
        public IActionResult Index()
        {
            var result = _usuarioServices.ObtenerUsuario();

            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> Crear()
        {
            List<Rol> result = await _rolServices.GetAll();
            ViewBag.Roles = result.Select(p => new SelectListItem()
            {
                Text = p.Nombre,
                Value = p.PkRol.ToString()
            });

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Crear(Usuario request)
        {
            await _usuarioServices.CrearUsuario(request);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var usuario = await _usuarioServices.ObtenerUsuario(id);
            if (usuario == null)
            {
                return NotFound();
            }

            List<Rol> roles = await _rolServices.GetAll();
            ViewBag.Roles = roles.Select(p => new SelectListItem()
            {
                Text = p.Nombre,
                Value = p.PkRol.ToString(),
                Selected = p.PkRol == usuario.FkRol
            });

            return View(usuario);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(Usuario request)
        {
            bool actualizado = await _usuarioServices.ActualizarUsuario(request);
            if (!actualizado)
            {
                ModelState.AddModelError("", "No se puede editar el usuario");
                return View(request);
            }
            return RedirectToAction("Index");
        }


        [HttpPost]
        public async Task<IActionResult> Eliminar(int PKUsuario)
        {
            bool result = await _usuarioServices.EliminarUsuarios(PKUsuario);

            if (result)
            {
                return Json(new { success = true, message = "Usuario eliminado correctamente." });
            }
            else
            {
                return Json(new { success = false, message = "No se pudo eliminar el usuario." });
            }
        }

    }
}
