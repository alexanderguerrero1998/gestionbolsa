using Unach.DA.Empleo.Persistencia.Core.Models;
namespace  Unach.DA.Empleo.Presentacion.CentralAdmin.ViewModel
{
    public class PerfilServidorEditadoViewModel
    {
        public PerfilServidorEditadoViewModel()
        {
            Persona = new ServidorInformacionPersonal();

        }
        public ServidorInformacionPersonal Persona { get; set; }
        public List<Tuple<string, string>> Cargos { get; set; }

    }
}
