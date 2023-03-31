using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Unach.DA.Empleo.Presentacion.CentralAdmin.ViewModel
{
    public class EstudianteViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public string Contrasenia { get; set; }
        }
}
