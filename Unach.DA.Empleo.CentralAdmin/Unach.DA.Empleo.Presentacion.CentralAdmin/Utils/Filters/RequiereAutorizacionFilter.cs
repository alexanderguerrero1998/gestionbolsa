//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Filters;
//using Microsoft.AspNetCore.Mvc.ViewFeatures;
//using Microsoft.AspNetCore.Routing;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Logging;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Unach.Codesi.SecretariaGeneral.Gaceta.Domain.Core;
//using Unach.Codesi.SecretariaGeneral.Gaceta.Persistence.Core.Models;

//namespace Unach.Codesi.Cgrni.Cooperacion.Presentation.CentralAdmin.Utils.Filters
//{
//    public class RequiereAutorizacionFilter : ActionFilterAttribute
//    {
//        private readonly ILogger _logger;
//        private readonly string _safelist;

//        private List<string[]> ipsPermitidas;

//        EntitiesDomain entitiesDomain;


//        public RequiereAutorizacionFilter(DbContextOptions<CooperacionContext> options,
//            ILoggerFactory log)
//        {
//            _logger = log.CreateLogger(typeof(ComprobarIpPermitidasAccionFilter));
//            entitiesDomain = new EntitiesDomain(options);

            
//        }
//        public override void OnActionExecuting(ActionExecutingContext context)
//        {
//            var route = context.HttpContext.GetRouteData();
//            int expediente = context.HttpContext.ServidorAutenticado()?.IdServidor ?? 0;

//            string controladorSolicitado = route?.Values != null && route.Values.ContainsKey("controller") ? route.Values["controller"].ToString() : string.Empty;
//            string accionSolicitada = route?.Values != null && route.Values.ContainsKey("action") ? route?.Values["action"].ToString() : string.Empty;

//            bool tieneAcceso = entitiesDomain.RolUsuarioRepositorio.Contar(
//                            filtro => filtro.IdUsuario == expediente
//                                && filtro.Hasta.Date >= DateTime.Now.Date
//                                && filtro.IdRolNavigation.RolTransaccion
//                                    .Any(c => c.IdTransaccionNavigation.Accion == accionSolicitada
//                                        && c.IdTransaccionNavigation.Controlador == controladorSolicitado)) > 0; 


//            if (!tieneAcceso)
//            {
//                Microsoft.AspNetCore.Routing.RouteValueDictionary redirectTargetDictionary =
//                                               new Microsoft.AspNetCore.Routing.RouteValueDictionary();
//                redirectTargetDictionary.Add("action", "Index");
//                redirectTargetDictionary.Add("controller", "Home");
                
//                // get TempData handle
//                ITempDataDictionaryFactory factory = context.HttpContext.RequestServices.GetService(typeof(ITempDataDictionaryFactory)) as ITempDataDictionaryFactory;
//                ITempDataDictionary tempData = factory.GetTempData(context.HttpContext);
//                tempData.MostrarAlerta(ViewModel.TipoAlerta.Advertencia, "ATENCIÓN! No tiene acceso a la funcionalidad solicitada");

//                context.Result = new RedirectToRouteResult(redirectTargetDictionary);
//                context.Result.ExecuteResultAsync(context); 
//            }            

//            base.OnActionExecuting(context);
//        }
//        /*
//         string accionSolicitada = "Permisos";
//            string controladorSolicitado = "ControlPersonal";
//            var permisoSobreAccionActual = entitiesDomain.RolUsuarioRepositorio.Contar(
//                            filtro => filtro.IdUsuario == expediente
//                                && filtro.Hasta.Date >= DateTime.Now.Date
//                                && filtro.IdRolNavigation.RolTransaccion
//                                    .Count(c => c.IdTransaccionNavigation.Accion == accionSolicitada
//                                        && c.IdTransaccionNavigation.Controlador == controladorSolicitado) > 0) > 0;

//            if (!permisoSobreAccionActual)
//            {
//                return RedirectToAction("Index", "Home");
//            }
//         */
//    }
//}
