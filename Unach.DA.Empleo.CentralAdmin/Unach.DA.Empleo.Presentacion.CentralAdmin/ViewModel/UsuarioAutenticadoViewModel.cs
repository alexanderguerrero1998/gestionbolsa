using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace Unach.DA.Empleo.Presentacion.CentralAdmin.ViewModel
{
    public class UsuarioAutenticadoViewModel
    {
        public UsuarioAutenticadoViewModel()
        {
            MenuItems = new List<MenuItemViewModel>();
            Roles = new List<RolViewModel>();
        }
        public string IdServidor { get; set; }
        public string NombresCompletos { get; set; }
        public string Nombres { get; set; }
        public string Dependencia { get; set; }
        public string Cargo { get; set; }
        public string Foto { get; set; }
        public string Email { get; set; }
        public string Identificacion { get; set; }
        public int EsSoloFuncionario { get; set; }
        public int EsSoloUsuario { get; set; }

        public List<RolViewModel> Roles { get; set; }
        public List<MenuItemViewModel> MenuItems { get; set; }
    }
}
