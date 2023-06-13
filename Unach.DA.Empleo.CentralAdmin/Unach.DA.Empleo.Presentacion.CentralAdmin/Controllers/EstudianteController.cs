using AutoMapper;
using DevExpress.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Unach.DA.Empleo.Dominio.Core;
using Unach.DA.Empleo.Persistencia.Core;
using Unach.DA.Empleo.Persistencia.Core.Models;
using Unach.DA.Empleo.Presentacion.CentralAdmin.Extensions;
using Unach.DA.Empleo.Presentacion.CentralAdmin.ViewModel;

namespace Unach.DA.Empleo.Presentacion.CentralAdmin.Controllers
{
    public class EstudianteController : Controller
    {

        EntitiesDomain entitiesDomain;
        private ILogger logger;
        private readonly IMapper _mapper;

        public EstudianteController(DbContextOptions<SicoaContext> options, IMapper mapper, ILoggerFactory log)
        {
            _mapper = mapper;
            entitiesDomain = new EntitiesDomain(options);
            logger = log.CreateLogger(typeof(EstudianteController));
        }
        //[Authorize(Policy = "RequireUserRole")]
        public IActionResult Index()
        {
            int expediente = 0;
            ViewBag.Expediente = expediente;

            List<EstudianteViewModel> estudiante = entitiesDomain.EstudianteRepositorio.ObtenerTodosEnOtraVista(
                m => new EstudianteViewModel
                {
                    IdEstudiante = m.IdEstudiante,
    

                },
                x => x.IdEstudiante > expediente,
                a => a.OrderBy(y => y.Id));

            return View(estudiante.ToList());

            
        }
        public IActionResult EstudianteEdit(int id, int expediente)
        {
            try
            {
                if (id == 0)
                {
                    EstudianteViewModel estudiante = new EstudianteViewModel();
                    estudiante.IdEstudiante = expediente;
                    // postulacion.IdEstudiante = entitiesDomain.PostulacionRepositorio.ObtenerTodos().OrderBy(x => x.Nombre).ToList();
                    estudiante.IdEstudiante = entitiesDomain.EstudianteRepositorio.ObtenerTodos().Count();
                    return PartialView("~/Views/Estudiante/_EstudianteEdit.cshtml", estudiante);
                }
                else
                {
                    var query = entitiesDomain.EstudianteRepositorio.ObtenerTodosEnOtraVista<EstudianteViewModel>(

                        m => new EstudianteViewModel
                        {
                            IdEstudiante = m.IdEstudiante,
                     
                        }
                        ,
                        x => x.IdEstudiante == id).FirstOrDefault();

                    if (query != null)
                    {

                        return PartialView("~/Views/Estudiante/_EstudianteEdit.cshtml", query);
                    }
                    else
                        return PartialView("~/Views/Estudiante/_EstudianteEdit.cshtml", new EstudianteViewModel());
                }

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error");
                TempData.MostrarAlerta(ViewModel.TipoAlerta.Error, "Error! " + ex.Message);
                return PartialView("~/Views/Estudiante/EstudianteEdit.cshtml", new EstudianteViewModel());
            }
        }

        [HttpPost]
        public IActionResult EstudianteEdit(EstudianteViewModel item)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // item.Id = item.Id == -1 ? null : item.Id;

                    if (item.IdEstudiante == 0)
                {
                    var estudiante = _mapper.Map<Estudiante>(item);
                    _mapper.AgregarDatosAuditoria(estudiante, HttpContext);
                    entitiesDomain.EstudianteRepositorio.Insertar(estudiante);
                    entitiesDomain.GuardarTransacciones();

                    TempData.MostrarAlerta(ViewModel.TipoAlerta.Exitosa, "Información registrada.");
                }
                else
                {
                    var estudiante = _mapper.Map<Estudiante>(item);
                    _mapper.AgregarDatosAuditoria(estudiante, HttpContext);
                    entitiesDomain.EstudianteRepositorio.Actualizar(estudiante);
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


        public IActionResult EstudianteDelete(int id)
        {
            try
            {
                Estudiante item = entitiesDomain.EstudianteRepositorio.BuscarPor(x => x.IdEstudiante == id).FirstOrDefault();
                return PartialView("~/Views/Estudiante/_EstudianteDelete.cshtml", item);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error");
                TempData.MostrarAlerta(ViewModel.TipoAlerta.Error, "Error!" + ex.Message);
            }
            return View();
        }



        [HttpPost]
        public IActionResult EstudianteDelete(Estudiante item)
        {
            try
            {
                if (item != null)
                {
                    entitiesDomain.EstudianteRepositorio.Eliminar(item);
                    entitiesDomain.GuardarTransacciones();
                    TempData.MostrarAlerta(ViewModel.TipoAlerta.Exitosa, "Información eliminada.");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error");
                TempData.MostrarAlerta(ViewModel.TipoAlerta.Error, "Error! " + ex.Message);
            }
            return RedirectToAction(nameof(Index), new { expediente = 1 });
        }

    }
}
