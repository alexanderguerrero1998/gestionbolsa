using Microsoft.AspNetCore.Mvc;

namespace Unach.DA.Empleo.Presentacion.CentralAdmin.Controllers
{
    public class ReporteController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Mostrar()
        {
            return View();
        }
    }
}
