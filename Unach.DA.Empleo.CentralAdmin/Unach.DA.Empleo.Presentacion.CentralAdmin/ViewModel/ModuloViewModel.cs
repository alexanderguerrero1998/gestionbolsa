using System.ComponentModel.DataAnnotations;

namespace Unach.DA.Empleo.Presentacion.CentralAdmin.ViewModel
{
    public class ModuloViewModel
    {
        public int Id { get; set; }
        public int IdSistema { get; set; }
        [Required]
        [StringLength(250)]
        public string Nombre { get; set; }
        [Required]
        public string Descripcion { get; set; }
    }
}
