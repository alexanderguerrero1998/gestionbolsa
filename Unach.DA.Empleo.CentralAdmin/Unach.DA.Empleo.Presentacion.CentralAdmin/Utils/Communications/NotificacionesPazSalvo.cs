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
//using Unach.Dath.Gestion.Presentacion.AdministracionCentral.ViewModel.PazSalvo;

//namespace Unach.Dath.Gestion.Presentacion.AdministracionCentral.Utils.Communications
//{
//    public class NotificacionesPazSalvo
//    {
//        private readonly IEmailSender _emailSender;
//        EntitiesDomain entitiesDomain;

//        public NotificacionesPazSalvo(DbContextOptions<DathGestionContext> options, IEmailSender emailSender)
//        {
//            _emailSender = emailSender;
//            entitiesDomain = new EntitiesDomain(options);
//        }

    

//        public void EnviarEmail(HttpContext httpContext, ServidorDesvincularseViewModel servidorDesvincularse)
//        {
           
//                List<ServidorCertificadorViewModel> correosNotificar =correosNotificar = entitiesDomain.ExecuteStoredProcedure<ServidorCertificadorViewModel>("Dath.GetServidoresCertificadores_CentralAdmin").ToList();


//                foreach (var item in correosNotificar)
//                {
                   
//                        string contents = GetTemplate(servidorDesvincularse);
//                        //contents = contents.Replace("*at*", "@");
//                        string[] to = new string[] { item.Email, httpContext.ServidorAutenticado().Email };
//                        Message message = new Message(to, $"Solicitud de Desvinculación Laboral", contents, null);
//                        _emailSender.SendEmail(message);
                   

//                }
           
//        }



//        string GetTemplate(ServidorDesvincularseViewModel model)
//        {
//            var engine = new RazorLightEngineBuilder()
//                         // required to have a default RazorLightProject type, but not required to create a template from string.
//                         .UseEmbeddedResourcesProject(typeof(Program))
//                         .UseMemoryCachingProvider()
//                         .Build();

//            string rutaSinUsuario = $"{Directory.GetCurrentDirectory()}{@"\wwwroot\public\templates\Modelo_Solicitud_Desvinculacion.html"}";
//            string template = File.ReadAllText(rutaSinUsuario);
//            string result = engine.CompileRenderStringAsync(".", template, model).Result;

//            return result;
//        }

//    }
//}
