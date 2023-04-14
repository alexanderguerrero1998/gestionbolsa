using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging.Signing;
using System.Collections.Generic;
using Unach.DA.Empleo.Dominio.Core;
using Unach.DA.Empleo.Persistencia.Core.Models;
using Unach.DA.Empleo.Presentacion.CentralAdmin.Extensions;
using Unach.DA.Empleo.Presentacion.CentralAdmin.ViewModel;

namespace Unach.DA.Empleo.Presentacion.CentralAdmin.Controllers
{
    public class PostulacionesController : Controller
    {
        EntitiesDomain entitiesDomain;
        private ILogger logger;
        private readonly IMapper _mapper;

        public PostulacionesController(DbContextOptions<SicoaContext> options, IMapper mapper, ILoggerFactory log)
        {
            _mapper = mapper;
            entitiesDomain = new EntitiesDomain(options);
            logger = log.CreateLogger(typeof(PostulacionesController));
        }
        public IActionResult Index()
        {
            /*#region Validar Usuario Funcionario
            int expediente = HttpContext.GetExpedienteModificar();
            if (expediente == -1)
                return RedirectToAction("Index", "Home");

            ViewBag.EsFuncionario = HttpContext.ServidorAutenticado().EsSoloFuncionario;
            #endregion*/

            int expediente = 1;

            ViewBag.Expediente = expediente;

            List<PostulacionesViewModel> postulacion = entitiesDomain.PostulacionRepositorio.ObtenerTodosEnOtraVista(
                m => new PostulacionesViewModel
                {
                    Id = m.Id,
                    IdEstudiante = m.IdEstudiante,
                    IdVacante = m.IdVacante,


                },
                x => x.Id > expediente,
                a => a.OrderBy(y => y.IdEstudiante));


            return View(postulacion.ToList());
        }
        /*

        public IActionResult PostulacionesEdit(int id, int expediente)
        {
            try
            {
                if (id == 0)
                {
                    PostulacionesViewModel postulacion = new PostulacionesViewModel();
                    postulacion.Id = expediente;
                    // postulacion.IdEstudiante = entitiesDomain.PostulacionRepositorio.ObtenerTodos().OrderBy(x => x.Nombre).ToList();
                    postulacion.IdEstudiante = entitiesDomain.PostulacionRepositorio.ObtenerTodos().Count();
                    return PartialView("~/Views/Postulaciones/PostulacionesEdit.cshtml", postulacion);
                }
                else
                {
                    var query = entitiesDomain.PostulacionRepositorio.ObtenerTodosEnOtraVista<PostulacionesViewModel>(

                        m => new PostulacionesViewModel
                        {
                            Id = m.Id,
                            IdEstudiante = m.IdEstudiante,
                            IdVacante = m.IdVacante,
                            Fecha = m.Fecha
                        }

                        ,
                        x => x.Id == id).FirstOrDefault();



                    if (query != null)
                    {

                        return PartialView("~/Views/Postulaciones/_PostulacionesEdit.cshtml", query);
                    }
                    else
                        return PartialView("~/Views/Postulaciones/_PostulacionesEdit.cshtml", new PostulacionesViewModel());
                }

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error");
                TempData.MostrarAlerta(ViewModel.TipoAlerta.Error, "Error! " + ex.Message);
                return PartialView("~/Views/Postulaciones/_PostulacionesEdit.cshtml", new PostulacionesViewModel());
            }
        }

        

        [HttpPost]
        public IActionResult PostulacionesEdit(PostulacionesViewModel item)
        {
            try
            {
                //if (ModelState.IsValid)
                // {
                // item.Id = item.Id == -1 ? null : item.Id;

                if (item.Id == 0)
                {
                    var postulacion = _mapper.Map<Postulacion>(item);
                    _mapper.AgregarDatosAuditoria(postulacion, HttpContext);
                    entitiesDomain.PostulacionRepositorio.Insertar(postulacion);
                    entitiesDomain.GuardarTransacciones();

                    TempData.MostrarAlerta(ViewModel.TipoAlerta.Exitosa, "Información registrada.");
                }
                else
                {
                    var dependencia = _mapper.Map<Postulacion>(item);
                    _mapper.AgregarDatosAuditoria(dependencia, HttpContext);
                    entitiesDomain.PostulacionRepositorio.Actualizar(dependencia);
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
            return RedirectToAction(nameof(Index), new { expediente = 1 });


        }
      
        public IActionResult PostulacionesDelete(int id)
        {
            try
            {
                Postulacion item = entitiesDomain.PostulacionRepositorio.BuscarPor(x => x.Id == id).FirstOrDefault();
                return PartialView("~/Views/Postulaciones/_PostulacionesDelete.cshtml", item);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error");
                TempData.MostrarAlerta(ViewModel.TipoAlerta.Error, "Error!" + ex.Message);
            }
            return View();
        }



        [HttpPost]
        public IActionResult PostulacionesDelete(Postulacion item)
        {
            try
            {
                if (item != null)
                {
                    entitiesDomain.PostulacionRepositorio.Eliminar(item);
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

        public void RegistrarPostulacion(int idVacante, int idEstudiante)
        {
            var vacante = entitiesDomain.VacanteRepositorio.Buscar(idVacante);

            if (vacante != null && vacante.Plaza > 0)
            { 
                    var postulacion = new Postulacion();
                    postulacion.IdEstudiante = idEstudiante;
                    postulacion.IdVacante = idVacante;
                    postulacion.Fecha = DateTime.Now;
                    _mapper.AgregarDatosAuditoria(postulacion, HttpContext);
                    entitiesDomain.PostulacionRepositorio.Insertar(postulacion);
                    entitiesDomain.GuardarTransacciones();
                    TempData.MostrarAlerta(ViewModel.TipoAlerta.Exitosa, "Postulacion Realizada");
            }
            TempData.MostrarAlerta(ViewModel.TipoAlerta.Informacion, "No hay postulaciones! " );
        }






    }
}
