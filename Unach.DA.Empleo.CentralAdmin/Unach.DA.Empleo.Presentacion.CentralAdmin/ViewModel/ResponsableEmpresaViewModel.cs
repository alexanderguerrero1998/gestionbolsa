using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Unach.DA.Empleo.Persistencia.Core.Models;

namespace Unach.DA.Empleo.Presentacion.CentralAdmin.ViewModel
{
    public class ResponsableEmpresaViewModel
    {
        public int Id { get; set; }
        public int IdEmpresa { get; set; }
        public int IdTipoUsuario { get; set; }
        public DateTime Fecha { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string IdRepresentante { get; set; }
    }
}
