using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using DevExpress.XtraCharts;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Unach.DA.Empleo.Dominio.Core;
using Unach.DA.Empleo.Persistencia.Core.Models;
using Unach.DA.Empleo.Presentacion.CentralAdmin.Controllers;
using Unach.DA.Empleo.Presentacion.CentralAdmin.Models;
using Unach.DA.Empleo.Presentacion.CentralAdmin.ViewModel;
using Unach.DA.Empleo.Presistencia.Api;
using AutoMapper;
using Unach.DA.Empleo.Presentacion.CentralAdmin.Extensions;
using DevExpress.CodeParser;

namespace Unach.DA.Empleo.Presentacion.CentralAdmin.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {

        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        // Yo inserte esto
        EntitiesDomain entitiesDomain;
        private ILogger logger;
        private readonly IMapper _mapper;

        public RegisterModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,

            // Yo inserte esto
            DbContextOptions<SicoaContext> options, IMapper mapper, ILoggerFactory log

            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            // Yo inserte esto
            _mapper = mapper;
            entitiesDomain = new EntitiesDomain(options);
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [Display(Name = "Cedula")]
            public string CI { get; set; }

            [Required]
            [Display(Name = "LinKedin")]
            public string Linkedin { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
           // returnUrl ??= Url.Action("Login", "Account", new { area = "Identity" }); // Cambia según tus necesidades
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var ci = Input.CI;
                // Contar la cantidad de dígitos en la cédula de identidad 
                int cantidadDigitos = ci.Length;
                if (cantidadDigitos == 10)
                {
                    // Verificar si el Estudiante ya existe en la API
                    bool userExists = await UserExistsInApi(ci);

                    if (userExists)
                    {

                        // Crear el nuevo usuario
                        var user = new IdentityUser { UserName = Input.CI, Email = Input.Email };
                        var result = await _userManager.CreateAsync(user, Input.Password);

                        if (result.Succeeded)
                        {
                            // Llamamos a la API para obtener  los valores y asignalos en tabla ESTUDIANTE
                            ClienteApi clienteapi = new ClienteApi("");
                            var response = clienteapi.Get<Api>("https://pruebas.unach.edu.ec:4431/api/Estudiante/InformacionBasicaPorCriterio/" + ci);


                            Estudiante estudianteNuevo = new Estudiante();
                            estudianteNuevo.Id = user.Id;
                            estudianteNuevo.IdEstudiante = response.EstudianteID;
                            estudianteNuevo.LinkLinkeding = Input.Linkedin;

                            // Agregamos datos de adutoria de la tabla Estudiante
                            var estudiante = _mapper.Map<Estudiante>(estudianteNuevo);

                            _mapper.AgregarDatosAuditoria(estudiante, HttpContext);


                            entitiesDomain.EstudianteRepositorio.Insertar(estudiante);
                            entitiesDomain.GuardarTransacciones();



                            //Agregamos roles al usuario
                            RolUsuario rolUsuarioNuevo = new RolUsuario();
                            rolUsuarioNuevo.IdRol = 5; // Este es el rol de estudiante
                            rolUsuarioNuevo.IdUsuario = user.Id;
                            rolUsuarioNuevo.Desde = DateTime.Now;
                            rolUsuarioNuevo.Hasta = DateTime.Now.AddYears(1);



                            // Agregamos datos de adutoria de la tabla RolUsuario
                            var roles = _mapper.Map<RolUsuario>(rolUsuarioNuevo);
                            _mapper.AgregarDatosAuditoria(roles, HttpContext);

                            entitiesDomain.RolUsuarioRepositorio.Insertar(roles);
                            entitiesDomain.GuardarTransacciones();

                           // TempData.MostrarAlerta(ViewModel.TipoAlerta.Exitosa, "Usuario registrado.");

                            _logger.LogInformation("User created a new account with password.");
                            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                            var callbackUrl = Url.Page(
                                "/Account/ConfirmEmail",
                                pageHandler: null,
                                values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                                protocol: Request.Scheme);

                            // Enviamos un correo de confirmacion
                            await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                                $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                            if (_userManager.Options.SignIn.RequireConfirmedAccount)
                            {
                                return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                            }
                            else
                            {
                                await _signInManager.SignInAsync(user, isPersistent: false);

                                return RedirectToPage("/Account/Login", new { area = "Identity" });
                                // return LocalRedirect(returnUrl);

                            }
                        }
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                    else 
                    {
                        // Crear el nuevo usuario que no es estudiante o egresado
                        var user = new IdentityUser { UserName = Input.CI, Email = Input.Email };
                        var result = await _userManager.CreateAsync(user, Input.Password);

                        if (result.Succeeded)
                        {
                            //TempData.MostrarAlerta(ViewModel.TipoAlerta.Exitosa, "Funcionario Registrado.");

                            _logger.LogInformation("User created a new account with password.");
                            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                            var callbackUrl = Url.Page(
                                "/Account/ConfirmEmail",
                                pageHandler: null,
                                values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                                protocol: Request.Scheme);

                            // Enviamos un correo de confirmacion
                            await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                                $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                            if (_userManager.Options.SignIn.RequireConfirmedAccount)
                            {
                                return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                            }
                            else
                            {
                                await _signInManager.SignInAsync(user, isPersistent: false);

                                return RedirectToPage("/Account/Login", new { area = "Identity" });
                                // return LocalRedirect(returnUrl);

                            }
                        }
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }

                    }
                } else
                {
                    TempData.MostrarAlerta(ViewModel.TipoAlerta.Advertencia, "Usuario Invalido");
                }
                

            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        private async Task<bool> UserExistsInApi(string ci)
        {
            ApiController apiController = new ApiController();
            var a = apiController.Verificar(ci);
            if (a) { return true; }
            else { return false; }

        }





    }
}
