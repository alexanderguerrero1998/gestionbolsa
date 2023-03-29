//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Unach.Dath.Gestion.Services.Email;
//using System.IO;
//using RazorLight;
//using Unach.Dath.Gestion.Presentacion.AdministracionCentral.ViewModel;
//using Unach.Dath.Gestion.Domain.Core;
//using Microsoft.EntityFrameworkCore;
//using Unach.Dath.Gestion.Persistence.Core.Models;
//using Microsoft.AspNetCore.Http;
//using Unach.Dath.Gestion.Presentacion.AdministracionCentral.Extensions;
//using Unach.Dath.Gestion.Domain.Core.Entities.Tipos;
//using Unach.Dath.Gestion.Domain.Core.Entities;
//using Unach.Dath.Gestion.Infrastructure.Core.Extensions;

//namespace Unach.Dath.Gestion.Presentacion.AdministracionCentral.Utils.Communications
//{
//    public class NotificacionesAccionesPersonal
//    {
//        private readonly IEmailSender _emailSender;
//        EntitiesDomain entitiesDomain;

//        public NotificacionesAccionesPersonal(DbContextOptions<DathGestionContext> options, IEmailSender emailSender)
//        {
//            _emailSender = emailSender;
//            entitiesDomain = new EntitiesDomain(options);
//        }

    

//        public void EnviarEmail(HttpContext httpContext, int idAccionPersonal)
//        {
//            var apNotificar = entitiesDomain.AccionPersonalRepositorio.ObtenerTodosEnOtraVista<AccionPersonalNotificarViewModel>(
//                m => new AccionPersonalNotificarViewModel
//                {
//                    Id = m.Id,
//                    IdEstado = m.EstadoAccionPersonal.OrderBy(x => x.Id).Select(y => y.IdTipoEstadoAccionPersonal).LastOrDefault(),
//                    Numero = m.Numero,
//                    Observacion = m.EstadoAccionPersonal.OrderBy(x => x.Id).Select(y => y.Observacion).LastOrDefault(),
//                    IdTipoDocumentoContractual = m.IdTipoAccionPersonalNavigation.IdTipoDocumentoContractual
//                },
//                x => x.Id == idAccionPersonal
//                ).FirstOrDefault();

//            if (apNotificar != null)
//            {
//                List<ServidorNotificar> correosNotificar = new List<ServidorNotificar>();

//                if (apNotificar.IdEstado == (int)TiposEstadoAccionPersonal.EnRevision && apNotificar.IdTipoDocumentoContractual== 4)
//                    correosNotificar = entitiesDomain.ExecuteStoredProcedure<ServidorNotificar>("Dath.GetNotificarUsuarioRevisarAccionesPersonal_CentralAdmin").ToList();

//                if (apNotificar.IdEstado == (int)TiposEstadoAccionPersonal.Revisado && apNotificar.IdTipoDocumentoContractual == 4)
//                    correosNotificar = entitiesDomain.ExecuteStoredProcedure<ServidorNotificar>("Dath.GetNotificarUsuarioValidarAccionesPersonal_CentralAdmin").ToList();

//                if (apNotificar.IdEstado == (int)TiposEstadoAccionPersonal.EnRevision && apNotificar.IdTipoDocumentoContractual == 6)
//                    correosNotificar = entitiesDomain.ExecuteStoredProcedure<ServidorNotificar>("Dath.GetNotificarUsuarioRevisarAccionesPersonalAdministrativas_CentralAdmin").ToList();

//                if (apNotificar.IdEstado == (int)TiposEstadoAccionPersonal.Revisado && apNotificar.IdTipoDocumentoContractual == 6)
//                    correosNotificar = entitiesDomain.ExecuteStoredProcedure<ServidorNotificar>("Dath.GetNotificarUsuarioValidarAccionesPersonalAdministrativas_CentralAdmin").ToList();


//                foreach (var item in correosNotificar)
//                {
//                    apNotificar.IdServidor = item.IdServidor;
//                    apNotificar.NombreRevisor = item.Nombres.ToCapitalize();
//                    apNotificar.NombreEstado = apNotificar.IdEstado switch
//                    {
//                        2 => "Validación",
//                        4 => "Validado",
//                        5 => "Revisión",
//                        _ => ""
//                    };

//                    if (apNotificar.IdEstado == 2 || apNotificar.IdEstado == 5)
//                    {
//                        string contents = GetTemplate(apNotificar);
//                        contents = contents.Replace("*at*", "@");
//                        string[] to = new string[] { item.Email, httpContext.ServidorAutenticado().Email };
//                        Message message = new Message(to, $"{apNotificar.NombreEstado} - Acción de Personal", contents, null);
//                        _emailSender.SendEmail(message);
//                    }

//                }
//            }
//        }



//        string GetTemplate(AccionPersonalNotificarViewModel model)
//        {
//            var engine = new RazorLightEngineBuilder()
//                         // required to have a default RazorLightProject type, but not required to create a template from string.
//                         .UseEmbeddedResourcesProject(typeof(Program))
//                         .UseMemoryCachingProvider()
//                         .Build();

//            string rutaSinUsuario = $"{Directory.GetCurrentDirectory()}{@"\wwwroot\public\templates\RevisarAPTemplate.html"}";
//            string template = File.ReadAllText(rutaSinUsuario);
//            string result = engine.CompileRenderStringAsync(".", template, model).Result;

//            return result;
//        }

//    }
//}
