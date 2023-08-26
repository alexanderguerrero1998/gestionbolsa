using System.ComponentModel.DataAnnotations;
using Unach.DA.Empleo.Persistencia.Core.Models;

namespace Unach.DA.Empleo.Presentacion.CentralAdmin.ViewModel
{
    public class EstudianteIdiomaViewModel
    {
        public int Id { get; set; }
  
        public string IdEstudiante { get; set; }
        public int IdIdioma { get; set; }
        public int NivelWriting { get; set; }
        public int NivelSpeaking { get; set; }
        public int NivelListening { get; set; }
        [Required]
        public string Certificado { get; set; }
        public List<Idioma> tipoIdioma { get; set; }
    }
}
