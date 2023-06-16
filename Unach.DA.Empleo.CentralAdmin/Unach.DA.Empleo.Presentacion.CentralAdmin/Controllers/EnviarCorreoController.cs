using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using Unach.DA.Empleo.Presentacion.CentralAdmin.Extensions;

namespace Unach.DA.Empleo.Presentacion.CentralAdmin.Controllers
{
    public class EnviarCorreoController : Controller
    {

        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;

        public EnviarCorreoController(UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }
        public IActionResult Index()
        {
            return View();
        }

        public void Enviar()
        {

            var smtpServer = _configuration["EmailSettings:SmtpServer"];
            var smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"]);
            var useSsl = bool.Parse(_configuration["EmailSettings:UseSsl"]);
            var username = _configuration["EmailSettings:Username"];
            var password = _configuration["EmailSettings:Password"];

            try {

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
            } catch (Exception e)
            {
                TempData.MostrarAlerta(ViewModel.TipoAlerta.Exitosa, "No se pudo enviar el mensaje:" + e.GetType());
            }
     
        }





    }
}
