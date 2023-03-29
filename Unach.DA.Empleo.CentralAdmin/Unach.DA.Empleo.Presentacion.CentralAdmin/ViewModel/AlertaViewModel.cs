using Newtonsoft.Json;
using Newtonsoft.Json.Converters;


namespace Unach.DA.Empleo.Presentacion.CentralAdmin.ViewModel
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum TipoAlerta
    {
        Error = 0,
        Informacion = 1,
        Exitosa = 2,
        Advertencia = 3
    }
    public class AlertaViewModel
    {
        public TipoAlerta TipoAlerta { get; set; }
        public string Mensaje { get; set; }
        public AlertaViewModel(TipoAlerta tipoAlerta, string mensaje)
        {
            this.TipoAlerta = tipoAlerta;
            this.Mensaje = mensaje;
        }
    }
}
