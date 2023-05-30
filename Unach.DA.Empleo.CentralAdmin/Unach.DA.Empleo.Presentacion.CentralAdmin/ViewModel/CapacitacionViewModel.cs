using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Unach.DA.Empleo.Presentacion.CentralAdmin.ViewModel
{
    public class CapacitacionViewModel
    {
        public int Id { get; set; }
        public int IdCapacitacion { get; set; }
        public int IdEstudiante { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaIncio { get; set; }
        public DateTime FechaFin { get; set; }
        public string Certificado { get; set; }

    }
}
