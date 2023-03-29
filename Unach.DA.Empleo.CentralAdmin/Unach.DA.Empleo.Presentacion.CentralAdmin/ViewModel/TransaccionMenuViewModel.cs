using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Unach.DA.Empleo.Presentacion.CentralAdmin.ViewModel
{
    public class TransaccioMenuViewModel
    {
        public int Id { get; set; }
        public int? IdPadre { get; set; }
        
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public string Controlador { get; set; }
        public string Accion { get; set; }
        public string IconClass { get; set; }
        public int Orden { get; set; }
        public bool Activo { get; set; }
        public bool Visible { get; set; }
    }
}
