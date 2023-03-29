using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Unach.DA.Empleo.Presentacion.CentralAdmin.ViewModel
{
    public class PostulacionesViewModel
    {
        public int Id { get; set; }
        public int IdEstudiante { get; set; }
        public int IdEmpresa { get; set; }
        public int IdVacante { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime Fecha { get; set; }
        [Required]
        [StringLength(100)]
        public string IdUsuarioAudd { get; set; }
        [Required]
        public string RolAudd { get; set; }
        [Required]
        [StringLength(50)]
        public string DireccionlpAudd { get; set; }
        [Required]
        [StringLength(250)]
        public string SistemaAudd { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime FechaTransaccion { get; set; }
    }
}
