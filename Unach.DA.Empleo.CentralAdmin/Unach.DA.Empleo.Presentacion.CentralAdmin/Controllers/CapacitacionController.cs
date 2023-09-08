using AutoMapper;
using IronPdf;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Mime;
using Unach.DA.Empleo.Dominio.Core;
using Unach.DA.Empleo.Persistencia.Core.Models;
using Unach.DA.Empleo.Presentacion.CentralAdmin.Extensions;
using Unach.DA.Empleo.Presentacion.CentralAdmin.ViewModel;

namespace Unach.DA.Empleo.Presentacion.CentralAdmin.Controllers
{
    public class CapacitacionController : Controller
    {
        EntitiesDomain entitiesDomain;
        private ILogger logger;
        private readonly IMapper _mapper;

        public CapacitacionController(DbContextOptions<SicoaContext> options, IMapper mapper, ILoggerFactory log)
        {
            _mapper = mapper;
            entitiesDomain = new EntitiesDomain(options);
            logger = log.CreateLogger(typeof(CapacitacionController));
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult EstudianteCapacitacion() // Trae la capacitacion correspondiente a cada estudiante
        {

            var idEstudiante = HttpContext.Session.GetString("IdServidor");
            int expediente = 0;
            ViewBag.Expediente = expediente;

            List<CapacitacionViewModel> capacitaciones = entitiesDomain.CapacitacionRepositorio.ObtenerTodosEnOtraVista(
                m => new CapacitacionViewModel
                {
                    Id = m.Id,
                    IdEstudiante = m.IdEstudiante,
                    Descripcion = m.Descripcion,
                    FechaIncio = m.FechaIncio,
                    FechaFin = m.FechaFin,
                    Certificado = m.Certificado,
                   TipoCapacitacion = m.IdCapacitacionNavigation.Nombre

                },
                x => x.Id > expediente && x.IdEstudiante == idEstudiante,
                a => a.OrderBy(y => y.Id));

     

            return View(capacitaciones);
        }

        public IActionResult CapacitacionEdit(int id, int expediente)
        {
            try
            {
                if (id == 0)
                {
                    CapacitacionViewModel capacitacion = new CapacitacionViewModel();
                    capacitacion.Id = expediente;
                    capacitacion.IdEstudiante = HttpContext.Session.GetString("IdServidor"); // Aqui le pesamos el Id para que viaje por el ViewModel
                    // Le pasamos todos lo tipos de capacitacion
                    capacitacion.TiposCapacitacion = entitiesDomain.TipoCapacitacionRepositorio.ObtenerTodos(); 
                    capacitacion.Id = entitiesDomain.CapacitacionRepositorio.ObtenerTodos().Count();
                    return PartialView("~/Views/Capacitacion/_CapacitacionEdit.cshtml", capacitacion);
                }
                else
                {

                    var query = entitiesDomain.CapacitacionRepositorio.ObtenerTodosEnOtraVista<CapacitacionViewModel>(

                        m => new CapacitacionViewModel
                        {
                            Id = m.Id,
                            Nombre = m.IdCapacitacionNavigation.Nombre,
                            Empresa = m.Empresa,
                            Descripcion= m.Descripcion,
                            FechaFin = m.FechaFin,  
                            FechaIncio = m.FechaIncio,
                            Certificado = m.Certificado,   
                            IdCapacitacion = m.IdCapacitacion,  
                            IdEstudiante = m.IdEstudiante,
                        },
                        x => x.Id == id).FirstOrDefault(); 

                    if (query != null)
                    {
                        query.TiposCapacitacion = entitiesDomain.TipoCapacitacionRepositorio.ObtenerTodos();
                        return PartialView("~/Views/Capacitacion/_CapacitacionEdit.cshtml", query);
                    }
                    else
                        return PartialView("~/Views/Capacitacion/_CapacitacionEdit.cshtml", new CapacitacionViewModel());
                }

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error");
                TempData.MostrarAlerta(ViewModel.TipoAlerta.Error, "Error! " + ex.Message);
                return PartialView("~/Views/Capacitacion/_CapacitacionEdit.cshtml", new CapacitacionViewModel());
            }
        }

        [HttpPost]
        public IActionResult CapacitacionEdit(CapacitacionViewModel item) 
        {
            try
            {
                //if (ModelState.IsValid)
                //{
                   // item.Id = item.Id == -1 ? null : item.Id;

                    if (item.Id == 0)
                    {
                        var capacitacion = _mapper.Map<Capacitacion>(item);
                        _mapper.AgregarDatosAuditoria(capacitacion, HttpContext);
                        entitiesDomain.CapacitacionRepositorio.Insertar(capacitacion);
                        entitiesDomain.GuardarTransacciones();

                        TempData.MostrarAlerta(ViewModel.TipoAlerta.Exitosa, "Información registrada.");
                    }
                    else
                    {
                        var capacitacion = _mapper.Map<Capacitacion>(item);
                        capacitacion.FechaIncio = DateTime.Now;// TOCA QUITAR
                        capacitacion.FechaFin = DateTime.Now;// TOCA QUITAR
                        capacitacion.FechaTransaccion = DateTime.Now;   //TOCA QUITAR

                    _mapper.AgregarDatosAuditoria(capacitacion, HttpContext);
                        entitiesDomain.CapacitacionRepositorio.Actualizar(capacitacion);
                        entitiesDomain.GuardarTransacciones();
                        TempData.MostrarAlerta(ViewModel.TipoAlerta.Exitosa, "Información actualizada");
                    }
                //}
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error");
                TempData.MostrarAlerta(ViewModel.TipoAlerta.Error, "Error! " + ex.Message);
            }
           // return RedirectToAction(nameof(Index), new { expediente = 1 });
            return RedirectToAction("EstudianteCapacitacion", "Capacitacion");


        }


        public IActionResult CapacitacionDelete(int id)
        {
            try
            {
                Capacitacion item = entitiesDomain.CapacitacionRepositorio.BuscarPor(x => x.Id == id).FirstOrDefault();
                return PartialView("~/Views/Capacitacion/_CapacitacionDelete.cshtml", item);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error");
                TempData.MostrarAlerta(ViewModel.TipoAlerta.Error, "Error!" + ex.Message);
            }
            return View();
        }
        [HttpPost]
        public IActionResult CapacitacionDelete(Capacitacion item)
        {
            try
            {
                if (item != null)
                {
                    entitiesDomain.CapacitacionRepositorio.Eliminar(item);
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
            return RedirectToAction("EstudianteCapacitacion", "Capacitacion");
        }


    }
}
