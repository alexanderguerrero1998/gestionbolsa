using Unach.DA.Empleo.Persistencia.Core.Models;

namespace Unach.DA.Empleo.Presentacion.CentralAdmin.ViewModel
{
    public class CvViewModel
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public string Carrera { get; set; }
        public List<FormacionAcademica> FromacionAcademica { get; set; }
        public List<ExperienciaLaboral> ExperienciaLaboral { get; set; }
        public List<Logro> Logros { get; set; }
        public List<EstudianteIdioma> Idiomas { get; set; }
        public List<Capacitacion> Capacitaciones { get; set; }

    }
}
