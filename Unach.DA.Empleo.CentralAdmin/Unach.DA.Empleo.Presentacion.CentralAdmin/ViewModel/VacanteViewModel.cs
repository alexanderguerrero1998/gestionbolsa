using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Unach.DA.Empleo.Persistencia.Core.Models;
using Unach.DA.Empleo.Persistencia.Core;

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

        public string Modalidad { get; set; }
        public List<Empresa> Empresas { get; set; }
        public List<TipoVacante> Tiposvacante { get; set; }
    }
}
