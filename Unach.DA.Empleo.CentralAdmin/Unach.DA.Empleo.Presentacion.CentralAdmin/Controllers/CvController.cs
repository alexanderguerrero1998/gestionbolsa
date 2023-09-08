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
using System.Linq;

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


        public IActionResult Curriculum()
        {
            var idEstudiante = HttpContext.Session.GetString("IdServidor");
            if (idEstudiante != null)
            {
                CvViewModel cv = new CvViewModel();
                ApiInfoAcademico clienteapi = new ApiInfoAcademico("");

                // Obtiene todos los estudiantes de la taba 'EstudianteRepositorio'
                var dataestudiante = entitiesDomain.EstudianteRepositorio.ObtenerTodos().ToList();

                // Luego, busca el estudiante por su ID y seleccionar su IdEstudiante
                var id = dataestudiante.FirstOrDefault(estudiante => estudiante.Id == idEstudiante)?.IdEstudiante;

                // Luego, consulta en la Api
                var apiUrl = "https://pruebas.unach.edu.ec:4431/api/Estudiante/InformacionAcademica/" + id;

                // Y extrae todos sus datos
                var datosApi = clienteapi.Get<ApiInformacionAcademica>(apiUrl);

                // Obtenemos una lista de las diferentes formaciones del estudiante
                var aspNetUserQuery = entitiesDomain.AspNetUsersRepositorio.ObtenerTodos().ToList();

                #region Logros
                var logrosQuery = entitiesDomain.LogroRepositorio.ObtenerTodos().ToList();
                var tipologrosQuery = entitiesDomain.TipoLogroRepositorio.ObtenerTodos().ToList();
                var ListaLogro = (from logros in logrosQuery
                                  join tipologros in tipologrosQuery
                                  on logros.IdLogro equals tipologros.Id
                                  where logros.IdEstudiante == idEstudiante
                                  select new CvLogroViewModel
                                  {
                                      Nombre = tipologros.Nombre,
                                      Descripcion = logros.Descripcion,
                                      Empresa = logros.Institucion,
                                      FechaInicio = logros.FechaInicio,
                                      FechaFin = logros.FechaFin,   
                                  }).ToList();

                cv.Logros = ListaLogro;
                #endregion

                #region ExperiencaLaboral
                var experiencialaboralQuery = entitiesDomain.ExperienciaLaboralRepositorio.ObtenerTodos().ToList();
                var tipoexperiencialaboralQuery = entitiesDomain.TipoExperienciaLaboralRepositorio.ObtenerTodos().ToList();
                var ListaExperienciaLaboral = (from experiencialaboral in experiencialaboralQuery
                                               join tipoexperiencialaboral in tipoexperiencialaboralQuery
                                               on experiencialaboral.IdExperienciaLaboral equals tipoexperiencialaboral.Id
                                               where experiencialaboral.IdEstudiante == idEstudiante
                                               select new CvExperienciaLaboralViewModel
                                               {
                                                   Empresa = experiencialaboral.NombreEmpresa,
                                                   Nombre = tipoexperiencialaboral.Nombre,
                                                   Descripcion = experiencialaboral.Descripcion,
                                                   FechaInicio = experiencialaboral.FechaIncio,
                                                   FechaFin = experiencialaboral.FechaFin
                                               }).ToList();

                cv.ExperienciaLaboral = ListaExperienciaLaboral;
                #endregion

                #region Lista_formacionAcademica
                var formacionAcademicaQuery = entitiesDomain.FormacionAcademicaRepositorio.ObtenerTodos().ToList();
                var tipoformacionAcademicaQuery = entitiesDomain.TipoFormacionAcademicaRepositorio.ObtenerTodos().ToList();
                var ListaFormacionAcademica = (from formacionAcademica in formacionAcademicaQuery
                                               join tipoformacionAcademica in tipoformacionAcademicaQuery
                                               on formacionAcademica.IdFormacionAcademica equals tipoformacionAcademica.Id
                                               where formacionAcademica.IdEstudiante == idEstudiante
                                               select new CvFormacionAcademicaViewModel
                                               { 
                                                   Nombre = tipoformacionAcademica.Nombre,
                                                   Descripcion = formacionAcademica.Descripcion,
                                                   FechaInicio = formacionAcademica.FechaIncio,
                                                   FechaFin  = formacionAcademica.FechaFin,
                                                   Empresa = formacionAcademica.Empresa
                                               
                                               }).ToList();

                cv.FormacionAcademica = ListaFormacionAcademica;
                //TASKS
                // Poner como not null el campo 'Fecha Fin' 
         
                #endregion

                #region Lista_capacitaciones
                var capacitacionesQuery = entitiesDomain.CapacitacionRepositorio.ObtenerTodos().ToList();
                var tipocapacitacionesQuery = entitiesDomain.TipoCapacitacionRepositorio.ObtenerTodos().ToList();
                var ListaCapacitaciones = (from capacitaciones in capacitacionesQuery
                                           join tipocapacitaciones in tipocapacitacionesQuery
                                           on capacitaciones.IdCapacitacion equals tipocapacitaciones.Id
                                           where capacitaciones.IdEstudiante == idEstudiante
                                           select new CvCapacitacionesViewModel
                                           {
                                               Nombre = tipocapacitaciones.Nombre,
                                               Descripcion = capacitaciones.Descripcion,
                                               FechaInicio = capacitaciones.FechaIncio,
                                               FechaFin = capacitaciones.FechaFin,
                                               Empresa = capacitaciones.Empresa

                                           }).ToList();

                cv.Capacitaciones = ListaCapacitaciones;

                //TASKS
                //Poner el campo 'Empresa' en la tabla CAPACITACION
                // Poner el la descripcion un numero minimo de 130 caracteres y maximo de 150
                #endregion

                #region Lista_idiomas
                var idiomasQuery = entitiesDomain.EstudianteIdiomaRepositorio.ObtenerTodos().ToList(); 
                var tipoidiomaQuery = entitiesDomain.IdiomaRepositorio.ObtenerTodos().ToList();
                var ListaIdiomas = (from idiomas in idiomasQuery
                                  join tipoIdioma in tipoidiomaQuery
                                  on idiomas.IdIdioma equals tipoIdioma.Id
                                  where idiomas.IdEstudiante == idEstudiante
                                  select new CvNombreIdiomaViewModel 
                                  {
                                      Nombre = tipoIdioma.Nombre,
                                      PromedioIdioma = (idiomas.NivelWriting + idiomas.NivelListening + idiomas.NivelSpeaking)/3
                                  }).ToList();

                cv.Idiomas = ListaIdiomas;
                #endregion

                // Filtramos por ID los datos y pasamos al ViewModel
                cv.Nombre = datosApi.Nombres;
                cv.Apellido = datosApi.ApellidoPaterno;
                cv.Telefono = aspNetUserQuery.Where(x => x.Id == idEstudiante).Select(telefono => telefono.PhoneNumber).FirstOrDefault();
                cv.Email = aspNetUserQuery.Where(x=>x.Id == idEstudiante).Select(email=>email.Email).FirstOrDefault();
                cv.Carrera = datosApi.Carrera;
               
     

                return new ViewAsPdf("Curriculum", cv)
                {
                    PageSize = Rotativa.AspNetCore.Options.Size.A4,
                    PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                };

            }
            return new ViewAsPdf("ViewY")
            {
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
            };
            //return View();

        }
        public IActionResult DownloadCurriculum(string IdEstudiante) 
        {
            var idEstudiante = IdEstudiante;
            if (idEstudiante != null)
            {
                CvViewModel cv = new CvViewModel();
                ApiInfoAcademico clienteapi = new ApiInfoAcademico("");

                // Obtiene todos los estudiantes de la taba 'EstudianteRepositorio'
                var dataestudiante = entitiesDomain.EstudianteRepositorio.ObtenerTodos().ToList();

                // Luego, busca el estudiante por su ID y seleccionar su IdEstudiante
                var id = dataestudiante.FirstOrDefault(estudiante => estudiante.Id == idEstudiante)?.IdEstudiante;

                // Luego, consulta en la Api
                var apiUrl = "https://pruebas.unach.edu.ec:4431/api/Estudiante/InformacionAcademica/" + id;

                // Y extrae todos sus datos
                var datosApi = clienteapi.Get<ApiInformacionAcademica>(apiUrl);

                // Obtenemos una lista de las diferentes formaciones del estudiante
                var aspNetUserQuery = entitiesDomain.AspNetUsersRepositorio.ObtenerTodos().ToList();

                #region Logros
                var logrosQuery = entitiesDomain.LogroRepositorio.ObtenerTodos().ToList();
                var tipologrosQuery = entitiesDomain.TipoLogroRepositorio.ObtenerTodos().ToList();
                var ListaLogro = (from logros in logrosQuery
                                  join tipologros in tipologrosQuery
                                  on logros.IdLogro equals tipologros.Id
                                  where logros.IdEstudiante == idEstudiante
                                  select new CvLogroViewModel
                                  {
                                      Nombre = tipologros.Nombre,
                                      Descripcion = logros.Descripcion,
                                      Empresa = logros.Institucion,
                                      FechaInicio = logros.FechaInicio,
                                      FechaFin = logros.FechaFin,
                                  }).ToList();

                cv.Logros = ListaLogro;
                #endregion

                #region ExperiencaLaboral
                var experiencialaboralQuery = entitiesDomain.ExperienciaLaboralRepositorio.ObtenerTodos().ToList();
                var tipoexperiencialaboralQuery = entitiesDomain.TipoExperienciaLaboralRepositorio.ObtenerTodos().ToList();
                var ListaExperienciaLaboral = (from experiencialaboral in experiencialaboralQuery
                                               join tipoexperiencialaboral in tipoexperiencialaboralQuery
                                               on experiencialaboral.IdExperienciaLaboral equals tipoexperiencialaboral.Id
                                               where experiencialaboral.IdEstudiante == idEstudiante
                                               select new CvExperienciaLaboralViewModel
                                               {
                                                   Empresa = experiencialaboral.NombreEmpresa,
                                                   Nombre = tipoexperiencialaboral.Nombre,
                                                   Descripcion = experiencialaboral.Descripcion,
                                                   FechaInicio = experiencialaboral.FechaIncio,
                                                   FechaFin = experiencialaboral.FechaFin
                                               }).ToList();

                cv.ExperienciaLaboral = ListaExperienciaLaboral;
                #endregion

                #region Lista_formacionAcademica
                var formacionAcademicaQuery = entitiesDomain.FormacionAcademicaRepositorio.ObtenerTodos().ToList();
                var tipoformacionAcademicaQuery = entitiesDomain.TipoFormacionAcademicaRepositorio.ObtenerTodos().ToList();
                var ListaFormacionAcademica = (from formacionAcademica in formacionAcademicaQuery
                                               join tipoformacionAcademica in tipoformacionAcademicaQuery
                                               on formacionAcademica.IdFormacionAcademica equals tipoformacionAcademica.Id
                                               where formacionAcademica.IdEstudiante == idEstudiante
                                               select new CvFormacionAcademicaViewModel
                                               {
                                                   Nombre = tipoformacionAcademica.Nombre,
                                                   Descripcion = formacionAcademica.Descripcion,
                                                   FechaInicio = formacionAcademica.FechaIncio,
                                                   FechaFin = formacionAcademica.FechaFin,
                                                   Empresa = formacionAcademica.Empresa

                                               }).ToList();

                cv.FormacionAcademica = ListaFormacionAcademica;
                //TASKS
                // Poner como not null el campo 'Fecha Fin' 

                #endregion

                #region Lista_capacitaciones
                var capacitacionesQuery = entitiesDomain.CapacitacionRepositorio.ObtenerTodos().ToList();
                var tipocapacitacionesQuery = entitiesDomain.TipoCapacitacionRepositorio.ObtenerTodos().ToList();
                var ListaCapacitaciones = (from capacitaciones in capacitacionesQuery
                                           join tipocapacitaciones in tipocapacitacionesQuery
                                           on capacitaciones.IdCapacitacion equals tipocapacitaciones.Id
                                           where capacitaciones.IdEstudiante == idEstudiante
                                           select new CvCapacitacionesViewModel
                                           {
                                               Nombre = tipocapacitaciones.Nombre,
                                               Descripcion = capacitaciones.Descripcion,
                                               FechaInicio = capacitaciones.FechaIncio,
                                               FechaFin = capacitaciones.FechaFin,
                                               Empresa = capacitaciones.Empresa

                                           }).ToList();

                cv.Capacitaciones = ListaCapacitaciones;

                //TASKS
                //Poner el campo 'Empresa' en la tabla CAPACITACION
                // Poner el la descripcion un numero minimo de 130 caracteres y maximo de 150
                #endregion

                #region Lista_idiomas
                var idiomasQuery = entitiesDomain.EstudianteIdiomaRepositorio.ObtenerTodos().ToList();
                var tipoidiomaQuery = entitiesDomain.IdiomaRepositorio.ObtenerTodos().ToList();
                var ListaIdiomas = (from idiomas in idiomasQuery
                                    join tipoIdioma in tipoidiomaQuery
                                    on idiomas.IdIdioma equals tipoIdioma.Id
                                    where idiomas.IdEstudiante == idEstudiante
                                    select new CvNombreIdiomaViewModel
                                    {
                                        Nombre = tipoIdioma.Nombre,
                                        PromedioIdioma = (idiomas.NivelWriting + idiomas.NivelListening + idiomas.NivelSpeaking) / 3
                                    }).ToList();

                cv.Idiomas = ListaIdiomas;
                #endregion

                // Filtramos por ID los datos y pasamos al ViewModel
                cv.Nombre = datosApi.Nombres;
                cv.Apellido = datosApi.ApellidoPaterno;
                cv.Telefono = aspNetUserQuery.Where(x => x.Id == idEstudiante).Select(telefono => telefono.PhoneNumber).FirstOrDefault();
                cv.Email = aspNetUserQuery.Where(x => x.Id == idEstudiante).Select(email => email.Email).FirstOrDefault();
                cv.Carrera = datosApi.Carrera;



                return new ViewAsPdf("Curriculum", cv)
                {
                    PageSize = Rotativa.AspNetCore.Options.Size.A4,
                    PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                    FileName = "Curriculum.pdf"
                };

            }
            return new ViewAsPdf("ViewY")
            {
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
            };

            return View();
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
