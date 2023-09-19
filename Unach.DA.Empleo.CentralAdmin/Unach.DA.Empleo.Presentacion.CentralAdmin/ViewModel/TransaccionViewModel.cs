using System.ComponentModel.DataAnnotations;
using Unach.DA.Empleo.Persistencia.Core.Models;

namespace Unach.DA.Empleo.Presentacion.CentralAdmin.ViewModel
{
    public class TransaccionViewModel
    {
        public int Id { get; set; }
        public int? IdPadre { get; set; }
        public int IdModulo { get; set; }
        [Required]
        [StringLength(250)]
        public string Titulo { get; set; }
        [Required]
        [StringLength(250)]
        public string Descripcion { get; set; }
        [Required]
        [StringLength(250)]
        public string Controlador { get; set; }
        [Required]
        [StringLength(250)]
        public string Accion { get; set; }
        [Required]
        [StringLength(250)]
        public string IconClass { get; set; }
        public int Orden { get; set; }
        public bool Estado { get; set; }
        public bool Visible { get; set; }
        public bool? Activo { get; set; }
        public List<Modulo> ListaModulos { get; set; }
}
}
