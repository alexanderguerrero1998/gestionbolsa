using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Unach.DA.Empleo.Dominio.Core;
using Unach.DA.Empleo.Persistencia.Core.Models;
using Unach.DA.Empleo.Presentacion.CentralAdmin.Extensions;
using Unach.DA.Empleo.Presentacion.CentralAdmin.ViewModel;

namespace Unach.DA.Empleo.Presentacion.CentralAdmin.Controllers
{

    public class EmpresaController : Controller
    {

        EntitiesDomain entitiesDomain;
        private ILogger logger;
        private readonly IMapper _mapper;

        public EmpresaController(DbContextOptions<SicoaContext> options, IMapper mapper, ILoggerFactory log)
        {
            _mapper = mapper;
            entitiesDomain = new EntitiesDomain(options);
            logger = log.CreateLogger(typeof(EmpresaController));
        }

      //  [Authorize(Policy = "RequireAdministratorRole")]
        public IActionResult Index()
        {
            int expediente = 0;
            ViewBag.Expediente = expediente;

            List<EmpresaViewModel> empresa = entitiesDomain.EmpresaRepositorio.ObtenerTodosEnOtraVista(
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

                },
                x => x.Id > expediente,
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

                        return PartialView("~/Views/Empresa/_EmpresaEdit.cshtml", query);
                    }
                    else
                        return PartialView("~/Views/Empresa/_EmpresaEdit.cshtml", new EmpresaViewModel());
                }

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error");
                TempData.MostrarAlerta(ViewModel.TipoAlerta.Error, "Error! " + ex.Message);
                return PartialView("~/Views/Empresa/_EmpresaEdit.cshtml", new EmpresaViewModel());
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
            return RedirectToAction(nameof(Index), new { expediente = 1 });

        }




    }
}
