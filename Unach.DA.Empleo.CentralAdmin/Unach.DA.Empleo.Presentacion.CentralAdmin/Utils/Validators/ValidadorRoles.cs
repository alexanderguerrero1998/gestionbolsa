using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Unach.DA.Empleo.Presentacion.CentralAdmin.Extensions;
using Unach.DA.Empleo.Dominio.Core;
using Unach.DA.Empleo.Persistencia.Core.Models;

namespace Unach.DA.Empleo.Presentacion.CentralAdmin.Utils.Validators
{
    public class ValidadorRoles : Attribute, IAuthorizationFilter
    {

        EntitiesDomain entitiesDomain;
        private ILogger logger;
        public ValidadorRoles()//DbContextOptions<DathGestionContext> options,  ILoggerFactory log)
        {
     
        }


        private readonly IEnumerable<string> roles;

        //public ValidadorRoles(params string[] roles) => this.roles = roles;

        //public ValidadorRoles(string rol) => roles = new List<string> { rol };

        public void OnAuthorization(AuthorizationFilterContext context)//, DbContextOptions<DathGestionContext> options, ILoggerFactory log)
        {
            var options = context.HttpContext.RequestServices.GetService<DbContextOptions<SicoaContext>>();

            entitiesDomain = new EntitiesDomain(options);
            //logger = log.CreateLogger(typeof(ValidadorRoles));

            //if (context.HttpContext.User.Claims == null || context.HttpContext.User.Claims?.Count() <= 0)
            if (context.HttpContext.Session.GetString("AuthenticatedUser") == null || context.HttpContext.Session.GetString("AuthenticatedUser")?.Count() <= 0)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var route = context.HttpContext.GetRouteData();
            
            string controladorSolicitado = route?.Values != null && route.Values.ContainsKey("controller") ? route.Values["controller"].ToString() : string.Empty;
            string actionRequested = route?.Values != null && route.Values.ContainsKey("action") ? route?.Values["action"].ToString() : string.Empty;

            //var rolesActual = context.HttpContext.ServidorAutenticado().Roles.Select(x=>x.Id);

           /* var rolesActual = entitiesDomain.RolUsuarioRepositorio.BuscarPor(
                x=>x.IdUsuario== context.HttpContext.ServidorAutenticado().IdServidor
                ).Select(x=>x.IdRol).ToList();

            var transacciones = entitiesDomain.RolTransaccionesRepositorio.BuscarPor(
                x => rolesActual.Contains(x.IdRol) &&
                x.IdTransaccionNavigation.Controlador.Equals(controladorSolicitado) &&
                x.IdTransaccionNavigation.Accion.Equals(actionRequested)
                ).Count();


            if (transacciones <= 0)                
                context.Result = new UnauthorizedResult();
            return;
           */






        }


    }
}
