using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Unach.DA.Empleo.Dominio.Core;
using Unach.DA.Empleo.Persistencia.Core.Models;
using Unach.DA.Empleo.Presentacion.CentralAdmin.Extensions;
using Unach.DA.Empleo.Presentacion.CentralAdmin.ViewModel;

namespace Unach.DA.Empleo.Presentacion.CentralAdmin.Controllers
{
    public class FormacionAcademicaController : Controller
    {
        EntitiesDomain entitiesDomain;
        private ILogger logger;
        private readonly IMapper _mapper;

        public FormacionAcademicaController(DbContextOptions<SicoaContext> options, IMapper mapper, ILoggerFactory log)
        {
            _mapper = mapper;
            entitiesDomain = new EntitiesDomain(options);
            logger = log.CreateLogger(typeof(FormacionAcademicaController));
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult EstudianteFormacionAcademica() // Trae la formacion academica segun el estudiante estudiante
        {

            int expediente = 0;
            ViewBag.Expediente = expediente;
            var idEstudiante = HttpContext.Session.GetString("IdServidor");
            List<FormacionAcademicaViewModel> formacionacademica = entitiesDomain.FormacionAcademicaRepositorio.ObtenerTodosEnOtraVista(
                m => new FormacionAcademicaViewModel
                {
                    Id = m.Id,
                    IdFormacionAcademica = m.IdFormacionAcademica,
                    Descripcion = m.Descripcion,
                    IdEstudiante = m.IdEstudiante,
                    FechaIncio = m.FechaIncio,
                    FechaFin = m.FechaFin,
                    Certificado = m.Certificado,
                    TipoFormacionAcademica = m.IdFormacionAcademicaNavigation.Nombre

                },
                x => x.Id > expediente && x.IdEstudiante == idEstudiante,
                a => a.OrderBy(y => y.Id));

            return View(formacionacademica);
        }
        public IActionResult FormacionAcademicaEdit(int id)
        {
            try
            {
                if (id == 0)
                {
                    FormacionAcademicaViewModel formacionacademica = new FormacionAcademicaViewModel();
                    formacionacademica.tipoFormacionAcademica = entitiesDomain.TipoFormacionAcademicaRepositorio.ObtenerTodos();
                    formacionacademica.IdEstudiante = HttpContext.Session.GetString("IdServidor");
                    return PartialView("~/Views/FormacionAcademica/_FormacionAcademicaEdit.cshtml", formacionacademica);
                }
                else
                {
                    var query = entitiesDomain.FormacionAcademicaRepositorio.ObtenerTodosEnOtraVista<FormacionAcademicaViewModel>(

                        m => new FormacionAcademicaViewModel
                        {
                            Id = m.Id,
                            IdFormacionAcademica = m.IdFormacionAcademica,
                            Descripcion = m.Descripcion,
                            IdEstudiante = m.IdEstudiante,
                            FechaIncio = m.FechaIncio,
                            FechaFin = m.FechaFin,
                            Certificado = m.Certificado,
                            Empresa = m.Empresa,
                          

                        },
                        x => x.Id == id).FirstOrDefault();

                    if (query != null)
                    {
                        query.tipoFormacionAcademica = entitiesDomain.TipoFormacionAcademicaRepositorio.ObtenerTodos();
                        return PartialView("~/Views/FormacionAcademica/_FormacionAcademicaEdit.cshtml", query);
                    }
                    else
                        return PartialView("~/Views/FormacionAcademica/_FormacionAcademicaEdit.cshtml", new FormacionAcademicaViewModel());
                }

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error");
                TempData.MostrarAlerta(ViewModel.TipoAlerta.Error, "Error! " + ex.Message);
                return PartialView("~/Views/FormacionAcademica/_FormacionAcademicaEdit.cshtml", new FormacionAcademicaViewModel());
            }

        }
        [HttpPost]
        public IActionResult FormacionAcademicaEdit(FormacionAcademicaViewModel item)
        {
            try
            {
                //if (ModelState.IsValid)
                // {
                // item.Id = item.Id == -1 ? null : item.Id;

                if (item.Id == 0)
                {
                    var formacionacademica = _mapper.Map<FormacionAcademica>(item);
                    //FormacionAcademica

                    _mapper.AgregarDatosAuditoria(formacionacademica, HttpContext);
                    entitiesDomain.FormacionAcademicaRepositorio.Insertar(formacionacademica);
                    entitiesDomain.GuardarTransacciones();

                    TempData.MostrarAlerta(ViewModel.TipoAlerta.Exitosa, "Información registrada.");
                }
                else
                {
                    var formacionacademica = _mapper.Map<FormacionAcademica>(item);
                    formacionacademica.FechaIncio = DateTime.Now;// TOCA QUITAR
                    formacionacademica.FechaFin = DateTime.Now;
                    formacionacademica.FechaTransaccion = DateTime.Now;


                    //logro.IdEstudiante = item.IdLogro[0]; // TOCA QUITAR
                    _mapper.AgregarDatosAuditoria(formacionacademica, HttpContext);
                    entitiesDomain.FormacionAcademicaRepositorio.Actualizar(formacionacademica);
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
            return RedirectToAction("EstudianteFormacionAcademica", "FormacionAcademica");
        }
        public IActionResult FormacionAcademicaDelete(int id)
        {
            try
            {
                FormacionAcademica item = entitiesDomain.FormacionAcademicaRepositorio.BuscarPor(x => x.Id == id).FirstOrDefault();
                return PartialView("~/Views/FormacionAcademica/_FormacionAcademicaDelete.cshtml", item);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error");
                TempData.MostrarAlerta(ViewModel.TipoAlerta.Error, "Error!" + ex.Message);
            }
            return View();
        }
        [HttpPost]
        public IActionResult FormacionAcademicaDelete(FormacionAcademica item)
        {
            try
            {
                if (item != null)
                {
                    entitiesDomain.FormacionAcademicaRepositorio.Eliminar(item);
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
            return RedirectToAction("EstudianteFormacionAcademica", "FormacionAcademica");
        }





    }
}
