using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Unach.DA.Empleo.Dominio.Core;
using Unach.DA.Empleo.Persistencia.Core.Models;
using Unach.DA.Empleo.Presentacion.CentralAdmin.Extensions;
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

            List<IdiomaViewModel> Idioma = entitiesDomain.IdiomaRepositorio.ObtenerTodosEnOtraVista(
                m => new IdiomaViewModel
                {
                    Id = m.Id,
                    Nombre = m.Nombre,
                },
                x => x.Id > expediente,
                a => a.OrderBy(y => y.Id));

            return View(Idioma.ToList());
                      
        }

        public IActionResult IdiomasEdit(int id, int expediente)
        {

            try
            {
                if (id == 0)
                {
                    IdiomaViewModel Idioma = new IdiomaViewModel();
                    Idioma.Id = expediente;
                    // postulacion.IdEstudiante = entitiesDomain.PostulacionRepositorio.ObtenerTodos().OrderBy(x => x.Nombre).ToList();
                    Idioma.Id = entitiesDomain.IdiomaRepositorio.ObtenerTodos().Count();
                    return PartialView("~/Views/Idioma/IdiomasEdit.cshtml", Idioma);
                }
                else
                {
                    var query = entitiesDomain.IdiomaRepositorio.ObtenerTodosEnOtraVista<IdiomaViewModel>(

                        m => new IdiomaViewModel
                        {
                            Id = m.Id,
                            Nombre = m.Nombre
                        },
                        x => x.Id == id).FirstOrDefault();



                    if (query != null)
                    {

                        return PartialView("~/Views/Idioma/IdiomaEdit.cshtml", query);
                    }
                    else
                        return PartialView("~/Views/Idioma/IdiomaEdit.cshtml", new IdiomaViewModel());
                }

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error");
                TempData.MostrarAlerta(ViewModel.TipoAlerta.Error, "Error! " + ex.Message);
                return PartialView("~/Views/Idioma/IdiomaEdit.cshtml", new IdiomaViewModel());
            }

        }


    }
}
