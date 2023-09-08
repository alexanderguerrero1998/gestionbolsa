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

        public IActionResult ResponsableEmpresaEdit(int id, int expediente, string idEstudiante)
        {
            try
            {
                if (idEstudiante == null)
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
                            IdRepresentante = m.IdRepresentante,
                        }

                        ,
                        x => x.IdRepresentante == idEstudiante).FirstOrDefault();



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
                //if (ModelState.IsValid)
                //{
                    // item.Id = item.Id == -1 ? null : item.Id;

                    if (item.Id == 0)
                    {
                        var responsable = _mapper.Map<ResponsableEmpresa>(item);
                        responsable.Fecha= DateTime.Now;
                        _mapper.AgregarDatosAuditoria(responsable, HttpContext);
                        entitiesDomain.ResponsableEmpresaRepositorio.Insertar(responsable);
                        entitiesDomain.GuardarTransacciones();

                        TempData.MostrarAlerta(ViewModel.TipoAlerta.Exitosa, "Información registrada.");
                    }
                    else
                    {
                        var responsable = _mapper.Map<ResponsableEmpresa>(item);
                         responsable.Fecha= DateTime.Now;
                        _mapper.AgregarDatosAuditoria(responsable, HttpContext);
                        entitiesDomain.ResponsableEmpresaRepositorio.Actualizar(responsable);
                        entitiesDomain.GuardarTransacciones();
                        TempData.MostrarAlerta(ViewModel.TipoAlerta.Exitosa, "Información actualizada");
                    }
               // }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error");
                TempData.MostrarAlerta(ViewModel.TipoAlerta.Error, "Error! " + ex.Message);
            }
            return RedirectToAction("DatosRepresentante", "ResponsableEmpresa");
        }


        public IActionResult VacantesByEstudiante() // Muestra las vacantes de esa empresa y la cantidad de usuarios que se han postulado a esa vacante
       {

            var Id = HttpContext.Session.GetString("IdServidor");
            ViewBag.Id = Id;
            List<EstudiantesPorVacanteViewModel> misPostulaciones = entitiesDomain.ExecuteStoredProcedure<EstudiantesPorVacanteViewModel>("dbo.ObtenerEstudiantesPorVacante", ("IdRepresentante", Id)).ToList();

            return View(misPostulaciones);

        }

        public IActionResult TraerUsuarioVacante(int idVacante, int idEmpresa) //Permite ver especificamente una lista de estudiantes que se encuentran postulados a una vacante 
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
                
                var estudianteQuery = entitiesDomain.AspNetUsersRepositorio.ObtenerTodos();
                var estudiantee = estudianteQuery.FirstOrDefault(e => e.Id == postulacion.Id);

                string phonenumber = null;
                if (estudiantee != null)
                {
                    phonenumber = estudiantee.PhoneNumber;
                }

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
                    TelefonoCelular = phonenumber,
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

    
        public IActionResult DatosEmpresa()
        {
            var idUsuario = HttpContext.Session.GetString("IdServidor");
            var representateQuery = entitiesDomain.ResponsableEmpresaRepositorio.ObtenerTodos();
            var idEmpresa = representateQuery.FirstOrDefault(representate => representate.IdRepresentante == idUsuario)?.IdEmpresa;
            ViewBag.idEmpresa = idEmpresa;    
            List<EmpresaViewModel> empresa = entitiesDomain.EmpresaRepositorio.ObtenerTodosEnOtraVista(
                m => new EmpresaViewModel
                {
                    Id = m.Id,
                    Nombre = m.Nombre,
                    Email = m.Email,
                    Direccion = m.Direccion,
                    Telefono = m.Telefono,
                    PaginaWeb = m.PaginaWeb,
                    Ruc = m.Ruc,
                    Tipo = m.Tipo,  
                },
                x => x.Id == idEmpresa,
                a => a.OrderBy(y => y.Id));
            return View(empresa.ToList());
        }

        public IActionResult EmpresaEdit(int id, int expediente)
        {
            try
            {
                if (id == 0)
                {
                    EmpresaViewModel empresa = new EmpresaViewModel();
                    empresa.Id = expediente;
                    // postulacion.IdEstudiante = entitiesDomain.PostulacionRepositorio.ObtenerTodos().OrderBy(x => x.Nombre).ToList();
                    empresa.Id = entitiesDomain.EmpresaRepositorio.ObtenerTodos().Count();
                    return PartialView("~/Views/Empresa/_EmpresaEdit.cshtml", empresa);
                }
                else
                {
                    var query = entitiesDomain.EmpresaRepositorio.ObtenerTodosEnOtraVista<EmpresaViewModel>(

                        m => new EmpresaViewModel
                        {
                            Id = m.Id,
                            Nombre = m.Nombre,
                            Tipo = m.Tipo,
                            Direccion = m.Direccion,
                            Ruc = m.Ruc,
                            PaginaWeb = m.PaginaWeb,
                            Email = m.Email,
                            Telefono = m.Telefono,
                        }
                        ,
                        x => x.Id == id).FirstOrDefault();

                    if (query != null)
                    {

                        return PartialView("~/Views/ResponsableEmpresa/_EmpresaEdit.cshtml", query);
                    }
                    else
                        return PartialView("~/Views/ResponsableEmpresa/_EmpresaEdit.cshtml", new EmpresaViewModel());
                }

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error");
                TempData.MostrarAlerta(ViewModel.TipoAlerta.Error, "Error! " + ex.Message);
                return PartialView("~/Views/ResponsableEmpresa/_EmpresaEdit.cshtml", new EmpresaViewModel());
            }
        }

        [HttpPost]
        public IActionResult EmpresaEdit(EmpresaViewModel item)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // item.Id = item.Id == -1 ? null : item.Id;

                    if (item.Id == 0)
                    {
                        var empresa = _mapper.Map<Empresa>(item);
                        _mapper.AgregarDatosAuditoria(empresa, HttpContext);
                        entitiesDomain.EmpresaRepositorio.Insertar(empresa);
                        entitiesDomain.GuardarTransacciones();

                        TempData.MostrarAlerta(ViewModel.TipoAlerta.Exitosa, "Información registrada.");
                    }
                    else
                    {
                        var empresa = _mapper.Map<Empresa>(item);
                        _mapper.AgregarDatosAuditoria(empresa, HttpContext);
                        entitiesDomain.EmpresaRepositorio.Actualizar(empresa);
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
            return RedirectToAction("DatosEmpresa","ResponsableEmpresa");

        }

        public IActionResult DatosRepresentante()
        {
            var idUsuario = HttpContext.Session.GetString("IdServidor");
            int expediente = 0;
            ViewBag.Expediente = expediente;
            ViewBag.idUsuario = idUsuario;
            List<ResponsableEmpresaViewModel> responsable = entitiesDomain.ResponsableEmpresaRepositorio.ObtenerTodosEnOtraVista(
                m => new ResponsableEmpresaViewModel
                {
                    Id = m.Id,
                    Nombre = m.Nombre,
                    Apellido = m.Apellido,
                    Email = m.Email,
                    Direccion = m.Direccion,
                    Telefono = m.Telefono,
                    IdRepresentante = m.IdRepresentante,
                    
                },
                x => x.IdRepresentante == idUsuario,
                a => a.OrderBy(y => y.Id));
            return View(responsable.ToList());

        }
    }
}
