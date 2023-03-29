using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;
using Unach.DA.Empleo.Presentacion.CentralAdmin.ViewModel;

namespace Unach.DA.Empleo.Presentacion.CentralAdmin.Extensions
{
    public static class AlertasExtensiones
    {
        public static void MostrarAlerta(this ITempDataDictionary TempData, TipoAlerta tipoAlerta, string mensaje)
        {
            var val = new AlertaViewModel(tipoAlerta, mensaje);
            TempData["notification"] = JsonConvert.SerializeObject(val);
        }


    }
}
