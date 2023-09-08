using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using DevExpress.XtraPrinting.Export;
// Importa el espacio de nombres necesario
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Unach.DA.Empleo.Presentacion.CentralAdmin.Models;
using Unach.DA.Empleo.Presentacion.CentralAdmin.Extensions;
using Newtonsoft.Json;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Unach.DA.Empleo.Persistencia.Core.Models;
using Unach.DA.Empleo.Dominio.Core;
using Unach.DA.Empleo.Presentacion.CentralAdmin.ViewModel;
using Unach.DA.Empleo.Presentacion.CentralAdmin.Controllers;
using Unach.DA.Empleo.Presistencia.Api;

namespace Unach.DA.Empleo.Presentacion.CentralAdmin.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly EmailSettings _emailSettings;
        private readonly IConfiguration _configuration;
        EntitiesDomain entitiesDomain;
        HttpContext hcontext;
        public LoginModel(
            SignInManager<IdentityUser> signInManager, 
            ILogger<LoginModel> logger,
            UserManager<IdentityUser> userManager,
            IEmailSender emailSender, 
            IOptions<EmailSettings> emailSettingsOptions,
             IConfiguration configuration,
             DbContextOptions<SicoaContext> options,
            IHttpContextAccessor haccess
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _emailSettings = emailSettingsOptions.Value;
            _configuration = configuration;
            entitiesDomain = new EntitiesDomain(options);
            hcontext = haccess.HttpContext;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            public string CI { get; set; }
        
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        
            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(Input.CI, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");

                    // Obtener el usuario autenticado
                    var userAuth = await _userManager.FindByNameAsync(Input.CI);

                    //Obtener solo CI/UserName
                    var ci = userAuth.UserName;

                    // Contar la cantidad de dígitos en la cédula de identidad 
                    int cantidadDigitos = ci.Length;
                    if (cantidadDigitos <= 13) { }

                    // llamamos a la api para obtener los datos en las sessiones
                    ClienteApi clienteapi = new ClienteApi("");

                    //Obtenemos los datos del usuario
                    var response = clienteapi.Get<Api>("https://pruebas.unach.edu.ec:4431/api/Estudiante/InformacionBasicaPorCriterio/" + ci);

                    if (userAuth != null)
                    {

                        var usuario = entitiesDomain.AspNetUsersRepositorio.ObtenerTodosEnOtraVista<UsuarioAutenticadoViewModel>(
                        m => new UsuarioAutenticadoViewModel
                        {
                           IdServidor = userAuth.Id,
                           Nombres = response.Nombres,
                           //NombresCompletos = response.NombresCompletos,
                           Identificacion = response.DocumentoIdentidad,
                           //Dependencia=m.
                           //Cargo=m.c
                           Foto = response.FotoRuta,
                           Email = response.CorreoInstitucional
                        }, x => x.UserName == ci).FirstOrDefault();

                        
                        if (usuario != null)
                        {
                            usuario.Roles = entitiesDomain.RolUsuarioRepositorio.ObtenerTodosEnOtraVista<RolViewModel>(
                               m => new RolViewModel
                               {
                                   Id = m.IdRol,
                                   Nombre = m.IdRolNavigation.Nombre,
                                   Descripcion = m.IdRolNavigation.Descripcion,
                               },
                               x => x.IdUsuario == usuario.IdServidor &&
                                    DateTime.Now >= x.Desde && DateTime.Now <= x.Hasta
                           );


                            if (usuario.Roles.Count == 1 && usuario.Roles.Where(x => x.Nombre.ToUpper() == "FUNCIONARIO") != null)
                            {
                                usuario.EsSoloFuncionario = 1;
                            }


                            if (usuario.Roles.Count > 1)
                            {
                                usuario.EsSoloFuncionario = 0;
                            }



                            HttpContext.Session.SetString("AuthenticatedUser", JsonConvert.SerializeObject(usuario));
                            HttpContext.Session.SetString("IdServidor", usuario.IdServidor.ToString());
                            //HttpContext.Session.SetString("Nombres", usuario.Nombres);
                           // HttpContext.Session.SetString("NombresApellidos", string.Format("{0}", usuario.Nombres));
                            //HttpContext.Session.SetString("Foto", usuario.Foto ?? string.Empty);

                   
                            HttpContext.Session.SetString("Nombres", usuario.Nombres ?? string.Empty);
                            HttpContext.Session.SetString("NombresApellidos", string.Format("{0}", usuario.Nombres ?? string.Empty));
                            HttpContext.Session.SetString("Foto", usuario.Foto ?? string.Empty);


                            #region
                            //var claims = new List<Claim>
                            //{
                            //           // new Claim("AuthenticatedUser",JsonConvert.SerializeObject(usuario)),
                            //            new Claim("AuthenticatedUser",JsonConvert.SerializeObject(usuario)),
                            //            new Claim("IdServidor", usuario.IdServidor.ToString()),
                            //            new Claim("Nombres", usuario.Nombres),
                            //            new Claim("NombresApellidos", string.Format("{0}", usuario.Nombres)),
                            //            new Claim("Foto", usuario.Foto??string.Empty),
                            // };
                            #endregion

                            if (usuario.Roles != null && usuario.Roles.Count() > 0)
                            {
                                //foreach (var item in usuario.Roles)
                                //    claims.Add(new Claim(ClaimTypes.Role, item.Nombre));

                                // Create a list to store the roles temporarily
                                var userRoles = usuario.Roles.Select(role => role.Nombre).ToList();

                                // Store the user roles in the session
                                HttpContext.Session.SetString("Roles", JsonConvert.SerializeObject(userRoles));

                            }

                            //var appIdentity=new ClaimsIdentity(claims);

                            //ClaimsPrincipal principal = HttpContext.User;

                            //if (principal.Identity is ClaimsIdentity identity)

                            //{
                            //   hcontext.User.AddIdentity(appIdentity);
                            //    //identity.AddClaims(claims);

                            //}


                        }
                       

                    }

                   // Enviar correo electrónico de verificación de cuenta
                   var user = await _userManager.FindByNameAsync(Input.CI);
                    if (user != null && !await _userManager.IsEmailConfirmedAsync(user))
                    {
                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        var callbackUrl = Url.Page(
                            "/Account/ConfirmEmail",
                            pageHandler: null,
                            values: new { userId = user.Id, code = code },
                            protocol: Request.Scheme);

                        await _emailSender.SendEmailAsync(
                            user.Email,
                            "Verifique su dirección de correo electrónico",
                            $"Por favor, confirme su cuenta haciendo clic <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>aquí</a>."

                            );

                        // Mostrar mensaje de éxito o instrucciones al usuario
                        // (por ejemplo, "Se ha enviado un correo electrónico de verificación a su dirección de correo electrónico.")
                    }

                    return LocalRedirect(returnUrl);
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }



    }
}
