using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Unach.DA.Empleo.Dominio.Core;
using Unach.DA.Empleo.Persistencia.Core.Models;
using Unach.DA.Empleo.Presentacion.CentralAdmin.Extensions;
using Unach.DA.Empleo.Presentacion.CentralAdmin.ViewModel;

namespace Unach.DA.Empleo.Presentacion.CentralAdmin.Controllers
{
    public class LogroController : Controller
    {
        EntitiesDomain entitiesDomain;
        private ILogger logger;
        private readonly IMapper _mapper;

        public LogroController(DbContextOptions<SicoaContext> options, IMapper mapper, ILoggerFactory log)
        {
            _mapper = mapper;
            entitiesDomain = new EntitiesDomain(options);
            logger = log.CreateLogger(typeof(LogroController));
        }
        public IActionResult Index()
        {
            int expediente = 0;

            ViewBag.Expediente = expediente;

            List<LogroViewModel> Logro = entitiesDomain.LogroRepositorio.ObtenerTodosEnOtraVista(
                m => new LogroViewModel
                {
                    Id = m.Id,
                    
                    Descripcion = m.Descripcion,
                
                    Institucion = m.Institucion,    
                   
                },
                x => x.Id > expediente,
                a => a.OrderBy(y => y.Id));

            return View(Logro.ToList());
        }


        public IActionResult LogroEdit(int id)
        {
            try
            {
                if (id == 0)
                {
                    LogroViewModel Logro = new LogroViewModel();
                    Logro.Logros = entitiesDomain.TipoLogroRepositorio.ObtenerTodos();
                    return PartialView("~/Views/Logro/_LogroEdit.cshtml", Logro);
                }
                else
                {
                    var query = entitiesDomain.LogroRepositorio.ObtenerTodosEnOtraVista<LogroViewModel>(

                        m => new LogroViewModel
                        {
                            Id = m.Id,
                            Institucion = m.Institucion,
                            Descripcion = m.Descripcion,
                           Certificado = m.Certificado,
                          
                        },
                        x => x.Id == id).FirstOrDefault();

                    if (query != null)
                    {

                        query.Logros = entitiesDomain.TipoLogroRepositorio.ObtenerTodos();
                        return PartialView("~/Views/Logro/_LogroEdit.cshtml", query);
                    }
                    else
                        return PartialView("~/Views/Logro/_LogroEdit.cshtml", new LogroViewModel());
                }

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error");
                TempData.MostrarAlerta(ViewModel.TipoAlerta.Error, "Error! " + ex.Message);
                return PartialView("~/Views/Logro/_LogroEdit.cshtml", new LogroViewModel());
            }

        }

        [HttpPost]
        public IActionResult LogroEdit(LogroViewModel item)
        {
            try
            {
                //if (ModelState.IsValid)
                // {
                // item.Id = item.Id == -1 ? null : item.Id;

                if (item.Id == 0)
                {
                    var logro = _mapper.Map<Logro>(item);
                    logro.Fecha = DateTime.Now; // TOCA QUITAR
                    //logro.IdEstudiante = 12; // TOCA QUITAR
                    _mapper.AgregarDatosAuditoria(logro, HttpContext);
                    entitiesDomain.LogroRepositorio.Insertar(logro);
                    entitiesDomain.GuardarTransacciones();

                    TempData.MostrarAlerta(ViewModel.TipoAlerta.Exitosa, "Información registrada.");
                }
                else
                {
                    var logro = _mapper.Map<Logro>(item);
                    logro.Fecha = DateTime.Now; // TOCA QUITAR
                    //logro.IdEstudiante = item.IdLogro[0]; // TOCA QUITAR
                    _mapper.AgregarDatosAuditoria(logro, HttpContext);
                    entitiesDomain.LogroRepositorio.Actualizar(logro);
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
            return RedirectToAction(nameof(Index), new { expediente = 1 });
        }

        public IActionResult LogroDelete(int id)
        {
            try
            {
                Logro item = entitiesDomain.LogroRepositorio.BuscarPor(x => x.Id == id).FirstOrDefault();
                return PartialView("~/Views/Logro/_LogroDelete.cshtml", item);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error");
                TempData.MostrarAlerta(ViewModel.TipoAlerta.Error, "Error!" + ex.Message);
            }
            return View();
        }

        [HttpPost]
        public IActionResult LogroDelete(Logro item)
        {
            try
            {
                if (item != null)
                {
                    entitiesDomain.LogroRepositorio.Eliminar(item);
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
