using DevExpress.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using Unach.DA.Empleo.Persistencia.Core.Models;
using Unach.DA.Empleo.Presentacion.CentralAdmin.Extensions;
using Unach.DA.Empleo.Presentacion.CentralAdmin.Models;
using System.Collections.Generic;
using System.Linq;

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
            List<EnviarCorreo> usuarios = new List<EnviarCorreo>();

            // Obtener el contexto de la base de datos
            using (var dbContext = new SicoaContext())
            {
                // Obtener todos los usuarios de la tabla AspNetUsers
                var users = _userManager.Users.ToList();

                // Mapear los usuarios a UserModel y agregarlos a la lista
                foreach (var user in users)
                {
                    usuarios.Add(new EnviarCorreo
                    {
                    
                        CorreoElectronico = user.Email
                        // Puedes mapear otras propiedades si las necesitas
                    });
                }
            }

            return View(usuarios);
        }

        public IActionResult Enviar(string[] destinatarios)
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
                foreach (var direccionCorreo in destinatarios)
                {
                    var mail = new MailMessage(username, direccionCorreo)
                    {
                        Subject = "Bolsa de Empleo UNACH",
                        Body = "Usted ha sido acreditado para una oferta laboral en HARVAD!"
                    };

                    smtpClient.Send(mail);

                    TempData.MostrarAlerta(ViewModel.TipoAlerta.Exitosa, "Información Enviada");
                }
               
            } catch (Exception e)
            {
                TempData.MostrarAlerta(ViewModel.TipoAlerta.Exitosa, "No se pudo enviar el mensaje:" + e.GetType());
            }

            return RedirectToAction("Index");

        }

    }
}
