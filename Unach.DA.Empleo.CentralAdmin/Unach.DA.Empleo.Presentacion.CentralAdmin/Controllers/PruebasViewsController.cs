using Microsoft.AspNetCore.Mvc;

namespace Unach.DA.Empleo.Presentacion.CentralAdmin.Controllers
{
    public class PruebasViewsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
