using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Unach.DA.Empleo.Dominio.Core;
using Unach.DA.Empleo.Persistencia.Core.Models;
using Unach.DA.Empleo.Presentacion.CentralAdmin.ViewModel;

namespace  Unach.DA.Empleo.Presentacion.CentralAdmin.Helpers
{
    public class MenuHelper
    {
        EntitiesDomain context;
        public MenuHelper(DbContextOptions<SicoaContext> _options)
        {
            context = new EntitiesDomain(_options);
        }


        
        public List<MenuItemViewModel> GetAllMenuItems(string idServidor, List<RolViewModel> roles)
        {
            //string rolesIds = roles.Select(x => x.Id.ToString()).ToList().ToUnSplit();
            List<TransaccioMenuViewModel> transaccions = new List<TransaccioMenuViewModel>();
            transaccions = context.ExecuteStoredProcedure<TransaccioMenuViewModel>("Auth.GetTransaccionesBySistemaUsuario", ("idSistema", 1),   ("idUsuario", idServidor)).ToList();
            List<MenuItemViewModel> items = new List<MenuItemViewModel>();

            foreach (var item in transaccions.Where(x=>x.Visible && x.Activo).ToList())
            {
                items.Add(new MenuItemViewModel()
                {
                    Id=item.Id,
                    IdPadre=item.IdPadre,
                    Titulo=item.Titulo,
                    Controlador=item.Controlador,
                    Accion=item.Accion,
                    IconClass=item.IconClass,
                    Orden=item.Orden
                });
            }
            return items;
        }


        List<MenuItemViewModel> GetChildrenMenu(List<MenuItemViewModel> menuList, int? parentId = null)
        {
            return menuList.Where(x => x.IdPadre == parentId).OrderBy(x => x.Orden).ToList();
        }

        MenuItemViewModel GetMenuItem(IList<MenuItemViewModel> menuList, int id)
        {
            return menuList.FirstOrDefault(x => x.Id == id);
        }


        public List<MenuItemViewModel> GetMenu(List<MenuItemViewModel> menuList, int? parentId)
        {
            var children = GetChildrenMenu(menuList, parentId);

            if (!children.Any())
            {
                return new List<MenuItemViewModel>();
            }

            var vmList = new List<MenuItemViewModel>();

            foreach (var item in children)
            {
                var menu = GetMenuItem(menuList, item.Id);

                var vm = new MenuItemViewModel();

                vm.Id = menu.Id;
                vm.Titulo = menu.Titulo;
                vm.IconClass = menu.IconClass;
                vm.Controlador = menu.Controlador;
                vm.Accion = menu.Accion;
                vm.InverseIdPadreNavigation = GetMenu(menuList, menu.Id);

                vmList.Add(vm);
            }

            return vmList;
        }





    }
}
