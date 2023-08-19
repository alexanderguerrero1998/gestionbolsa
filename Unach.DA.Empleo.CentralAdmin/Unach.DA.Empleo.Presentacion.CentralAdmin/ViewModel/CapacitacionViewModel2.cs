using Unach.DA.Empleo.Persistencia.Core.Models;

namespace Unach.DA.Empleo.Presentacion.CentralAdmin.ViewModel
{
    public class CapacitacionViewModel2
    {
        public int Id { get; set; }
        public int IdCapacitacion { get; set; }
        public string IdEstudiante { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaIncio { get; set; }
        public DateTime FechaFin { get; set; }
        public string Certificado { get; set; }
        public string Nombre { get; set; }

        public List<TipoCapacitacion> TiposCapacitacion { get; set; }
    }
}
