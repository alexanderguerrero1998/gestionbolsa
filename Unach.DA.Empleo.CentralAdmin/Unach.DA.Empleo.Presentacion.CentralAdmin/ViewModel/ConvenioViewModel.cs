using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Unach.DA.Empleo.Persistencia.Core.Models;
namespace Unach.DA.Empleo.Presentacion.CentralAdmin.ViewModel
{
    public class ConvenioViewModel
    {
        public int Id { get; set; }
        public int IdInstrumento { get; set; }
        public int IdPais { get; set; }
        public int IdTipoConvenio { get; set; }
        public int IdAmbito { get; set; }
        public int IdClaseConvenio { get; set; }
        public string Nombre { get; set; }
        public string Objeto { get; set; }
        public int Duracion { get; set; }
        public DateTime Suscripcion { get; set; }
        public DateTime Expiracion { get; set; }
        public int Estado { get; set; }

        public string Instrumento { get; set; }
        public string Pais { get; set; }
        public string TipoConvenio { get; set; }
        public string Ambito { get; set; }
        public string ClaseConvenio { get; set; }



    
        public virtual List<Administrador> AdministradoresInternos { get; set; }
        public virtual List<Administrador> AdministradoresContraparte { get; set; }
        public virtual List<Dependencia> Dependencias { get; set; }


        public virtual List<ClaseConvenio> Clases { get; set; }
        public virtual List<TipoConvenio> TiposConvenio { get; set; }
        public virtual List<Instrumento> Instrumentos { get; set; }
        public virtual List<Pais> Paises { get; set; }
        public virtual List<AmbitoInstrumento> Ambitos { get; set; }
    }
}
