using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Mail;
using Unach.DA.Empleo.Dominio.Core;
using Unach.DA.Empleo.Persistencia.Core.Models;
using Unach.DA.Empleo.Presentacion.CentralAdmin.Extensions;
using Unach.DA.Empleo.Presentacion.CentralAdmin.Models;
using Unach.DA.Empleo.Presentacion.CentralAdmin.ViewModel;

namespace Unach.DA.Empleo.Presentacion.CentralAdmin.Controllers
{
    public class IdiomaController : Controller
    {
        EntitiesDomain entitiesDomain;
        private ILogger logger;
        private readonly IMapper _mapper;

        public IdiomaController(DbContextOptions<SicoaContext> options, IMapper mapper, ILoggerFactory log)
        {
            _mapper = mapper;
            entitiesDomain = new EntitiesDomain(options);
            logger = log.CreateLogger(typeof(IdiomaController));
        }

        public IActionResult Index()
        {

            int expediente = 0;

            ViewBag.Expediente = expediente;

            List<EstudianteIdiomaViewModel> Idioma = entitiesDomain.IdiomaRepositorio.ObtenerTodosEnOtraVista(
                m => new EstudianteIdiomaViewModel
                {
                    Id = m.Id,
                  
                },
                x => x.Id > expediente,
                a => a.OrderBy(y => y.Id));

            return View(Idioma.ToList());
                      
        }

        public IActionResult EstudianteIdioma() // Trae los idiomas segun el estudiante 
        {

            int expediente = 0;
            ViewBag.Expediente = expediente;
            var idEstudiante = HttpContext.Session.GetString("IdServidor");
            List<EstudianteIdiomaViewModel> logros = entitiesDomain.EstudianteIdiomaRepositorio.ObtenerTodosEnOtraVista(
                m => new EstudianteIdiomaViewModel
                {
                    Id = m.Id,
                    IdIdioma = m.IdIdioma,
                    IdEstudiante = m.IdEstudiante,
                    NivelListening = m.NivelListening,
                    NivelSpeaking = m.NivelSpeaking,
                    NivelWriting = m.NivelWriting,
                    Certificado = m.Certificado,    
                   
                },
                x => x.Id > expediente && x.IdEstudiante == idEstudiante,
                a => a.OrderBy(y => y.Id));

            return View(logros);
        }

        public IActionResult EstudianteIdiomaEdit(int id)
        {
            try
            {
                if (id == 0)
                {
                    EstudianteIdiomaViewModel Idioma = new EstudianteIdiomaViewModel();
                    Idioma.tipoIdioma = entitiesDomain.IdiomaRepositorio.ObtenerTodos();
                    Idioma.IdEstudiante = HttpContext.Session.GetString("IdServidor");
                    return PartialView("~/Views/Idioma/_EstudianteIdiomaEdit.cshtml", Idioma);
                }
                else
                {
                    var query = entitiesDomain.EstudianteIdiomaRepositorio.ObtenerTodosEnOtraVista<EstudianteIdiomaViewModel>(

                        m => new EstudianteIdiomaViewModel
                        {
                            Id = m.Id,
                            IdIdioma = m.IdIdioma,
                            IdEstudiante = m.IdEstudiante,
                            NivelListening = m.NivelListening,
                            NivelSpeaking = m.NivelSpeaking,
                            NivelWriting = m.NivelWriting,
                            Certificado = m.Certificado,
                        },
                        x => x.Id == id).FirstOrDefault();

                    if (query != null)
                    {
                        query.tipoIdioma = entitiesDomain.IdiomaRepositorio.ObtenerTodos();
                        return PartialView("~/Views/Idioma/_EstudianteIdiomaEdit.cshtml", query);
                    }
                    else
                        return PartialView("~/Views/Idioma/_EstudianteIdiomaEdit.cshtml", new EstudianteIdiomaViewModel());
                }

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error");
                TempData.MostrarAlerta(ViewModel.TipoAlerta.Error, "Error! " + ex.Message);
                return PartialView("~/Views/Idioma/_EstudianteIdiomaEdit.cshtml", new EstudianteIdiomaViewModel());
            }

        }

        [HttpPost]
        public IActionResult EstudianteIdiomaEdit(EstudianteIdiomaViewModel item)
        {
            try
            {
                //if (ModelState.IsValid)
                // {
                // item.Id = item.Id == -1 ? null : item.Id;

                if (item.Id == 0)
                {
                    var idioma = _mapper.Map<EstudianteIdioma>(item);
                    idioma.FechaTransaccion = DateTime.Now;

                    _mapper.AgregarDatosAuditoria(idioma, HttpContext);
                    entitiesDomain.EstudianteIdiomaRepositorio.Insertar(idioma);
                    entitiesDomain.GuardarTransacciones();

                    TempData.MostrarAlerta(ViewModel.TipoAlerta.Exitosa, "Información registrada.");
                }
                else
                {
                    var idioma = _mapper.Map<EstudianteIdioma>(item);
               
                    //logro.IdEstudiante = item.IdLogro[0]; // TOCA QUITAR
                    _mapper.AgregarDatosAuditoria(idioma, HttpContext);
                    entitiesDomain.EstudianteIdiomaRepositorio.Actualizar(idioma);
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
            return RedirectToAction("EstudianteIdioma", "Idioma");
        }

        public IActionResult EstudianteIdiomaDelete(int id)
        {
            try
            {
                EstudianteIdioma item = entitiesDomain.EstudianteIdiomaRepositorio.BuscarPor(x => x.Id == id).FirstOrDefault();
                return PartialView("~/Views/Idioma/_EstudianteIdiomaDelete.cshtml", item);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error");
                TempData.MostrarAlerta(ViewModel.TipoAlerta.Error, "Error!" + ex.Message);
            }
            return View();
        }

        [HttpPost]
        public IActionResult EstudianteIdiomaDelete(EstudianteIdioma item)
        {
            try
            {
                if (item != null)
                {
                    entitiesDomain.EstudianteIdiomaRepositorio.Eliminar(item);
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
            return RedirectToAction("EstudianteIdioma", "Idioma");
        }


    }
}
