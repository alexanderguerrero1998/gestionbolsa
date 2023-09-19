using Unach.DA.Empleo.Persistencia.Core.Models;

namespace Unach.DA.Empleo.Presentacion.CentralAdmin.ViewModel
{
    public class PerfilUsuarioEditadoViewModel
    {
        public PerfilUsuarioEditadoViewModel() 
        {
            Persona = new UsuarioInformacionPersonal();
        }
        public UsuarioInformacionPersonal Persona { get; set; }
        public List<Tuple<string, string>> Cargos { get; set; }
    }
}
