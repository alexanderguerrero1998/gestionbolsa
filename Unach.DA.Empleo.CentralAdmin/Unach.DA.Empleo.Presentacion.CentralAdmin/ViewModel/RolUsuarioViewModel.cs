using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace Unach.DA.Empleo.Presentacion.CentralAdmin.ViewModel
{
    public class RolUsuarioViewModel
    {
    
        public int IdRol { get; set; }
 
        public string IdUsuario { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime Desde { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime Hasta { get; set; }
       
    }
}
