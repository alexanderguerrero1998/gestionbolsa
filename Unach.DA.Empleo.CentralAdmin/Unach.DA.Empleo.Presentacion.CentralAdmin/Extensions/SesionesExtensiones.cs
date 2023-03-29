using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Unach.DA.Empleo.Presentacion.CentralAdmin.Extensions
{
    public static class SesionesExtensiones
    {
        public static void AgregarInt(this ISession session, string key, int value)
        {
            session.SetInt32(key, value);
        }

        public static void AgregarString(this ISession session, string key, string value)
        {
            session.SetString(key, value);
        }

        public static int ObtenerInt(this ISession session, string key)
        {
            return session.GetInt32(key) ?? -1;
        }

        public static string ObtenerString(this ISession session, string key)
        {
            return session.GetString(key);
        }

        public static void AgregarObjeto<T>(this ISession session, string key, T value)  where T : class
        {
            var valueString = JsonConvert.SerializeObject(value);
            session.SetString(key, valueString);
        }

        public static T ObtenerObjeto<T>(this ISession session, string key) where T : class
        {
            string valueString = session.GetString(key);
            if (string.IsNullOrEmpty(valueString))
            {
                return null;
            }
            return JsonConvert.DeserializeObject<T>(valueString);           
        }

        public static void BorrarVariable(this ISession session, string key)
        {
            session.Remove(key);
        }





        public static int GetSoloFuncionario(this HttpContext httpContext)
        {
            if (httpContext.Session.GetInt32("SOLO_FUNCIONARIO_NO_ADMINISTRADOR") != null)
            {
                return httpContext.Session.GetInt32("SOLO_FUNCIONARIO_NO_ADMINISTRADOR").Value;
            }
            return -1;
        }
        public static void SetSoloFuncionario(this HttpContext httpContext, int esFuncionario)
        {
            httpContext.Session.SetInt32("SOLO_FUNCIONARIO_NO_ADMINISTRADOR", esFuncionario);

        }



        public static int GetExpedienteModificar(this HttpContext httpContext)
        {
            if (httpContext.Session.GetInt32("EXPEDIENTE") != null)
            {
                return httpContext.Session.GetInt32("EXPEDIENTE").Value;
            }
            return -1;
        }


        public static void SetExpedienteModificar(this HttpContext httpContext,int expediente)
        {
            httpContext.Session.SetInt32("EXPEDIENTE", expediente);
            
        }



    }
}
