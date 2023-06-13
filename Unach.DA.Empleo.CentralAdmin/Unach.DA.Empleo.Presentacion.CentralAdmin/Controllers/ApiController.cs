using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Unach.DA.Empleo.Dominio.Core;
using Unach.DA.Empleo.Persistencia.Core.Models;
using Unach.DA.Empleo.Presentacion.CentralAdmin.Extensions;
using Unach.DA.Empleo.Presentacion.CentralAdmin.Models;
using Unach.DA.Empleo.Presentacion.CentralAdmin.ViewModel;
using Unach.DA.Empleo.Presistencia.Api;

namespace Unach.DA.Empleo.Presentacion.CentralAdmin.Controllers
{
    public class ApiController : Controller
    {
      
        public async Task<IActionResult> Index()
        {
            return View();
        }


        public bool Verificar (string ci) {

            using (HttpClient client = new HttpClient())
            {
                ClienteApi clienteapi = new ClienteApi("");
              
                var response = clienteapi.Get<Api>("https://pruebas.unach.edu.ec:4431/api/Estudiante/InformacionBasicaPorCriterio/" + ci);
                if (response != null)
                {

                    return true;
                }
                else
                {
                    return false;
                }
  
            }
           
        }


    }


}



