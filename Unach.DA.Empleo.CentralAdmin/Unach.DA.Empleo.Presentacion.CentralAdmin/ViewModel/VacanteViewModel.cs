using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Unach.DA.Empleo.Persistencia.Core.Models;
using Unach.DA.Empleo.Persistencia.Core;
using Unach.DA.Empleo.Presentacion.CentralAdmin.Models;

namespace Unach.DA.Empleo.Presentacion.CentralAdmin.ViewModel
{
    public class VacanteViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Empresa", Prompt = "Ej. Investigación & Desarrollo")]
        public int IdEmpresa { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "Modalida", Prompt = "ejm. Presencial")]
        public int IdTipoVacante { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Plaza", Prompt = "Ejm. 7 Vacantes")]
        public int? Plaza { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Descripción", Prompt = "Ejm. Desarrollar una aplicacion")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Cargo", Prompt = "Ejm. Asistente Compras")]
        public string Cargo { get; set; }
        [Required(ErrorMessage = "*")]

        public string Cuidad { get; set; }
        [Required(ErrorMessage = "*")]
        [StringLength(250)]
        public string Sector { get; set; }
        [Required(ErrorMessage = "*")]
        [StringLength(250)]
        public string Contacto { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Tipo de Contrato", Prompt = "Ejm. Tiempo Completo")]
        [StringLength(250)]
        public string TipoContrato { get; set; }
        [Required(ErrorMessage = "*")]
        [StringLength(250)]
        public string Parroquia { get; set; }
        [Required(ErrorMessage = "*")]
        [StringLength(250)]
        public string Telefono { get; set; }
        [Required(ErrorMessage = "*")]
        [StringLength(250)]
        public string CorreoElectronico { get; set; }
        [Required(ErrorMessage = "*")]
        [StringLength(250)]
        public string Instruccion { get; set; }
        [Required(ErrorMessage = "*")]
        [StringLength(250)]
        public string AreaDeEstudios { get; set; }
        [Required(ErrorMessage = "*")]
        public string ConocimientosPrevios { get; set; }
        [Required(ErrorMessage = "*")]
        [StringLength(50)]
        public string Remuneracion { get; set; }
        [Required(ErrorMessage = "*")]
        public string Experiencia { get; set; }
        [Required(ErrorMessage = "*")]
        public string Actividades { get; set; }
        [Required(ErrorMessage = "*")]
        [StringLength(250)]
        public string Jornada { get; set; }
        [StringLength(250)]
        [Required(ErrorMessage = "*")]
        [Display(Name = "Area de Experiencia", Prompt = "ejm. MAESTRIA EN GEOGRAFIA APLICADA")]
        public string AreaExperiencia { get; set; }
        [StringLength(250)]
        [Required(ErrorMessage = "*")]
        [Display(Name = "Area de Capacitacion", Prompt = "ejm. MAESTRIA EN GEOGRAFIA APLICADA")]
        public string AreaCapacitacion { get; set; }
        [Required(ErrorMessage = "*")]

        public string Modalidad { get; set; }
 
        public List<Empresa> Empresas { get; set; }
        public List<TipoVacante> Tiposvacante { get; set; }
        public List<ApiCarreras> Carreras { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Area de Estudios", Prompt = "ejm. MAESTRIA EN GEOGRAFIA APLICADA")]
        public int IdTipoCarrera  { get; set; }
    }
}
