using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unach.DA.Empleo.Dominio.Core;
using Unach.DA.Empleo.Persistencia.Core.Models;
using Unach.DA.Empleo.Presentacion.CentralAdmin.Extensions;
using Unach.DA.Empleo.Presentacion.CentralAdmin.ViewModel;

namespace Unach.Codesi.Cgrni.Cooperacion.Presentation.CentralAdmin.Views.Shared.Components.MenuPerfilServidor
{
    public class MenuPerfilServidorViewComponent: ViewComponent
    {
        EntitiesDomain entitiesDomain;
        private ILogger logger;

        public MenuPerfilServidorViewComponent(DbContextOptions<SicoaContext> options, ILoggerFactory log)
        {
            entitiesDomain = new EntitiesDomain(options);
            logger = log.CreateLogger(typeof(MenuPerfilServidorViewComponent));
        }



        public IViewComponentResult Invoke(int idServidor)
        {
            PerfilServidorEditadoViewModel pf = new PerfilServidorEditadoViewModel();
            pf.Persona.ServidorId = idServidor;
            ViewBag.EsFuncionario = HttpContext.ServidorAutenticado().EsSoloFuncionario;

            List<TransaccioMenuViewModel> transacciones = new();
            transacciones.AddRange(entitiesDomain.ExecuteStoredProcedure<TransaccioMenuViewModel>("Auth.GetTransaccionesBySistemaUsuario", ("idSistema", 1), ("idUsuario", HttpContext.ServidorAutenticado().IdServidor)).ToList());
            ViewBag.TransaccionesHabilitadas = transacciones;


            return View(pf);
        }


    }
}
