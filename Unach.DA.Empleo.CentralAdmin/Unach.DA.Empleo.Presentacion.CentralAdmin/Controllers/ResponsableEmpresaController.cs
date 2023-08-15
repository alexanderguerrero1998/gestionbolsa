using AutoMapper;
using DevExpress.XtraRichEdit.Import.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using System.Linq.Expressions;
using Unach.DA.Empleo.Dominio.Core;
using Unach.DA.Empleo.Persistencia.Core.Models;
using Unach.DA.Empleo.Presentacion.CentralAdmin.Extensions;
using Unach.DA.Empleo.Presentacion.CentralAdmin.Models;
using Unach.DA.Empleo.Presentacion.CentralAdmin.ViewModel;
using Unach.DA.Empleo.Presistencia.Api;

namespace Unach.DA.Empleo.Presentacion.CentralAdmin.Controllers
{
    public class ResponsableEmpresaController : Controller
    {

        EntitiesDomain entitiesDomain;
        private ILogger logger;
        private readonly IMapper _mapper;
        private readonly SicoaContext _dbContext;
        public ResponsableEmpresaController(DbContextOptions<SicoaContext> options, IMapper mapper, ILoggerFactory log, SicoaContext dbContext)
        {
            _mapper = mapper;
            entitiesDomain = new EntitiesDomain(options);
            logger = log.CreateLogger(typeof(ResponsableEmpresaController));
            _dbContext = dbContext;
        }
        public IActionResult Index()
        {
            int expediente = 0;
            ViewBag.Expediente = expediente;

            List<ResponsableEmpresaViewModel> responsable = entitiesDomain.ResponsableEmpresaRepositorio.ObtenerTodosEnOtraVista(
                m => new ResponsableEmpresaViewModel
                {
                    Id = m.Id,
                    Nombre = m.Nombre,
                    Apellido = m.Apellido,
                    Email = m.Email,
                    Direccion = m.Direccion,
                    Telefono = m.Telefono,

                },
                x => x.Id > expediente,
                a => a.OrderBy(y => y.Id));
            return View(responsable.ToList());
        }

        public IActionResult ResponsableEmpresaEdit(int id, int expediente)
        {
            try
            {
                if (id == 0)
                {
                    ResponsableEmpresaViewModel responsable = new ResponsableEmpresaViewModel();
                    responsable.Id = expediente;
                    responsable.Fecha=DateTime.Now;
                    // postulacion.IdEstudiante = entitiesDomain.PostulacionRepositorio.ObtenerTodos().OrderBy(x => x.Nombre).ToList();
                    responsable.Id = entitiesDomain.ResponsableEmpresaRepositorio.ObtenerTodos().Count();
                    return PartialView("~/Views/ResponsableEmpresa/_ResponsableEmpresaEdit.cshtml", responsable);
                }
                else
                {
                    var query = entitiesDomain.ResponsableEmpresaRepositorio.ObtenerTodosEnOtraVista<ResponsableEmpresaViewModel>(

                        m => new ResponsableEmpresaViewModel
                        {
                            Id = m.Id,
                            IdEmpresa = m.IdEmpresa,
                            IdTipoUsuario = m.IdTipoUsuario,
                            Nombre = m.Nombre,
                            Apellido = m.Apellido,
                            Email = m.Email,
                            Telefono = m.Telefono,
                            Direccion = m.Direccion,
                            Fecha = m.Fecha,
                        }

                        ,
                        x => x.Id == id).FirstOrDefault();



                    if (query != null)
                    {

                        return PartialView("~/Views/ResponsableEmpresa/_ResponsableEmpresaEdit.cshtml", query);
                    }
                    else
                        return PartialView("~/Views/ResponsableEmpresa/_ResponsableEmpresaEdit.cshtml", new ResponsableEmpresaViewModel());
                }

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error");
                TempData.MostrarAlerta(ViewModel.TipoAlerta.Error, "Error! " + ex.Message);
                return PartialView("~/Views/ResponsableEmpresa/_ResponsableEmpresaEdit.cshtml", new ResponsableEmpresaViewModel());
            }
        }

        [HttpPost]
        public IActionResult ResponsableEmpresaEdit(ResponsableEmpresaViewModel item)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // item.Id = item.Id == -1 ? null : item.Id;

                    if (item.Id == 0)
                    {
                        var responsable = _mapper.Map<ResponsableEmpresa>(item);
                        _mapper.AgregarDatosAuditoria(responsable, HttpContext);
                        entitiesDomain.ResponsableEmpresaRepositorio.Insertar(responsable);
                        entitiesDomain.GuardarTransacciones();

                        TempData.MostrarAlerta(ViewModel.TipoAlerta.Exitosa, "Información registrada.");
                    }
                    else
                    {
                        var responsable = _mapper.Map<ResponsableEmpresa>(item);
                        _mapper.AgregarDatosAuditoria(responsable, HttpContext);
                        entitiesDomain.ResponsableEmpresaRepositorio.Actualizar(responsable);
                        entitiesDomain.GuardarTransacciones();
                        TempData.MostrarAlerta(ViewModel.TipoAlerta.Exitosa, "Información actualizada");
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error");
                TempData.MostrarAlerta(ViewModel.TipoAlerta.Error, "Error! " + ex.Message);
            }
            return RedirectToAction(nameof(Index), new { expediente = 1 });
        }



        public IActionResult VacantesByEstudiante()
       {

            var Id = HttpContext.Session.GetString("IdServidor");
            ViewBag.Id = Id;
            List<EstudiantesPorVacanteViewModel> misPostulaciones = entitiesDomain.ExecuteStoredProcedure<EstudiantesPorVacanteViewModel>("dbo.ObtenerEstudiantesPorVacante", ("IdRepresentante", Id)).ToList();

            return View(misPostulaciones);

        }

        public IActionResult TraerUsuarioVacante(int idVacante, int idEmpresa)
        {
            int expediente = 0;
            ViewBag.Expediente = expediente;
            //llamamos a la api 'ApiInfoAcademico'
            ApiInfoAcademico clienteapi = new ApiInfoAcademico("");

            var Id = HttpContext.Session.GetString("IdServidor");
            ViewBag.IdServidor = Id;


            ViewBag.IdVacante = idVacante;
            ViewBag.IdEmpresa = idEmpresa;

           // Lista para almacenar los datos de los usuarios obtenidos de la API
           var estudiantesViewModel = new List<EstudianteViewModel>();

            List<IdEstudiantePorVacanteViewModel> misPostulaciones = entitiesDomain.ExecuteStoredProcedure<IdEstudiantePorVacanteViewModel>("dbo.ObtenerIdEstudiantePorVacante", ("IdVacante ", idVacante),("IdEmpresa", idEmpresa)).ToList();
            
            foreach (var postulacion in misPostulaciones)
            {
                var idEstudiante = postulacion.IdEstudiante;
                
                // Crear una expresión de filtro para buscar por ID de estudiante
                Expression<Func<Estudiante, bool>> filtro = estudiante => estudiante.IdEstudiante == idEstudiante;

                //Obtenemos los usuarios
                var dataestudiante = entitiesDomain.EstudianteRepositorio.BuscarPor(filtro).ToList();

                Estudiante estudiante = dataestudiante.FirstOrDefault();

                // Se deserializa en el modelo de 'ApiInformacionAcademica'
                var apiUrl = "https://pruebas.unach.edu.ec:4431/api/Estudiante/InformacionAcademica/" + idEstudiante;
                    var estudianteApi = clienteapi.Get<ApiInformacionAcademica>(apiUrl);


                // Convertir ApiInformacionAcademica a EstudianteViewModel
                var estudianteViewModel = new EstudianteViewModel
                {
                    EstudianteID = estudianteApi.EstudianteID,
                    DocumentoIdentidad = estudianteApi.DocumentoIdentidad,
                    Nombres = estudianteApi.Nombres,
                    ApellidoPaterno = estudianteApi.ApellidoPaterno,
                    ApellidoMaterno = estudianteApi.ApellidoMaterno,
                    Genero = estudianteApi.Genero,
                    CorreoInstitucional = estudianteApi.CorreoInstitucional,
                    TelefonoCelular = estudianteApi.TelefonoCelular,
                    TelefonoDomicilio = estudianteApi.TelefonoDomicilio,
                    Facultad = estudianteApi.Facultad,
                    Carrera = estudianteApi.Carrera,
                    Nivel = estudianteApi.Nivel,
                    Periodo = estudianteApi.Periodo,
                    LinkLinkeding = estudiante.LinkLinkeding,
                    Id = postulacion.Id,
                    IdPostulacion =  postulacion.IdPostulacion
                 

            };

                    // Agregar el estudianteViewModel a la lista de estudiantes
                    estudiantesViewModel.Add(estudianteViewModel);
            }

            return View(estudiantesViewModel);


        }

       
        public IActionResult ActualizarPostulacion(int idPostulacion, int idEmpresa, string idEstudiante, int EstadoValor)
        {

            //entitiesDomain.ExecuteStoredProcedure<object>("dbo.ActualizarPostulacion", 
            //    ("IdPostulacion ", idPostulacion), ("IdEmpresa", idEmpresa), ("IdEstudiante", idEstudiante), ("NuevoEstado", EstadoValor)).ToList();

            _dbContext.Database.ExecuteSqlInterpolated(
                $"EXEC dbo.ActualizarPostulacion {idPostulacion}, {idEmpresa}, {idEstudiante}, {EstadoValor}"
            );


            return View("~/Views/ResponsableEmpresa/VacantesByEstudiante.cshtml");

        }


    }
}
