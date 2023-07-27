using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;

namespace Unach.DA.Empleo.Presentacion.CentralAdmin.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LogoutModel : PageModel
    {
        HttpContext hcontext;

        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<LogoutModel> _logger;

        public LogoutModel(SignInManager<IdentityUser> signInManager, ILogger<LogoutModel> logger)
        {
            _signInManager = signInManager;
            _logger = logger;
        }

        public void OnGet()
        {
        }

        //public async Task<IActionResult> OnPost(string returnUrl = null)
        //{
        //    await _signInManager.SignOutAsync();
        //    _logger.LogInformation("User logged out.");
        //    if (returnUrl != null)
        //    {
        //        HttpContext.Session.Clear();
        //        return LocalRedirect(returnUrl);
        //    }
        //    else
        //    {
        //        return RedirectToPage();
        //    }
        //}

        public async Task<IActionResult> OnPost()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");

            HttpContext.Session.Clear();
            //return LocalRedirect("");
            //return RedirectToPage();
            return Redirect("/Identity/Account/Login");


        }
    }
}
