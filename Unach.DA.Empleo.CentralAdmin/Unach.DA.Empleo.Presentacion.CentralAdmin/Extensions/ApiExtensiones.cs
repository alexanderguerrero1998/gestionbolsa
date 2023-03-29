using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Unach.DA.Empleo.Presentacion.CentralAdmin.Extensions
{
    public static class ApiExtensiones
    {

        public static void LoadApiAcademicoSettings(IConfiguration Configuration)
        {
            //ApiAcademicoSettings.Instance.ApiAcademicoUrl = Configuration.GetValue<string>("ApiAcademicoSettings:URL_API");
            //ApiAcademicoSettings.Instance.DocentesDedicacion = Configuration.GetValue<string>("ApiAcademicoSettings:DOCENTES_DEDICACION");


            //ApiReportesSettings.Instance.ApiReportesUrl = Configuration.GetValue<string>("ApiReportesSettings:URL_API");
            //ApiReportesSettings.Instance.GenerarDocumento = Configuration.GetValue<string>("ApiReportesSettings:CONVERTIR_PDF");

        }

        public static void LoadMyApplicationSettings(IConfiguration Configuration)
        {
            //MyApplicationSettings.Instance.UrlFotoServidorDocumentos = Configuration.GetValue<string>("MyApplicationSettings:URL_FOTO_SERVIDOR_DOCUMENTOS");
            //MyApplicationSettings.Instance.UrlServidorDocumentos = Configuration.GetValue<string>("MyApplicationSettings:URL_SERVIDOR_DOCUMENTOS");
        }
    }
}
