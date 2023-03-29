using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Unach.DA.Empleo.Presentacion.CentralAdmin.Utils.Validators;
namespace Unach.DA.Empleo.Presentacion.CentralAdmin.ViewModel
{
    public class DocumentoViewModel
    {
        public DocumentoViewModel()
        {

        }

        public int Id { get; set; }


        [Required(ErrorMessage = "*")]
        [Display(Name = "Tipo", Prompt = "Ej. Resolución")]
        [Range(0, Int32.MaxValue, ErrorMessage = "*")]
        public int IdTipoDocumento { get; set; }


        [Required(ErrorMessage = "*")]
        [Display(Name = "Organismo Emisor", Prompt = "Ej. Concejo Universitario")]
        [Range(0, Int32.MaxValue, ErrorMessage = "*")]
        public int IdOrganismoEmisor { get; set; }


        [Required(ErrorMessage = "*")]
        [Display(Name = "Estado", Prompt = "Ej. Activo/Inactivo")]
        [Range(0, Int32.MaxValue, ErrorMessage = "*")]
        public int IdEstado { get; set; }



        [Required(ErrorMessage = "*")]
        [Display(Name = "Título", Prompt = "Ej. Resolución No. SJAS-22-23-2022-UNACH.")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Descripción", Prompt = "Ej. Aprobar los proyectos de las Maestrías Académicas con Trayectoria Profesional en el campo de la salud ...")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Fecha de Sesión", Prompt = "Ej. 22/05/2022")]
        public DateTime FechaSesion { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Fecha de Vigencia", Prompt = "Ej. 23/01/2023")]
        public DateTime FechaVigencia { get; set; }
        [StringLength(250)]


        [Required(ErrorMessage = "*")]
        [Display(Name = "No. Documento", Prompt = "Ej. SJAS-SO-02-No.034-2023")]
        public string NumeroDocumento { get; set; }
        public string Archivo { get; set; }



        public string TipoDocumento { get; set; }
        public string OrganismoEmisor { get; set; }
        public string Estado { get; set; }


        [ValidadorExtensionesArchivos(".pdf", ErrorMessage = "Solo es aceptado el formato PDF")]
        [Display(Name = "Archivo", Prompt = "Archivo")]
        [DataType(DataType.Upload)]
        public IFormFile Certifcado { get; set; }

        public IFormFile CertificadoModificado { get; set; }

        public string PathArchivo { get; set; }



    }
}

