using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unach.DA.Empleo.Presentacion.CentralAdmin.Helpers;
using Unach.DA.Empleo.Presentacion.CentralAdmin.Extensions;
using Unach.DA.Empleo.Persistencia.Core.Models;

namespace Unach.DA.Empleo.Presentacion.CentralAdmin.ViewComponents
{
    public class MenuViewComponent: ViewComponent
    {
        MenuHelper menus ;
        public MenuViewComponent(DbContextOptions<SicoaContext> _options)
        {
            menus = new MenuHelper(_options);
        }


        public IViewComponentResult Invoke()
        {


            var query = menus.GetAllMenuItems(HttpContext.ServidorAutenticado().IdServidor);//, HttpContext.ServidorAutenticado().Roles);
            var lista = menus.GetMenu(query, null);
            return View(lista);

        }
            
    }
}
