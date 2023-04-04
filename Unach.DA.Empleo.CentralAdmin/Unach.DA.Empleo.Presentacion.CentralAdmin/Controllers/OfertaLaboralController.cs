using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Unach.DA.Empleo.Dominio.Core;
using Unach.DA.Empleo.Persistencia.Core.Models;
using Unach.DA.Empleo.Presentacion.CentralAdmin.Extensions;
using Unach.DA.Empleo.Presentacion.CentralAdmin.ViewModel;

namespace Unach.DA.Empleo.Presentacion.CentralAdmin.Controllers
{
    public class OfertaLaboralController : Controller
    {
        EntitiesDomain entitiesDomain;
        private ILogger logger;
        private readonly IMapper _mapper;

        public OfertaLaboralController(DbContextOptions<SicoaContext> options, IMapper mapper, ILoggerFactory log)
        {
            _mapper = mapper;
            entitiesDomain = new EntitiesDomain(options);
            logger = log.CreateLogger(typeof(EstudianteController));
        }


        public IActionResult Index()
        {

            List<OfertasLaboralesViewModel> oferta = entitiesDomain.ExecuteStoredProcedure<OfertasLaboralesViewModel>("dbo.ObtenerOfertasLaborales").ToList();
            return View(oferta);
        }

        

    }
}
