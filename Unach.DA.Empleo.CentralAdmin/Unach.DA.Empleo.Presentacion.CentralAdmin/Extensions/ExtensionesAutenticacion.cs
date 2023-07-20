using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Unach.DA.Empleo.Dominio.Core;
using Unach.DA.Empleo.Persistencia.Core.Models;
using Unach.DA.Empleo.Presentacion.CentralAdmin.ViewModel;
using Unach.DA.Empleo.Presentacion.CentralAdmin.Data;

namespace Unach.DA.Empleo.Presentacion.CentralAdmin.Extensions
{
    public static class ExtensionesAutenticacion
    {
        
        /// <summary>
        /// Obtener los datos del usuario autenticado
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public static UsuarioAutenticadoViewModel ServidorAutenticado(this HttpContext httpContext)
        {
            // if (httpContext.User.Claims.Count() > 0 && httpContext.User.Claims.Where(c => c.Type == "AuthenticatedUser") != null && httpContext.User.Claims.Where(c => c.Type == "AuthenticatedUser").Count() > 0)
            //   return JsonConvert.DeserializeObject<UsuarioAutenticadoViewModel>(httpContext.User.Claims.Where(c => c.Type == "AuthenticatedUser")?.FirstOrDefault()?.Value);
            if (httpContext.Session.GetString("AuthenticatedUser") != null)
            {
                return JsonConvert.DeserializeObject<UsuarioAutenticadoViewModel>(httpContext.Session.GetString("AuthenticatedUser"));
            }


            else
                return new UsuarioAutenticadoViewModel();
                //return null;
        }


        public static string RolServidorInformacionPersonal(this HttpContext httpContext)
        {
            if (httpContext.User.Claims.Count() > 0)
               // return httpContext.User.Claims.Where(c => c.Type == ClaimTypes.Role)?.FirstOrDefault()?.Value;
            return httpContext.Session.GetString("Rol");
            else
                return "";

        }

        public static List<RolUsuario> RolesServidorInformacionPersonal(this HttpContext httpContext)
        {
           // string rolJson = httpContext.Session.GetString("VariablesSesion.ROLES_SERVIDOR");
            string rolJson = httpContext.Session.GetString("Rol");
            if (!string.IsNullOrEmpty(rolJson))
                return JsonConvert.DeserializeObject<List<RolUsuario>>(rolJson);
            else
                return null;

        }

        public static RolUsuario RolActivoServidorInformacionPersonal(this HttpContext httpContext)
        {
            var roles = RolesServidorInformacionPersonal(httpContext);
            if (roles?.Count > 0)
            {
                var rolActivo = roles.Where(filtro => filtro.Desde>=DateTime.Now && filtro.Hasta<=DateTime.Now).FirstOrDefault();
                return rolActivo;
            }

            return null;
        }

        /// <summary>
        /// Obtener los datos del usuario autenticado
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public static ServidorInformacionPersonal ServidorInformacionPersonal(this HttpContext httpContext)
        {
            var servidorJson = httpContext.User.Claims.Where(c => c.Type == "DatosServidor")?.FirstOrDefault()?.Value;
            if (string.IsNullOrEmpty(servidorJson))
            {
                return null;
            }

            var servidor = JsonConvert.DeserializeObject<ServidorInformacionPersonal>(servidorJson);


         

            return servidor;
        }




    }
}