using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Unach.DA.Empleo.Dominio.Core;
using Unach.DA.Empleo.Persistencia.Core.Models;
using Unach.DA.Empleo.Presentacion.CentralAdmin.Extensions;
using Unach.DA.Empleo.Presentacion.CentralAdmin.ViewModel;

namespace Unach.DA.Empleo.Presentacion.CentralAdmin.Controllers
{
    public class VacanteController : Controller
    {

        EntitiesDomain entitiesDomain;
        private ILogger logger;
        private readonly IMapper _mapper;

        public VacanteController(DbContextOptions<SicoaContext> options, IMapper mapper, ILoggerFactory log)
        {
            _mapper = mapper;
            entitiesDomain = new EntitiesDomain(options);
            logger = log.CreateLogger(typeof(VacanteController));
        }
        public IActionResult Index()
        {
            int expediente = 0;
            ViewBag.Expediente = expediente;

            List<VacanteViewModel> vacante = entitiesDomain.VacanteRepositorio.ObtenerTodosEnOtraVista(
                m => new VacanteViewModel
                {
                    Id = m.Id,
                    Descripcion = m.Descripcion,
                    Cargo = m.Cargo,
                    Plaza = m.Plaza,   
                    Modalidad = m.IdTipoVacanteNavigation.Nombre
                },
                x => x.Id > expediente,
                a => a.OrderBy(y => y.Id));
            return View(vacante.ToList());
        }

        public IActionResult VacanteEdit(int id)
        {
            try
            {
                if (id == 0)
                {
                    VacanteViewModel vacante = new VacanteViewModel();
                    vacante.Empresas = entitiesDomain.EmpresaRepositorio.ObtenerTodos();
                    vacante.Tiposvacante= entitiesDomain.TipoVacanteRepositorio.ObtenerTodos();
                    return PartialView("~/Views/Vacante/_VacanteEdit.cshtml", vacante);
                }
                else
                {
                    var query = entitiesDomain.VacanteRepositorio.ObtenerTodosEnOtraVista<VacanteViewModel>(

                        m => new VacanteViewModel
                        {
                            Id = m.Id,
                            Descripcion = m.Descripcion,
                            Cargo = m.Cargo,
                            Plaza = m.Plaza,
                        }
                        ,
                        x => x.Id == id).FirstOrDefault();

                    if (query != null)
                    {
                        query.Empresas = entitiesDomain.EmpresaRepositorio.ObtenerTodos();
                        query.Tiposvacante = entitiesDomain.TipoVacanteRepositorio.ObtenerTodos();
                        return PartialView("~/Views/Vacante/_VacanteEdit.cshtml", query);
                    }
                    else
                        return PartialView("~/Views/Vacante/_VacanteEdit.cshtml", new VacanteViewModel());
                }

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error");
                TempData.MostrarAlerta(ViewModel.TipoAlerta.Error, "Error! " + ex.Message);
                return PartialView("~/Views/Vacante/_VacanteEdit.cshtml", new VacanteViewModel());
            }
        }



        [HttpPost]
        public IActionResult VacanteEdit(VacanteViewModel item)
        {
            try
            {
                //if (ModelState.IsValid)
               // {
                    // item.Id = item.Id == -1 ? null : item.Id;

                    if (item.Id == 0)
                    {
                        var vacante = _mapper.Map<Vacante>(item);
                        _mapper.AgregarDatosAuditoria(vacante, HttpContext);
                        entitiesDomain.VacanteRepositorio.Insertar(vacante);
                        entitiesDomain.GuardarTransacciones();

                        TempData.MostrarAlerta(ViewModel.TipoAlerta.Exitosa, "Información registrada.");
                    }
                    else
                    {
                        var vacante = _mapper.Map<Vacante>(item);
                        _mapper.AgregarDatosAuditoria(vacante, HttpContext);
                        entitiesDomain.VacanteRepositorio.Actualizar(vacante);
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

        /*
        public IActionResult VacanteDelete(int id)
        {
            try
            {
                Vacante item = entitiesDomain.VacanteRepositorio.BuscarPor(x => x.Id == id).FirstOrDefault();
                return PartialView("~/Views/Vacante/_VacanteDelete.cshtml", item);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error");
                TempData.MostrarAlerta(ViewModel.TipoAlerta.Error, "Error!" + ex.Message);
            }
            return View();
        }
        [HttpPost]
        public IActionResult VacanteDelete(Vacante item)
        {
            try
            {
                if (item != null)
                {
                    entitiesDomain.VacanteRepositorio.Eliminar(item);
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

        */











    }
}
