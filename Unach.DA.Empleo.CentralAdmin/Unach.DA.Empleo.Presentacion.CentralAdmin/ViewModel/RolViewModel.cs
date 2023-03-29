using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Unach.DA.Empleo.Persistencia.Core.Models;
namespace Unach.DA.Empleo.Presentacion.CentralAdmin.ViewModel
{
    public class RolViewModel
    {
        public RolViewModel()
        {
            Transacciones = new HashSet<Transaccion>();
            TransaccionesMenu = new List<TransaccioMenuViewModel>();
        }


        public int Id { get; set; }
        public int IdSistema { get; set; } = 1;

        [Required(ErrorMessage = "*")]
        [Display(Name = "Nombre", Prompt = "Ej. Analista")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Descripción", Prompt = "Ej. Permite el acceso a funciones XYZ en el sistema ABC.")]
        public string Descripcion { get; set; }


        [Display(Name = "¿Está Activo?", Prompt = "Ej. Activo")]
        public bool Activo { get; set; } = true;

        [BindProperty]
        public bool IsSelected { get; set; }

        public virtual ICollection<Transaccion> Transacciones { get; set; }

        public List<TransaccioMenuViewModel> TransaccionesMenu { get; set; }
    }
}
