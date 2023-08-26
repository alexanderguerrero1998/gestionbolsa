using Microsoft.AspNetCore.Mvc;
using Unach.DA.Empleo.Dominio.Core;
using Unach.DA.Empleo.Presentacion.CentralAdmin.ViewModel;

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
