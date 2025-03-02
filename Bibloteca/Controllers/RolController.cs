using Bibloteca.Models.Domain;
using Bibloteca.Servicios.IServices;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Bibloteca.Controllers
{
    public class RolController : Controller
    {
        private readonly IRolServices _rolService;

        public RolController(IRolServices rolService)
        {
            _rolService = rolService;
        }

        public async Task<IActionResult> Index()
        {
            var roles = await _rolService.GetAll();
            return View(roles);
        }

        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Crear(Rol rol)
        {
            if (!ModelState.IsValid)
                return View(rol);

            await _rolService.Add(rol);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Editar(int id)
        {
            var rol = await _rolService.GetById(id);
            if (rol == null) return NotFound();

            return View(rol);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(Rol rol)
        {
            if (!ModelState.IsValid)
                return View(rol);

            await _rolService.Update(rol);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Eliminar(int PkRol)
        {
            bool eliminado = await _rolService.Delete(PkRol);

            if (!eliminado)
            {
                return Json(new { success = false, message = "El rol no existe o no se pudo eliminar." });
            }

            return Json(new { success = true, message = "Rol eliminado correctamente." });
        }
    }
}
