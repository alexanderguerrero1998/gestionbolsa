using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using Unach.DA.Empleo.Dominio.Core;
using Unach.DA.Empleo.Persistencia.Core.Models;
using Unach.DA.Empleo.Presentacion.CentralAdmin.Extensions;
using Unach.DA.Empleo.Presentacion.CentralAdmin.Models;
using Unach.DA.Empleo.Presentacion.CentralAdmin.ViewModel;
using Unach.DA.Empleo.Presistencia.Api;

namespace Unach.DA.Empleo.Presentacion.CentralAdmin.Controllers
{
    public class VacanteController : Controller
    {

        EntitiesDomain entitiesDomain;
        private ILogger logger;
        private readonly IMapper _mapper;

        //Esto es para sacar todas la carreras
        private static readonly HttpClient _httpClient = new HttpClient();
        private const string ApiUrl = "https://pruebas.unach.edu.ec:4431/api/Facultad/Carreras/";
        private const string AccessToken = "eyJhbGciOiJSUzI1NiIsImtpZCI6IjQ1NDA2M0FBMkVCREVGQzdBNTNGRDM4MDE0REYwMTFFMjkxQTAyNEYiLCJ0eXAiOiJhdCtqd3QiLCJ4NXQiOiJSVUJqcWk2OTc4ZWxQOU9BRk44Qkhpa2FBazgifQ.eyJuYmYiOjE2OTMyMzEwNDUsImV4cCI6MTY5NTgyMzA0NSwiaXNzIjoiaHR0cHM6Ly9wcnVlYmFzLnVuYWNoLmVkdS5lYzo0NDMwIiwiYXVkIjoiQWNhZGVtaWNvQVBJIiwiY2xpZW50X2lkIjoiMDBjODJmNjctNmFkYy00ZTJiLTllODItMWE5M2MzMWE2ZjM2IiwiY2xpZW50ZSI6IkJvbHNhIGRlIEVtcGxlbyIsImFtYmllbnRlIjoiRGVzYXJyb2xsbyIsImp0aSI6IkNVSEFSei1QR2dGRWlPaGRmYk5hb1EiLCJzY29wZSI6WyJhY2FkZW1pY28uYXBpLmxlY3R1cmEiXX0.R6UL9713ZoMo8Ytbi_MZy8dm1_imrwmrYomyAPK3mYShUdqomjFACfM8LApyHFvwnhf9-nx5ajEto8NQY-o6eHAe6bokMUnJ_s99l8xRoHkS17f4mTwL8m5N4Qj-9fQNf4VFEpJW7VyLFexf1ZWIVBrote-jHgqu242d0rL0soEVuq554Y9JbpDE0i3J7GPF5Lx5ZO77DCxxL-kZrK6kqZf14-8uwClLqqSZPb0BkVAl0cNDDnzURbL9XQJoPIhMmfv03TnTt36GoToZQZmr7CdNoTZ1zayAhvXYOG_3KdvLLdwJckx-G583SoxReVQWQJgGWmFy71a6LjuXulR7YMeoSUGIRjJmn_gyKf0GiUA72QYUmmmhoyLGaAyMv1xEQPUXhR1tS5NLFTg3aRlobp-UDQufRzuuWMH-UQsfa6y9fDwdWXu3-LxA3rpeZr_mMPzl9C45fBZvgIZO9sbnHu2l49fpQme1PcwvV0CeIe6Ovpfox76Wa5m3dc_4iGtl_-qeRu1MT-z9iDatz7bnx0fJxs6jm-COFGes8JHgI0w1WsWyNN0XCU8KHaZ9FEJLV9QDTNCYdFIZIWOSluxlwSVLY_9_JnqGeiREkWfI-p_VpTbxL4FeNn8dRp_rFC4odgYrwiWVayIbfdxxPTJwaJMdiygz1Fh7snIwBLylhLU"; // Reemplaza con tu token de autenticación


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

        public async Task<IActionResult> VacanteEdit(int id, int idEmpresa)
        {
            try
            {
                if (id == 0)
                {
                    VacanteViewModel vacante = new VacanteViewModel();
                    ViewBag.idEmpresa = idEmpresa;
                    var listaCarreras = await ObtenerTodasLasCarreras();
                    vacante.Carreras = listaCarreras;
                    vacante.Tiposvacante = entitiesDomain.TipoVacanteRepositorio.ObtenerTodos();
                    return PartialView("~/Views/Vacante/_VacanteEdit.cshtml", vacante);
                }
                else
                {
                    var query = entitiesDomain.VacanteRepositorio.ObtenerTodosEnOtraVista<VacanteViewModel>(

                        m => new VacanteViewModel
                        {
                            #region CamposVacanteViewModel
                            Id = m.Id,
                            Descripcion = m.Descripcion,
                            Cargo = m.Cargo,
                            Plaza = m.Plaza,
                            Remuneracion = m.Remuneracion, 
                            TipoContrato = m.TipoContrato,
                            Cuidad = m.Cuidad,
                            Parroquia = m.Parroquia,    
                            Sector = m.Sector,
                            Contacto = m.Contacto,
                            CorreoElectronico = m.CorreoElectronico,
                            Telefono = m.Telefono,
                            Instruccion = m.Instruccion,
                            IdTipoVacante = m.IdTipoVacante,
                            IdTipoCarrera =m.IdTipoCarrera,
                            ConocimientosPrevios = m.ConocimientosPrevios,  
                            Experiencia = m.Experiencia,
                            Actividades = m.Actividades,    
                            Jornada = m.Jornada,
                            AreaCapacitacion = m.AreaCapacitacion,
                            AreaExperiencia = m.AreaExperiencia,
                            IdEmpresa = m.IdEmpresa,
                            Modalidad = m.IdTipoVacanteNavigation.Nombre
                            #endregion

                        }
                        ,
                        x => x.Id == id).FirstOrDefault();

                    if (query != null)

                    {
                        // Llama a ObtenerTodasLasCarreras de forma asincrónica
                        var listaCarreras = await ObtenerTodasLasCarreras();

                        query.Carreras = listaCarreras; 
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
                vacante.FechaTransaccion = DateTime.Now;
                    _mapper.AgregarDatosAuditoria(vacante, HttpContext);
                    entitiesDomain.VacanteRepositorio.Insertar(vacante);
                    entitiesDomain.GuardarTransacciones();

                    TempData.MostrarAlerta(ViewModel.TipoAlerta.Exitosa, "Información registrada.");
                }
                else
                {
                    var vacante = _mapper.Map<Vacante>(item);
                    vacante.FechaTransaccion = DateTime.Now;
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
            return RedirectToAction("CrearVacantePorEmpresa", "Vacante");
        }

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
            // return RedirectToAction(nameof(Index), new { expediente = 1 });
            return RedirectToAction("CrearVacantePorEmpresa", "Vacante");
        }

        public IActionResult CrearVacantePorEmpresa()
        {
            #region IdEmpresa

            var idEstudiante = HttpContext.Session.GetString("IdServidor");
            var EmpresaQuery = entitiesDomain.EmpresaRepositorio.ObtenerTodos().ToList();
            var RepresentanteEmpresaQuery = entitiesDomain.ResponsableEmpresaRepositorio.ObtenerTodos().ToList();

            var IdEmpresa = (from empresa in EmpresaQuery
                             join representante in RepresentanteEmpresaQuery
                             on empresa.Id equals representante.IdEmpresa
                             where representante.IdRepresentante == idEstudiante
                             select representante.IdEmpresa).FirstOrDefault();

            ViewBag.IdEmpresa = IdEmpresa;
            #endregion

    
            // Sacamos todas las vacantes que pertenecen a una empresa

            var vacantesQuery = entitiesDomain.VacanteRepositorio.ObtenerTodos();
            var Listvacantes = (from vacantes in vacantesQuery
                                join empresa in EmpresaQuery
                                on vacantes.IdEmpresa equals empresa.Id
                                where empresa.Id == IdEmpresa
                                select new VacanteViewModel
                                {
                                    Id = vacantes.Id,
                                    Cargo = vacantes.Cargo,
                                    Remuneracion = vacantes.Remuneracion,    
                                    Cuidad = vacantes.Cuidad,    
                                    Sector = vacantes.Sector,

                                }).ToList();

            return View(Listvacantes);
  

        }

        public async Task<IActionResult> PreviewVacanteDetalle(int idVacante)
        {
            int expediente = 0;
            ViewBag.Expediente = expediente;
            var listaCarreras = await ObtenerTodasLasCarreras();
            
            var vacanteQuery = entitiesDomain.VacanteRepositorio.ObtenerTodos();
            var idcarrera = vacanteQuery.FirstOrDefault(e => e.Id == idVacante)?.IdTipoCarrera;
            var NombreAreaEstudio = listaCarreras.FirstOrDefault(e=>e.IdCarrera== idcarrera)?.Nombre;

            List<VacanteViewModel> vacante = entitiesDomain.VacanteRepositorio.ObtenerTodosEnOtraVista(
                 m => new VacanteViewModel
                 {
                     #region CamposViewModel
                     Id = m.Id,
                     Descripcion = m.Descripcion,
                     Cargo = m.Cargo,
                     Plaza = m.Plaza,
                     Remuneracion = m.Remuneracion,
                     TipoContrato = m.TipoContrato,
                     Cuidad = m.Cuidad,
                     Parroquia = m.Parroquia,
                     Sector = m.Sector,
                     Contacto = m.Contacto,
                     CorreoElectronico = m.CorreoElectronico,
                     Telefono = m.Telefono,
                     Instruccion = m.Instruccion,
                     IdTipoCarrera = m.IdTipoCarrera,
                     ConocimientosPrevios = m.ConocimientosPrevios,
                     Experiencia = m.Experiencia,
                     Actividades = m.Actividades,
                     Jornada = m.Jornada,
                     AreaDeEstudios = NombreAreaEstudio,
                     AreaCapacitacion = m.AreaCapacitacion,
                     AreaExperiencia = m.AreaExperiencia,
                     IdEmpresa = m.IdEmpresa,
                     Modalidad = m.IdTipoVacanteNavigation.Nombre,
                     #endregion

                 },
                 x => x.Id == idVacante,
                 a => a.OrderBy(y => y.Id));

          

            return View(vacante.ToList());

        }

        public async Task<List<ApiCarreras>> ObtenerTodasLasCarreras()
        {
            List<ApiCarreras> todasLasCarreras = new List<ApiCarreras>();

            // Configura el encabezado de autenticación con el token
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);

            HttpResponseMessage response = await _httpClient.GetAsync(ApiUrl);

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                todasLasCarreras = JsonConvert.DeserializeObject<List<ApiCarreras>>(json);
            }
            else
            {
         
                
            }
           

            return todasLasCarreras;
        }


    }
}
