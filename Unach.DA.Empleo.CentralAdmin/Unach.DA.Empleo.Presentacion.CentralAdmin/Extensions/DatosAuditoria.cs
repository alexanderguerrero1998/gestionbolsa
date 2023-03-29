//using AutoMapper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Unach.DA.Empleo.Presentacion.CentralAdmin.Extensions
{
    public static class DatosAuditoria
    {
        //public static void AgregarDatosAuditoria<TDestination>(this IMapper mapper,
        //    TDestination destino,
        //    HttpContext context)
        //{
        //    mapper.Map<AuditoriaViewModel, TDestination>(new AuditoriaViewModel()
        //    {
        //        IdUsuarioAudd = context.ServidorAutenticado().IdServidor.ToString(),
        //        RolAudd = string.Join(",", context.ServidorAutenticado().Roles.Select(x => x.Nombre)),
        //        DireccionIpAudd = context.Connection.RemoteIpAddress.ToString(),
        //        SistemaAudd = "DATH-CentralAdmin",
        //        FechaTransaccion = DateTime.Now
        //    },
        //       destino
        //   );
        //}







    }
}
