using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Unach.DA.Empleo.Presentacion.CentralAdmin.Models;
using Unach.DA.Empleo.Presentacion.CentralAdmin.ViewModel;
using Unach.DA.Empleo.Presistencia.Api;

namespace Unach.DA.Empleo.Presentacion.CentralAdmin.Controllers
{
    public class ApiController : Controller
    {
        public async Task<IActionResult> Index()
        {
             using (HttpClient client = new HttpClient())
            {
                ClienteApi clienteapi = new ClienteApi("");
                var email = "edisson.guerrero@unach.edu.ec";
                var response = clienteapi.Get<Api>("https://pruebas.unach.edu.ec:4431/api/Estudiante/InformacionBasica/"+email);

                var viewModel = new ApiViewModel
                {
                    Apis = new List<Api> { response }
                };

                return View(viewModel);
            }

        }
    }
}



