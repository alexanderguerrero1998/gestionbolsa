using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Unach.DA.Empleo.Presentacion.CentralAdmin.ViewModel
{
    public class PostulacionesViewModel
    {
        public int Id { get; set; }
        public string IdEstudiante { get; set; }
        public int IdVacante { get; set; }
        public DateTime Fecha { get; set; }
       


    }
}
