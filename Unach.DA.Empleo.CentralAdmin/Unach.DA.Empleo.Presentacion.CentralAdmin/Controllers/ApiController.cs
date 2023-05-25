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
                var response = clienteapi.Get<Api>("https://dev-bb4zah0ivws1wedd.us.auth0.com/api/v2/client-grants?per_page=2&page=1&include_totals=true");

                var viewModel = new ApiViewModel
                {
                    Apis = new List<Api> { response }
                };

                return View(viewModel);
            }

        }
    }
}



