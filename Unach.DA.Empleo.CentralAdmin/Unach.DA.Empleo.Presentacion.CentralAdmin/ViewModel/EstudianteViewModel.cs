using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Unach.DA.Empleo.Presentacion.CentralAdmin.ViewModel
{
    public class EstudianteViewModel
    {
        public int IdEstudiante { get; set; }

        public string LinkLinkeding { get; set; }



        public int EstudianteID { get; set; }
        public string DocumentoIdentidad { get; set; }
        public string Nombres { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string Genero { get; set; }
        public string CorreoInstitucional { get; set; }
        public string TelefonoCelular { get; set; }
        public string TelefonoDomicilio { get; set; }
        public string Facultad { get; set; }
        public string Carrera { get; set; }
        public string Nivel { get; set; }
        public string Periodo { get; set; }
    }
}
