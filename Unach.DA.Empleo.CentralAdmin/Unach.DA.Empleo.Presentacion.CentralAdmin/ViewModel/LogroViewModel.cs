using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Unach.DA.Empleo.Persistencia.Core.Models;

namespace Unach.DA.Empleo.Presentacion.CentralAdmin.ViewModel
{
    public class LogroViewModel
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public DateTime Fecha { get; set; }

        [Display(Name = "Logros", Prompt = "Ejm. Concurso")]
        public int IdLogro { get; set; }
        public int IdEstudiante { get; set; }
        public string Institucion { get; set; }
        [Required(ErrorMessage = "*")]
        public string Certificado { get; set; }
        public string NombreLogro { get; set; }

        public string NombreEstudiante { get; set; } // Este campo se puede borrar par ala vista de estudiante y agregar en la del administrador
        public List<TipoLogro> Logros { get; set; }
       
        public List<Estudiante> Estudiantes { get; set; }

    }
}
