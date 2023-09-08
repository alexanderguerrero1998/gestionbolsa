using System.ComponentModel.DataAnnotations;
using Unach.DA.Empleo.Presentacion.CentralAdmin.Models;

namespace Unach.DA.Empleo.Presentacion.CentralAdmin.ViewModel
{
    public class NotificarCorreoViewModel
    {
        public int Id { get; set; } 
        public string Mensaje { get; set; }
      
        public List<ApiCarreras> ListaCarreras { get; set; }
        public int IdTipoCarrera { get; set; }

    }
}
