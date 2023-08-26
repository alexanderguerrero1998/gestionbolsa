using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Unach.DA.Empleo.Dominio.Core;
using Unach.DA.Empleo.Persistencia.Core.Models;
using Unach.DA.Empleo.Presentacion.CentralAdmin.ViewModel;
using System.Net;
using System.Net.Mail;
using Unach.DA.Empleo.Presentacion.CentralAdmin.Extensions;
using Unach.DA.Empleo.Presentacion.CentralAdmin.Models;
using Microsoft.AspNetCore.Http.Extensions;
using Rotativa.AspNetCore;
using Unach.DA.Empleo.Presistencia.Api;

namespace Unach.DA.Empleo.Presentacion.CentralAdmin.Controllers
{
    public class CvController : Controller
    {
        EntitiesDomain entitiesDomain;
        private ILogger logger;
        private readonly IMapper _mapper;
       

        public CvController(DbContextOptions<SicoaContext> options, IMapper mapper, ILoggerFactory log)
       
        {
            _mapper = mapper;
            entitiesDomain = new EntitiesDomain(options);
            logger = log.CreateLogger(typeof(CvController));
          
        }
        public IActionResult Index()
        {
            
            return View();
        }


        public IActionResult ViewX()
        {
            var idEstudiante = HttpContext.Session.GetString("IdServidor");
            if (idEstudiante != null)
            {
                CvViewModel cv = new CvViewModel();
                ApiInfoAcademico clienteapi = new ApiInfoAcademico("");

                var dataestudiante = entitiesDomain.EstudianteRepositorio.ObtenerTodos().ToList();
                // Buscar el estudiante por su ID y seleccionar su IdEstudiante
                var id = dataestudiante.FirstOrDefault(estudiante => estudiante.Id == idEstudiante)?.IdEstudiante;

                var apiUrl = "https://pruebas.unach.edu.ec:4431/api/Estudiante/InformacionAcademica/" + id;
                var datosApi = clienteapi.Get<ApiInformacionAcademica>(apiUrl);

                var logrosQuery = entitiesDomain.LogroRepositorio.ObtenerTodos().ToList();
                var experiencialaboralQuery = entitiesDomain.ExperienciaLaboralRepositorio.ObtenerTodos().ToList();
                var formacionAcademicaQuery = entitiesDomain.FormacionAcademicaRepositorio.ObtenerTodos().ToList();
                var capacitacionQuery = entitiesDomain.CapacitacionRepositorio.ObtenerTodos().ToList();
                var idiomaQuery = entitiesDomain.EstudianteIdiomaRepositorio.ObtenerTodos().ToList();

                // Filtramos los datos y pasamos al ViewModel

                cv.Nombre = datosApi.Nombres;
                cv.Apellido = datosApi.ApellidoPaterno;
                cv.Carrera = datosApi.Carrera;
                cv.Logros = logrosQuery.Where(logro => logro.IdEstudiante == idEstudiante).ToList();
                cv.ExperienciaLaboral = experiencialaboralQuery.Where(experiencialaboral => experiencialaboral.IdEstudiante == idEstudiante).ToList();
                cv.FromacionAcademica = formacionAcademicaQuery.Where(formacionAcademica => formacionAcademica.IdEstudiante == idEstudiante).ToList();
                cv.Capacitaciones = capacitacionQuery.Where(capacitacion => capacitacion.IdEstudiante == idEstudiante).ToList();
                cv.Idiomas = idiomaQuery.Where(idioma => idioma.IdEstudiante == idEstudiante).ToList();

                return new ViewAsPdf("ViewX", cv)
                {
                    PageSize = Rotativa.AspNetCore.Options.Size.A4,
                    PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                };

            }
            return new ViewAsPdf("ViewX")
            {
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
            };
            //return View();

        }

        public IActionResult ViewY()
        {
            //return new ViewAsPdf("ViewY")
            //{
            //    PageSize = Rotativa.AspNetCore.Options.Size.A4,
            //    PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
            //};
            return View();
        }

        }
}
