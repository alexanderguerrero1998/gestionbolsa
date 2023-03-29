//using RazorLight;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace Unach.Dath.Gestion.Presentacion.AdministracionCentral.Utils
//{
//    public class ReportUtils
//    {
//        public static string GetTemplateRendered<T>(string template, T model) where T : class
//        {
//            var engine = new RazorLightEngineBuilder()
//                         // required to have a default RazorLightProject type, but not required to create a template from string.
//                         .UseEmbeddedResourcesProject(typeof(Program))
//                         .UseMemoryCachingProvider()
//                         .Build();            
//            string result = engine.CompileRenderStringAsync(".", template, model).Result;
//            return result;
//        }
//    }
//}
