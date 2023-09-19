using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Unach.Codesi.Cgrni.Cooperacion.Presentation.CentralAdmin.Views.Shared.Components.MenuPerfilServidor;
using Unach.DA.Empleo.Dominio.Core;
using Unach.DA.Empleo.Persistencia.Core.Models;
using Unach.DA.Empleo.Presentacion.CentralAdmin.Extensions;
using Unach.DA.Empleo.Presentacion.CentralAdmin.ViewModel;

namespace Unach.DA.Empleo.Presentacion.CentralAdmin.Views.Shared.Components.MenuUsuario
{
    public class MenuUsuarioViewComponent : ViewComponent
    {
        EntitiesDomain entitiesDomain;
        private ILogger logger;

        public MenuUsuarioViewComponent(DbContextOptions<SicoaContext> options, ILoggerFactory log)
        {
            entitiesDomain = new EntitiesDomain(options);
            logger = log.CreateLogger(typeof(MenuPerfilServidorViewComponent));
        }

        public IViewComponentResult Invoke(string idServidor)
        {
            PerfilUsuarioEditadoViewModel pf = new PerfilUsuarioEditadoViewModel();
            pf.Persona.ServidorId = idServidor;
            ViewBag.EsUsuario= HttpContext.ServidorAutenticado().EsSoloUsuario;

            List<TransaccioMenuViewModel> transacciones = new();
            transacciones.AddRange(entitiesDomain.ExecuteStoredProcedure<TransaccioMenuViewModel>("Auth.GetTransaccionesBySistemaUsuario", ("idSistema", 1), ("idUsuario", "c3a8ae9d-8a91-47ea-b65a-bc610ab67380")));  // HttpContext.ServidorAutenticado().IdServidor)).ToList());
            ViewBag.TransaccionesHabilitadas = transacciones;


            return View(pf);
        }

    }
}
