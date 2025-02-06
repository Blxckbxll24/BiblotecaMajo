using Bibloteca.Servicios.IServices;
using Bibloteca.Servicios.Services;
using Microsoft.AspNetCore.Mvc;

namespace Bibloteca.Controllers
{
    public class UsuarioController : Controller

       
    {
        private readonly IUsuarioServices _usuarioServices;

        public UsuarioController(IUsuarioServices usuarioServices)
        {
            _usuarioServices = usuarioServices;
        }
        public IActionResult Index()
        {
            var result = _usuarioServices.ObtenerUsuario();
            return View(result);
        }
    }
}
