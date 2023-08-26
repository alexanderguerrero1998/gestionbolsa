using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Unach.DA.Empleo.Dominio.Core;
using Unach.DA.Empleo.Persistencia.Core.Models;
using Unach.DA.Empleo.Presentacion.CentralAdmin.Extensions;
using Unach.DA.Empleo.Presentacion.CentralAdmin.ViewModel;


namespace Unach.DA.Empleo.Presentacion.CentralAdmin.Controllers
{
    public class ExperienciaLaboralController : Controller
    {
        EntitiesDomain entitiesDomain;
        private ILogger logger;
        private readonly IMapper _mapper;

        public ExperienciaLaboralController(DbContextOptions<SicoaContext> options, IMapper mapper, ILoggerFactory log)
        {
            _mapper = mapper;
            entitiesDomain = new EntitiesDomain(options);
            logger = log.CreateLogger(typeof(ExperienciaLaboralController));
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult EstudianteExperienciaLaboral() // Trae la formacion academica segun el estudiante estudiante
        {

            int expediente = 0;
            ViewBag.Expediente = expediente;
            var idEstudiante = HttpContext.Session.GetString("IdServidor");
            List<ExperienciaLaboralViewModel> experiencialaboral = entitiesDomain.ExperienciaLaboralRepositorio.ObtenerTodosEnOtraVista(
                m => new ExperienciaLaboralViewModel
                {
                    Id = m.Id,
                    IdExperienciaLaboral = m.IdExperienciaLaboral,
                    IdEstudiante = m.IdEstudiante,
                    Descripcion = m.Descripcion,
                    Tecnologia = m.Tecnologia,
                    FechaIncio = m.FechaIncio,
                    FechaFin = m.FechaFin,
                    Certificado = m.Certificado,
                    NombreEmpresa = m.NombreEmpresa,    

                },
                x => x.Id > expediente && x.IdEstudiante == idEstudiante,
                a => a.OrderBy(y => y.Id));

            return View(experiencialaboral);
        }
        public IActionResult ExperienciaLaboralEdit(int id)
        {
            try
            {
                if (id == 0)
                {
                    ExperienciaLaboralViewModel experiencialaboral = new ExperienciaLaboralViewModel();
                    experiencialaboral.tipoExperiencialaboral= entitiesDomain.TipoExperienciaLaboralRepositorio.ObtenerTodos();
                    experiencialaboral.IdEstudiante = HttpContext.Session.GetString("IdServidor");
                    return PartialView("~/Views/ExperienciaLaboral/_ExperienciaLaboralEdit.cshtml", experiencialaboral);
                }
                else
                {
                    var query = entitiesDomain.ExperienciaLaboralRepositorio.ObtenerTodosEnOtraVista<ExperienciaLaboralViewModel>(

                        m => new ExperienciaLaboralViewModel
                        {
                            Id = m.Id,
                            IdExperienciaLaboral = m.IdExperienciaLaboral,
                            IdEstudiante = m.IdEstudiante,
                            Descripcion = m.Descripcion,
                            Tecnologia = m.Tecnologia,
                            FechaIncio = m.FechaIncio,
                            FechaFin = m.FechaFin,
                            Certificado = m.Certificado,
                            NombreEmpresa = m.NombreEmpresa,
                        },
                        x => x.Id == id).FirstOrDefault();

                    if (query != null)
                    {
                        query.tipoExperiencialaboral = entitiesDomain.TipoExperienciaLaboralRepositorio.ObtenerTodos();
                        return PartialView("~/Views/ExperienciaLaboral/_ExperienciaLaboralEdit.cshtml", query);
                    }
                    else
                        return PartialView("~/Views/ExperienciaLaboral/_ExperienciaLaboralEdit.cshtml", new ExperienciaLaboralViewModel());
                }

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error");
                TempData.MostrarAlerta(ViewModel.TipoAlerta.Error, "Error! " + ex.Message);
                return PartialView("~/Views/ExperienciaLaboral/_ExperienciaLaboralEdit.cshtml", new ExperienciaLaboralViewModel());
            }

        }
        [HttpPost]
        public IActionResult ExperienciaLaboralEdit(ExperienciaLaboralViewModel item)
        {
            try
            {
                //if (ModelState.IsValid)
                // {
                // item.Id = item.Id == -1 ? null : item.Id;

                if (item.Id == 0)
                {
                    var experiencialaboral = _mapper.Map<ExperienciaLaboral>(item);
                    

                    _mapper.AgregarDatosAuditoria(experiencialaboral, HttpContext);
                    entitiesDomain.ExperienciaLaboralRepositorio.Insertar(experiencialaboral);
                    entitiesDomain.GuardarTransacciones();

                    TempData.MostrarAlerta(ViewModel.TipoAlerta.Exitosa, "Información registrada.");
                }
                else
                {
                    var experiencialaboral = _mapper.Map<ExperienciaLaboral>(item);
                    experiencialaboral.FechaIncio = DateTime.Now;// TOCA QUITAR
                    experiencialaboral.FechaFin = DateTime.Now;
                    experiencialaboral.FechaTransaccion = DateTime.Now;


                    //logro.IdEstudiante = item.IdLogro[0]; // TOCA QUITAR
                    _mapper.AgregarDatosAuditoria(experiencialaboral, HttpContext);
                    entitiesDomain.ExperienciaLaboralRepositorio.Actualizar(experiencialaboral);
                    entitiesDomain.GuardarTransacciones();
                    TempData.MostrarAlerta(ViewModel.TipoAlerta.Exitosa, "Información actualizada");
                }
                //  }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error");
                TempData.MostrarAlerta(ViewModel.TipoAlerta.Error, "Error! " + ex.Message);
            }
            // return RedirectToAction(nameof(Index), new { expediente = 1 });
            return RedirectToAction("EstudianteExperienciaLaboral", "ExperienciaLaboral");
        }
        public IActionResult ExperienciaLaboralDelete(int id)
        {
            try
            {
                ExperienciaLaboral item = entitiesDomain.ExperienciaLaboralRepositorio.BuscarPor(x => x.Id == id).FirstOrDefault();
                return PartialView("~/Views/ExperienciaLaboral/_ExperienciaLaboralDelete.cshtml", item);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error");
                TempData.MostrarAlerta(ViewModel.TipoAlerta.Error, "Error!" + ex.Message);
            }
            return View();
        }
        [HttpPost]
        public IActionResult ExperienciaLaboralDelete(ExperienciaLaboral item)
        {
            try
            {
                if (item != null)
                {
                    entitiesDomain.ExperienciaLaboralRepositorio.Eliminar(item);
                    entitiesDomain.GuardarTransacciones();
                    TempData.MostrarAlerta(ViewModel.TipoAlerta.Exitosa, "Información eliminada.");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error");
                TempData.MostrarAlerta(ViewModel.TipoAlerta.Error, "Error! " + ex.Message);
            }
            //return RedirectToAction(nameof(Index), new { expediente = 1 });
            return RedirectToAction("EstudianteExperienciaLaboral", "ExperienciaLaboral");
        }

    }
}
