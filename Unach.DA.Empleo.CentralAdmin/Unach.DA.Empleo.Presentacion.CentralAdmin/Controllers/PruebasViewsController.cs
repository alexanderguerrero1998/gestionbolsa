using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using Unach.DA.Empleo.Dominio.Core;
using Unach.DA.Empleo.Presentacion.CentralAdmin.Models;
using Unach.DA.Empleo.Presentacion.CentralAdmin.ViewModel;
using Unach.DA.Empleo.Presistencia.Api;

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
