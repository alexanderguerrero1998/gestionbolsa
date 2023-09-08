using AutoMapper;
using Azure.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net.Mail;
using Unach.DA.Empleo.Dominio.Core;
using Unach.DA.Empleo.Persistencia.Core.Models;
using Unach.DA.Empleo.Presentacion.CentralAdmin.Models;
using Unach.DA.Empleo.Presentacion.CentralAdmin.ViewModel;
using Unach.DA.Empleo.Presistencia.Api;

namespace Unach.DA.Empleo.Presentacion.CentralAdmin.Controllers
{
    public class CoordinadorController : Controller
    {

        EntitiesDomain entitiesDomain;
        private ILogger logger;
        private readonly IMapper _mapper;
        private readonly SicoaContext _dbContext;

        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;


        //Esto es para sacar todas la carreras
        private static readonly HttpClient _httpClient = new HttpClient();
        private const string ApiUrl = "https://pruebas.unach.edu.ec:4431/api/Facultad/Carreras/";
        private const string AccessToken = "eyJhbGciOiJSUzI1NiIsImtpZCI6IjQ1NDA2M0FBMkVCREVGQzdBNTNGRDM4MDE0REYwMTFFMjkxQTAyNEYiLCJ0eXAiOiJhdCtqd3QiLCJ4NXQiOiJSVUJqcWk2OTc4ZWxQOU9BRk44Qkhpa2FBazgifQ.eyJuYmYiOjE2OTMyMzEwNDUsImV4cCI6MTY5NTgyMzA0NSwiaXNzIjoiaHR0cHM6Ly9wcnVlYmFzLnVuYWNoLmVkdS5lYzo0NDMwIiwiYXVkIjoiQWNhZGVtaWNvQVBJIiwiY2xpZW50X2lkIjoiMDBjODJmNjctNmFkYy00ZTJiLTllODItMWE5M2MzMWE2ZjM2IiwiY2xpZW50ZSI6IkJvbHNhIGRlIEVtcGxlbyIsImFtYmllbnRlIjoiRGVzYXJyb2xsbyIsImp0aSI6IkNVSEFSei1QR2dGRWlPaGRmYk5hb1EiLCJzY29wZSI6WyJhY2FkZW1pY28uYXBpLmxlY3R1cmEiXX0.R6UL9713ZoMo8Ytbi_MZy8dm1_imrwmrYomyAPK3mYShUdqomjFACfM8LApyHFvwnhf9-nx5ajEto8NQY-o6eHAe6bokMUnJ_s99l8xRoHkS17f4mTwL8m5N4Qj-9fQNf4VFEpJW7VyLFexf1ZWIVBrote-jHgqu242d0rL0soEVuq554Y9JbpDE0i3J7GPF5Lx5ZO77DCxxL-kZrK6kqZf14-8uwClLqqSZPb0BkVAl0cNDDnzURbL9XQJoPIhMmfv03TnTt36GoToZQZmr7CdNoTZ1zayAhvXYOG_3KdvLLdwJckx-G583SoxReVQWQJgGWmFy71a6LjuXulR7YMeoSUGIRjJmn_gyKf0GiUA72QYUmmmhoyLGaAyMv1xEQPUXhR1tS5NLFTg3aRlobp-UDQufRzuuWMH-UQsfa6y9fDwdWXu3-LxA3rpeZr_mMPzl9C45fBZvgIZO9sbnHu2l49fpQme1PcwvV0CeIe6Ovpfox76Wa5m3dc_4iGtl_-qeRu1MT-z9iDatz7bnx0fJxs6jm-COFGes8JHgI0w1WsWyNN0XCU8KHaZ9FEJLV9QDTNCYdFIZIWOSluxlwSVLY_9_JnqGeiREkWfI-p_VpTbxL4FeNn8dRp_rFC4odgYrwiWVayIbfdxxPTJwaJMdiygz1Fh7snIwBLylhLU"; // Reemplaza con tu token de autenticación


        public CoordinadorController(DbContextOptions<SicoaContext> options, IMapper mapper, ILoggerFactory log, SicoaContext dbContext, UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            _mapper = mapper;
            entitiesDomain = new EntitiesDomain(options);
            logger = log.CreateLogger(typeof(CoordinadorController));
            _dbContext = dbContext;

            _userManager = userManager;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult OfertasLaborales() 
        {

            List<OfertasLaboralesViewModel> oferta = entitiesDomain.ExecuteStoredProcedure<OfertasLaboralesViewModel>("dbo.ObtenerOfertasLaborales").ToList();
            //Enviamos el Id del usuario autenticado para la postulacion
            ViewBag.Id = HttpContext.Session.GetString("IdServidor");

            return View(oferta);
        }
        public async Task<IActionResult> CorreoConsulta()
        {
            NotificarCorreoViewModel notificarCorreo = new NotificarCorreoViewModel();
            var listaCarreras = await ObtenerCarrerasDeTabla();
            var listaCarrerasSinDuplicados = listaCarreras.Distinct().ToList();
            notificarCorreo.ListaCarreras = listaCarrerasSinDuplicados;
            return PartialView("~/Views/Coordinador/CorreoConsulta.cshtml", notificarCorreo);
        }

        [HttpPost]
        public async Task<IActionResult> CorreoConsulta(NotificarCorreoViewModel item) 
        {
            // Lista para almacenar los datos de los usuarios obtenidos de la API
            var estudiantesViewModel = new List<EstudianteViewModel>();
            //llamamos a la api 'ApiInfoAcademico'
            ApiInfoAcademico clienteapi = new ApiInfoAcademico("");

            var smtpServer = _configuration["EmailSettings:SmtpServer"];
            var smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"]);
            var useSsl = bool.Parse(_configuration["EmailSettings:UseSsl"]);
            var username = _configuration["EmailSettings:Username"];
            var password = _configuration["EmailSettings:Password"];

            // Obtén todas la carreras a las que se podrian enviar un mensaje
            // var listaCarreras = await ObtenerTodasLasCarreras();

            //Obtenemos los usuarios
            var dataestudiante = entitiesDomain.EstudianteRepositorio.ObtenerTodos().ToList();
            var aspnetuserquery = entitiesDomain.AspNetUsersRepositorio.ObtenerTodos().ToList();

            //Obtenemos los Id's de los usuarios
            var userIds = dataestudiante.Select(dataestudiante => dataestudiante.IdEstudiante);
          
            //Pasamos los id's  a la 'ApiInformacionAcademica'
            foreach (var id in userIds)
            {
                // Se deserializa en el modelo de 'ApiInformacionAcademica'
                var apiUrl = "https://pruebas.unach.edu.ec:4431/api/Estudiante/InformacionAcademica/" + id;
                var estudianteApi = clienteapi.Get<ApiInformacionAcademica>(apiUrl);
                var idEstudiante = dataestudiante.FirstOrDefault(e => e.IdEstudiante == estudianteApi.EstudianteID)?.Id;
                var email = aspnetuserquery.FirstOrDefault(x => x.Id == idEstudiante)?.Email;


                // Convertir ApiInformacionAcademica a EstudianteViewModel
                var estudianteViewModel = new EstudianteViewModel
                {
                    EstudianteID = estudianteApi.EstudianteID,
                    CorreoPersonal = email,
                    Facultad = estudianteApi.Facultad,
                    Carrera = estudianteApi.Carrera,
                };

              

                // Agregar el estudianteViewModel a la lista de estudiantes
                estudiantesViewModel.Add(estudianteViewModel);
            }


            var todaslascarreras = await ObtenerTodasLasCarreras();

            var carrera = todaslascarreras.FirstOrDefault(e=>e.IdCarrera == item.IdTipoCarrera).Nombre;

            var listemail = estudiantesViewModel.Where(e => e.Carrera == carrera).Select(x => x.CorreoPersonal).ToList();

            //// Obtén los usuarios seleccionados
            //var usuariosSeleccionados = _userManager.Users.ToList();
            //// Obtén las direcciones de correo electrónico de los usuarios seleccionados
            //var direccionesCorreo = usuariosSeleccionados.Select(u => u.Email).ToList();

            // Configura el cliente SMTP para el envío de correos electrónicos
            var smtpClient = new SmtpClient(smtpServer, smtpPort)
            {
                UseDefaultCredentials = false,
                EnableSsl = useSsl,
                Credentials = new System.Net.NetworkCredential(username, password)
            };

            // Envía los correos electrónicos a cada dirección de correo electrónico
            foreach (var direccionCorreo in listemail)
            {
                var mail = new MailMessage(username, direccionCorreo)
                {
                    Subject = "Bolsa de Empleo UNACH",
                    Body = item.Mensaje
                };

                smtpClient.Send(mail);
            }


            return View();  
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

        public async Task<List<ApiCarreras>> ObtenerCarrerasDeTabla()
        {
            
            ApiInfoAcademico clienteapi = new ApiInfoAcademico("");
            var carreraViewModel = new List<ApiCarreras>();
            var dataestudiante = entitiesDomain.EstudianteRepositorio.ObtenerTodos().ToList();
            var userIds = dataestudiante.Select(dataestudiante => dataestudiante.IdEstudiante);
            var todaslascarreras = await ObtenerTodasLasCarreras();
            // Utiliza un conjunto para evitar carreras duplicadas
            var carrerasUnicas = new HashSet<string>();

            //Pasamos los id's  a la 'ApiInformacionAcademica'
            foreach (var id in userIds)
            {
                // Se deserializa en el modelo de 'ApiInformacionAcademica'
                var apiUrl = "https://pruebas.unach.edu.ec:4431/api/Estudiante/InformacionAcademica/" + id;
        
                var estudianteApi = clienteapi.Get<ApiInformacionAcademica>(apiUrl);
          
                var nombreCarrera = estudianteApi.Carrera;
                var x = todaslascarreras.FirstOrDefault(e => e.Nombre == nombreCarrera);
                
                // Verifica si la carrera ya se ha agregado
                if (!carrerasUnicas.Contains(nombreCarrera))
                {
                    ApiCarreras carrerasViewModel = new ApiCarreras
                    {
                        IdFacultad = x.IdFacultad,
                        IdCarrera = x.IdCarrera,
                        Nombre = nombreCarrera
                    };

                    carreraViewModel.Add(carrerasViewModel);

                    // Agrega la carrera al conjunto de carreras únicas
                    carrerasUnicas.Add(nombreCarrera);
                }
            }
            return carreraViewModel;
        }

        public IActionResult ObtenerEstudiantes()
        {

            int expediente = 0;
            ViewBag.Expediente = expediente;

            // Lista para almacenar los datos de los usuarios obtenidos de la API
            var estudiantesViewModel = new List<EstudianteViewModel>();

            //Obtenemos los usuarios
            var dataestudiante = entitiesDomain.EstudianteRepositorio.ObtenerTodos().ToList();

            //Obtenemos los Id's de los usuarios
            var userIds = dataestudiante.Select(dataestudiante => dataestudiante.IdEstudiante);

            //llamamos a la api 'ApiInfoAcademico'
            ApiInfoAcademico clienteapi = new ApiInfoAcademico("");

            //Pasamos los id's  a la 'ApiInformacionAcademica'
            foreach (var id in userIds)
            {
                
                // Se deserializa en el modelo de 'ApiInformacionAcademica'
                var apiUrl = "https://pruebas.unach.edu.ec:4431/api/Estudiante/InformacionAcademica/" + id;
                var estudianteApi = clienteapi.Get<ApiInformacionAcademica>(apiUrl);

                var aspNetUsersQuery = entitiesDomain.AspNetUsersRepositorio.ObtenerTodos();
                var estudianteQuery = entitiesDomain.EstudianteRepositorio.ObtenerTodos();

                var IdEstudiante = estudianteQuery.FirstOrDefault(e => e.IdEstudiante == estudianteApi.EstudianteID)?.Id;
                var phoneNumber = aspNetUsersQuery.FirstOrDefault(e => e.Id == IdEstudiante)?.PhoneNumber;

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
                    TelefonoCelular = phoneNumber,
                    TelefonoDomicilio = estudianteApi.TelefonoDomicilio,
                    Facultad = estudianteApi.Facultad,
                    Carrera = estudianteApi.Carrera,
                    Nivel = estudianteApi.Nivel,
                    Periodo = estudianteApi.Periodo
                };

                // Agregar el estudianteViewModel a la lista de estudiantes
                estudiantesViewModel.Add(estudianteViewModel);

            }

            return View(estudiantesViewModel);

            return View();  
        }


    }
}
