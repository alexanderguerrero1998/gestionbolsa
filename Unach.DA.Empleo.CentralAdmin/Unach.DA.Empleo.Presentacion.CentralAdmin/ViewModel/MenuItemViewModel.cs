using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Unach.DA.Empleo.Presentacion.CentralAdmin.ViewModel
{
    public class MenuItemViewModel
    {
        public MenuItemViewModel()
        {
            InverseIdPadreNavigation = new HashSet<MenuItemViewModel>();

        }

        public int Id { get; set; }
        public int? IdPadre { get; set; }
        public string Titulo { get; set; }
        public string Controlador { get; set; }
        public string Accion { get; set; }
        public string IconClass { get; set; }
        public int Orden { get; set; }

        public virtual MenuItemViewModel IdPadreNavigation { get; set; }
        public virtual ICollection<MenuItemViewModel> InverseIdPadreNavigation { get; set; }

    }
}

