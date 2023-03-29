using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unach.DA.Empleo.Dominio.Core;
using Unach.DA.Empleo.Persistencia.Core.Models;

namespace Unach.Codesi.Cgrni.Cooperacion.Presentation.CentralAdmin.Middlewares
{
    public class AcuerdosMiddleware
    {
        private readonly RequestDelegate _next;
        EntitiesDomain entitiesDomain;
        public AcuerdosMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext, DbContextOptions<SicoaContext> options)
        {
            entitiesDomain = new EntitiesDomain(options);

            var route = httpContext.GetRouteData();
            string controladorSolicitado = route?.Values != null && route.Values.ContainsKey("controller") ? route.Values["controller"].ToString() : string.Empty;
            string actionRequested = route?.Values != null && route.Values.ContainsKey("action") ? route?.Values["action"].ToString() : string.Empty;

            if (!httpContext.Request.Method.Equals("Post"))
            {
                if ((controladorSolicitado == "Home" && actionRequested == "NoAuth") ||
                (controladorSolicitado == "Account" && actionRequested == "SignOut"))
                {
                    httpContext.Response.StatusCode = StatusCodes.Status200OK;
                }
                // 1. Preservar petición original

                // 2.- Redireccionar a vista para aceptar acuerdos
                httpContext.Response.Redirect("");
            }

            
            
            await _next(httpContext);
        }
    }
}
