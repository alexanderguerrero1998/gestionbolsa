
namespace Unach.DA.Empleo.Presentacion.CentralAdmin.ViewModel
{
    public class ParametrosBusquedaViewModel
    {
        //public List<TipoDocumento>  TiposDocumento{ get; set; }
        public int IdTipoDocumento { get; set; }
        public string Criterio { get; set; }
        public DateTime Desde { get; set; }
        public DateTime Hasta { get; set; }
    }
}
