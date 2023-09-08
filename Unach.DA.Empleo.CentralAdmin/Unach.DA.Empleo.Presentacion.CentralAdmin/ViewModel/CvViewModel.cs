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
        public List<CvFormacionAcademicaViewModel> FormacionAcademica { get; set; }
        public List<CvExperienciaLaboralViewModel> ExperienciaLaboral { get; set; }
        public List<CvLogroViewModel> Logros { get; set; }
        public List<CvNombreIdiomaViewModel> Idiomas { get; set; }
        public List<CvCapacitacionesViewModel> Capacitaciones { get; set; }

    }
}
