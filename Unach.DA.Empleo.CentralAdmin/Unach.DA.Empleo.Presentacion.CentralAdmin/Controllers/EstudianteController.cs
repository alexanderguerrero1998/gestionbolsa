using AutoMapper;
using DevExpress.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Net;
using Unach.DA.Empleo.Dominio.Core;
using Unach.DA.Empleo.Persistencia.Core;
using Unach.DA.Empleo.Persistencia.Core.Models;
using Unach.DA.Empleo.Presentacion.CentralAdmin.Extensions;
using Unach.DA.Empleo.Presentacion.CentralAdmin.Models;
using Unach.DA.Empleo.Presentacion.CentralAdmin.ViewModel;
using DevExpress.Data.Helpers;
using Microsoft.AspNetCore.Identity;
using Unach.DA.Empleo.Presistencia.Api;
using System.Text.Json;
using System.Net.Http.Json;

namespace Unach.DA.Empleo.Presentacion.CentralAdmin.Controllers
{
    public class EstudianteController : Controller
    {

        EntitiesDomain entitiesDomain;
        private ILogger logger;
        private readonly IMapper _mapper;

        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;

        public EstudianteController(DbContextOptions<SicoaContext> options, IMapper mapper, ILoggerFactory log, UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            _mapper = mapper;
            entitiesDomain = new EntitiesDomain(options);
            logger = log.CreateLogger(typeof(EstudianteController));

            _userManager = userManager;
            _configuration = configuration;
        }
        //[Authorize(Policy = "RequireUserRole")]
        public IActionResult Index()
        {
            int expediente = 0;
            ViewBag.Expediente = expediente;

            // Lista para almacenar los datos de los usuarios obtenidos de la API
            var estudiantesViewModel = new List<EstudianteViewModel>();

            //Obtenemos los usuarios
            var dataestudiante  = entitiesDomain.EstudianteRepositorio.ObtenerTodos().ToList();

            //Obtenemos los Id's de los usuarios
            var userIds = dataestudiante.Select(dataestudiante => dataestudiante.IdEstudiante);

            //llamamos a la api 'ApiInfoAcademico'
            ApiInfoAcademico clienteapi = new ApiInfoAcademico("");

         
      
            //Pasamos los id's  a la 'ApiInformacionAcademica'
            foreach (var id in userIds) {
                // Se deserializa en el modelo de 'ApiInformacionAcademica'
                var apiUrl = "https://pruebas.unach.edu.ec:4431/api/Estudiante/InformacionAcademica/" + id;
                var estudianteApi = clienteapi.Get<ApiInformacionAcademica>(apiUrl);

            
                // Convertir ApiInformacionAcademica a EstudianteViewModel
                var estudianteViewModel = new EstudianteViewModel
                {

                    EstudianteID = estudianteApi.EstudianteID,
                    DocumentoIdentidad = estudianteApi.DocumentoIdentidad,
                    Nombres = estudianteApi.Nombres,
                    ApellidoPaterno = estudianteApi.ApellidoPaterno,
                    ApellidoMaterno = estudianteApi.ApellidoMaterno,
                    Genero = estudianteApi.Genero,
                    CorreoInstitucional = estudianteApi.CorreoInstitucional,
                    TelefonoCelular = estudianteApi.TelefonoCelular,
                    TelefonoDomicilio = estudianteApi.TelefonoDomicilio,
                    Facultad = estudianteApi.Facultad,
                    Carrera = estudianteApi.Carrera,
                    Nivel = estudianteApi.Nivel,
                    Periodo = estudianteApi.Periodo
                };

                // Agregar el estudianteViewModel a la lista de estudiantes
                estudiantesViewModel.Add(estudianteViewModel);

            }
    



            ////Obtenemos los datos del usuario

            //List<EstudianteViewModel> estudiante = entitiesDomain.EstudianteRepositorio.ObtenerTodosEnOtraVista(
            //    m => new EstudianteViewModel
            //    {
            //        IdEstudiante = m.IdEstudiante,
            //        LinkLinkeding = m.LinkLinkeding
            //    },
            //    x => x.IdEstudiante > expediente,
            //    a => a.OrderBy(y => y.Id));

            //return View(estudiante.ToList());
            return View(estudiantesViewModel);

        }
        public IActionResult EstudianteEdit(int id, int expediente)
        {
            try
            {
                if (id == 0)
                {
                    EstudianteViewModel estudiante = new EstudianteViewModel();
                    estudiante.IdEstudiante = expediente;
                    // postulacion.IdEstudiante = entitiesDomain.PostulacionRepositorio.ObtenerTodos().OrderBy(x => x.Nombre).ToList();
                    estudiante.IdEstudiante = entitiesDomain.EstudianteRepositorio.ObtenerTodos().Count();
                    return PartialView("~/Views/Estudiante/_EstudianteEdit.cshtml", estudiante);
                }
                else
                {
                    var query = entitiesDomain.EstudianteRepositorio.ObtenerTodosEnOtraVista<EstudianteViewModel>(

                        m => new EstudianteViewModel
                        {
                            IdEstudiante = m.IdEstudiante,

                        }
                        ,
                        x => x.IdEstudiante == id).FirstOrDefault();

                    if (query != null)
                    {

                        return PartialView("~/Views/Estudiante/_EstudianteEdit.cshtml", query);
                    }
                    else
                        return PartialView("~/Views/Estudiante/_EstudianteEdit.cshtml", new EstudianteViewModel());
                }

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error");
                TempData.MostrarAlerta(ViewModel.TipoAlerta.Error, "Error! " + ex.Message);
                return PartialView("~/Views/Estudiante/EstudianteEdit.cshtml", new EstudianteViewModel());
            }
        }
        [HttpPost]
        public IActionResult EstudianteEdit(EstudianteViewModel item)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // item.Id = item.Id == -1 ? null : item.Id;

                    if (item.IdEstudiante == 0)
                    {
                        var estudiante = _mapper.Map<Estudiante>(item);
                        _mapper.AgregarDatosAuditoria(estudiante, HttpContext);
                        entitiesDomain.EstudianteRepositorio.Insertar(estudiante);
                        entitiesDomain.GuardarTransacciones();

                        TempData.MostrarAlerta(ViewModel.TipoAlerta.Exitosa, "Información registrada.");
                    }
                    else
                    {
                        var estudiante = _mapper.Map<Estudiante>(item);
                        _mapper.AgregarDatosAuditoria(estudiante, HttpContext);
                        entitiesDomain.EstudianteRepositorio.Actualizar(estudiante);
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
        public IActionResult EstudianteDelete(int id)
        {
            try
            {
                Estudiante item = entitiesDomain.EstudianteRepositorio.BuscarPor(x => x.IdEstudiante == id).FirstOrDefault();
                return PartialView("~/Views/Estudiante/_EstudianteDelete.cshtml", item);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error");
                TempData.MostrarAlerta(ViewModel.TipoAlerta.Error, "Error!" + ex.Message);
            }
            return View();
        }
        [HttpPost]
        public IActionResult EstudianteDelete(Estudiante item)
        {
            try
            {
                if (item != null)
                {
                    entitiesDomain.EstudianteRepositorio.Eliminar(item);
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

        public IActionResult MisPostulaciones() 
        {


            return View();
        }

        public void Enviar()
        {

            //EmailSettings _emailSettings =  new EmailSettings();
            //// Crear mensaje de correo electrónico
            //MailMessage mensaje = new MailMessage();
            //mensaje.Subject = "Asunto del correo";
            //mensaje.Body = "Contenido del correo";
            //mensaje.From = new MailAddress("edisson.guerrero@unach.edu.ec");

            //try { 
            //// Configurar detalles del servidor SMTP
            //SmtpClient clienteSmtp = new SmtpClient();
            //clienteSmtp.Host = _emailSettings.SmtpServer;
            //clienteSmtp.Port = _emailSettings.SmtpPort;
            //clienteSmtp.EnableSsl = _emailSettings.UseSsl;
            //clienteSmtp.Credentials = new NetworkCredential(_emailSettings.Username, _emailSettings.Password);
            //// Enviar correo electrónico a cada dirección de correo
            ////foreach (string direccionCorreo in direccionesCorreo)
            ////{
            //    mensaje.To.Clear(); // Asegurarse de que los destinatarios anteriores se borren
            //    mensaje.To.Add("edisson.guerrero@unach.edu.ec");

            //    clienteSmtp.Send(mensaje);
            //    //}
            //}
            //catch (Exception e)
            //{
            //    TempData.MostrarAlerta(ViewModel.TipoAlerta.Exitosa, "No se pudo enviar el mensaje:"+e.GetType());
            //}


            var smtpServer = _configuration["EmailSettings:SmtpServer"];
            var smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"]);
            var useSsl = bool.Parse(_configuration["EmailSettings:UseSsl"]);
            var username = _configuration["EmailSettings:Username"];
            var password = _configuration["EmailSettings:Password"];

            // Obtén los usuarios seleccionados y las direcciones de correo electrónico como se mostró anteriormente

            // Obtén los usuarios seleccionados
            var usuariosSeleccionados = _userManager.Users.ToList();
            // Obtén las direcciones de correo electrónico de los usuarios seleccionados
            var direccionesCorreo = usuariosSeleccionados.Select(u => u.Email).ToList();

            // Configura el cliente SMTP para el envío de correos electrónicos
            var smtpClient = new SmtpClient(smtpServer, smtpPort)
            {
                UseDefaultCredentials = false,
                EnableSsl = useSsl,
                Credentials = new System.Net.NetworkCredential(username, password)
            };

            // Envía los correos electrónicos a cada dirección de correo electrónico
            foreach (var direccionCorreo in direccionesCorreo)
            {
                var mail = new MailMessage(username, direccionCorreo)
                {
                    Subject = "Bolsa de Empleo UNACH",
                    Body = "Usted ha sido acreditado para una oferta laboral en HARVAD!"
                };

                smtpClient.Send(mail);
            }



        }
    }
}
