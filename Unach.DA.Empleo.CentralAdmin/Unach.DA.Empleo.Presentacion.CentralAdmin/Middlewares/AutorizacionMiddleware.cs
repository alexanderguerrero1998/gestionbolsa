using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Unach.DA.Empleo.Presentacion.CentralAdmin.Extensions;
using Unach.DA.Empleo.Persistencia.Core.Models;
using Unach.DA.Empleo.Dominio.Core;

namespace Unach.Codesi.Cgrni.Cooperacion.Presentation.CentralAdmin.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    /// <summary>
    /// Middleware para la comprobación de los permisos al tratar de acceder a una acción dentro de un controlador
    /// </summary>
    public class AutorizacionMiddleware
    {
        private readonly RequestDelegate _next;
        EntitiesDomain dominioEntidades;
        private ILogger logger;

        public AutorizacionMiddleware(RequestDelegate next,
           DbContextOptions<SicoaContext> options,
            ILoggerFactory log)
        {
            _next = next;
            logger = log.CreateLogger(typeof(AutorizacionMiddleware));
            dominioEntidades = new EntitiesDomain(options);
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var route = httpContext.GetRouteData();
            string controladorSolicitado = route?.Values != null && route.Values.ContainsKey("controller") ? route.Values["controller"].ToString() : string.Empty;
            string accionSolicitada = route?.Values != null && route.Values.ContainsKey("action") ? route?.Values["action"].ToString() : string.Empty;

            if ((controladorSolicitado == "Home"
                    && (accionSolicitada == "Error" ||
                    accionSolicitada == "NoAuth" || accionSolicitada == "OhNo" || accionSolicitada == "ShowImage" || accionSolicitada == "AcuerdosConfidencialidad")) ||
                    (controladorSolicitado == "Account" && (accionSolicitada == "SignOut" || accionSolicitada == "SignIn")) ||
                    (controladorSolicitado == "" && accionSolicitada == "")
                    || controladorSolicitado == "Validaciones"
                    || controladorSolicitado == "WebDocumentViewer"
                    || controladorSolicitado == "Inscripcion"
                    || controladorSolicitado == "Preinscripcion"
                    || controladorSolicitado == "Login"
                    || controladorSolicitado == "Documentacion")
            {
                httpContext.Response.StatusCode = StatusCodes.Status200OK;
            }
            else
            {
                var servidor = httpContext.ServidorAutenticado();
               /* if (servidor != null && servidor.IdServidor != 0)
                {
                    var hoy = DateTime.Now;
                    bool tienePermiso = dominioEntidades.RolUsuarioRepositorio.Contar(
                                filtro => filtro.IdUsuario == servidor.IdServidor
                                    && filtro.IdRolNavigation.IdSistema == 7
                                    && filtro.Hasta.Date >= hoy.Date
                                    && filtro.IdRolNavigation.RolTransaccion
                                        .Count(c => c.IdTransaccionNavigation.Accion == accionSolicitada
                                            && c.IdTransaccionNavigation.Controlador == controladorSolicitado) > 0
                                        ) > 0;
                    if (tienePermiso)
                    {
                        var roles = dominioEntidades.RolUsuarioRepositorio.ObtenerTodosEnOtraVista<RolUsuario>(
                            select => new RolUsuario
                            {
                                IdRol = select.IdRol,
                                //Nombre = select.IdRolNavigation.Nombre,
                                //Activo = false
                            },
                            filtro => filtro.IdUsuario == servidor.IdServidor
                                && filtro.Hasta.Date >= hoy.Date
                                && filtro.IdRolNavigation.RolTransaccion
                                    .Count(c => c.IdTransaccionNavigation.Accion == accionSolicitada
                                        && c.IdTransaccionNavigation.Controlador == controladorSolicitado) > 0
                        );
                        if (roles != null && roles.Count > 0)
                        {

                            // 2.- Si no se han seteado todos los roles en la sesión
                            //if (httpContext.RolesServidorInformacionPersonal() == null
                            //        || httpContext.RolesServidorInformacionPersonal()?.Count < roles.Count)
                            //{
                            //roles[0].Activo = true;
                            //httpContext.Session.SetString(VariablesSesion.ROLES_SERVIDOR, JsonConvert.SerializeObject(roles));
                            //}
                            //var idRol = httpContext.RolActivoServidorInformacionPersonal()?.IdRol ?? 0;
                            ////var acuerdos = dominioEntidades.UsuarioAcuerdoRepositorio.BuscarPor(
                            //        filtro => filtro.IdServidor == servidor.IdServidor
                            //            && filtro.Firmado
                            //            && filtro.IdUsuarioRol == idRol
                            //    ).FirstOrDefault();
                            //if (acuerdos != null)
                            {
                                await _next(httpContext);
                                return;
                            }
                            //else
                            //{
                            //    httpContext.Response.StatusCode = StatusCodes.Status406NotAcceptable;
                            //    httpContext.Response.Redirect("/Home/AcuerdosConfidencialidad");
                            //    return;
                            //}
                            //if (usuario.AceptoAcuerdoConfidencialidad)
                            //{
                            //    if (roles != null && roles.Count > 0)
                            //    {
                            //        await _next(httpContext);
                            //        return;
                            //    }
                            //}
                            //else
                            //{
                            //    httpContext.Response.StatusCode = StatusCodes.Status406NotAcceptable;
                            //    httpContext.Response.Redirect("/Home/AcuerdosConfidencialidad");
                            //    return;
                            //}
                            //

                        }
                    }
                }
                else // Sesión caducada, TODO: agregar vista
                {
                    logger.LogError("ERROR AUTENTICACIÓN: No se ha seteado los valores de servidor, posible causa, token ApiDath caducado");
                }
                */
               
                
                httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                httpContext.Response.Redirect("/Home/OhNo");
                return;


            }

            await _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class AutorizacionMiddlewareExtensions
    {
        public static IApplicationBuilder UseAutorizacionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AutorizacionMiddleware>();
        }
    }
}
