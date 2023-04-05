using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Unach.DA.Empleo.Dominio.Core;
using Unach.DA.Empleo.Persistencia.Core.Models;
using Unach.DA.Empleo.Presentacion.CentralAdmin.Extensions;
using Unach.DA.Empleo.Presentacion.CentralAdmin.ViewModel;

namespace Unach.DA.Empleo.Presentacion.CentralAdmin.Controllers
{
    public class ResponsableEmpresaController : Controller
    {

        EntitiesDomain entitiesDomain;
        private ILogger logger;
        private readonly IMapper _mapper;

        public ResponsableEmpresaController(DbContextOptions<SicoaContext> options, IMapper mapper, ILoggerFactory log)
        {
            _mapper = mapper;
            entitiesDomain = new EntitiesDomain(options);
            logger = log.CreateLogger(typeof(ResponsableEmpresaController));
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




    }
}
