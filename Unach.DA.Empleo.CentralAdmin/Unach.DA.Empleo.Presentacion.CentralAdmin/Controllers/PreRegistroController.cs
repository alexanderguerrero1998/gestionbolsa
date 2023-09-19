using AutoMapper;
using DevExpress.Xpo.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Unach.DA.Empleo.Dominio.Core;
using Unach.DA.Empleo.Persistencia.Core.Models;
using Unach.DA.Empleo.Presentacion.CentralAdmin.Extensions;
using Unach.DA.Empleo.Presentacion.CentralAdmin.Models;
using Unach.DA.Empleo.Presentacion.CentralAdmin.ViewModel;
using Unach.DA.Empleo.Presistencia.Api;

namespace Unach.DA.Empleo.Presentacion.CentralAdmin.Controllers
{
    public class PreRegistroController : Controller
    {
        EntitiesDomain entitiesDomain;
        private ILogger logger;
        private readonly IMapper _mapper;

        public PreRegistroController(DbContextOptions<SicoaContext> options, IMapper mapper, ILoggerFactory log)
        {
            _mapper = mapper;
            entitiesDomain = new EntitiesDomain(options);
            logger = log.CreateLogger(typeof(EstudianteController));

        }

        public IActionResult Index()
        {

            return View();
        }

        [HttpPost]
        public IActionResult VerificarDatosUsuario(EstudianteViewModel item)
        {
            try {
                if (item.DocumentoIdentidad != null)
                {
                    bool userExist = ExistenciaUsuario(item.DocumentoIdentidad);
                    if (userExist)
                    {
                        ApiInformacionBasicaPorCriterio api = new ApiInformacionBasicaPorCriterio("");
                        ApiInfoAcademico api2 = new ApiInfoAcademico("");
                           

                        var apiUrl = "https://pruebas.unach.edu.ec:4431/api/Estudiante/InformacionBasicaPorCriterio/" + item.DocumentoIdentidad;
                        var ApiInfoBasicaPorCriterio = api.Get<Api>(apiUrl);

                        var apiUrl2 = "https://pruebas.unach.edu.ec:4431/api/Estudiante/InformacionAcademica/" + ApiInfoBasicaPorCriterio.EstudianteID;
                        var estudianteApiIfoAcademica = api2.Get<ApiInformacionAcademica>(apiUrl2);

                        // Convertir ApiInformacionAcademica a EstudianteViewModel
                        var estudianteViewModel = new EstudianteViewModel
                        {
                            EstudianteID = estudianteApiIfoAcademica.EstudianteID,
                            DocumentoIdentidad = estudianteApiIfoAcademica.DocumentoIdentidad,
                            Nombres = estudianteApiIfoAcademica.Nombres,
                            ApellidoPaterno = estudianteApiIfoAcademica.ApellidoPaterno,
                            ApellidoMaterno = estudianteApiIfoAcademica.ApellidoMaterno,
                            Genero = estudianteApiIfoAcademica.Genero,
                            CorreoInstitucional = estudianteApiIfoAcademica.CorreoInstitucional,
                            TelefonoCelular = estudianteApiIfoAcademica.TelefonoCelular,
                            TelefonoDomicilio = estudianteApiIfoAcademica.TelefonoDomicilio,
                            Facultad = estudianteApiIfoAcademica.Facultad,
                            Carrera = estudianteApiIfoAcademica.Carrera,
                            Nivel = estudianteApiIfoAcademica.Nivel,
                            Periodo = estudianteApiIfoAcademica.Periodo
                        };

                        return View(estudianteViewModel);
                    }
                    else 
                    {
                        TempData.MostrarAlerta(ViewModel.TipoAlerta.Error, "Usuario Invalido");
                    }
          
                }

            }
            catch(Exception ex) {


                logger.LogError(ex, "Error");
                TempData.MostrarAlerta(ViewModel.TipoAlerta.Error, "Error! " + ex.Message);
            }
          return View("Index");    
        }

    

        bool ExistenciaUsuario(string ci)
        {
            ApiInformacionBasicaPorCriterio api = new ApiInformacionBasicaPorCriterio("");
            var apiUrl = "https://pruebas.unach.edu.ec:4431/api/Estudiante/InformacionBasicaPorCriterio/" +ci;
            var ApiInfoBasicaPorCriterio = api.Get<Api>(apiUrl);
            if (ApiInfoBasicaPorCriterio.DocumentoIdentidad != null)
            {
                return true;
            }
            else { return false; }
           
        }






    }
}
