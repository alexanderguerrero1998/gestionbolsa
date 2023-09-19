using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Unach.DA.Empleo.Dominio.Core;
using Unach.DA.Empleo.Persistencia.Core.Models;
using Unach.DA.Empleo.Presentacion.CentralAdmin.Extensions;
using Unach.DA.Empleo.Presentacion.CentralAdmin.ViewModel;

namespace Unach.DA.Empleo.Presentacion.CentralAdmin.Controllers
{
    public class AdministradorController : Controller
    {
        EntitiesDomain entitiesDomain;
        private ILogger logger;
        private readonly IMapper _mapper;

        public AdministradorController(DbContextOptions<SicoaContext> options, IMapper mapper, ILoggerFactory log)
        {
            _mapper = mapper;
            entitiesDomain = new EntitiesDomain(options);
            logger = log.CreateLogger(typeof(AdministradorController));
        }
        public IActionResult Index()
        {
            try 
            { 
                



            } catch (Exception ex) 
            {
                
                

            }    

            return View();
        }
        public IActionResult Roles()
        {
            try
            {
                int expediente = 0;
                ViewBag.Expediente = expediente;

                List<RolViewModel> roles = entitiesDomain.RolRepositorio.ObtenerTodosEnOtraVista(
                    m => new RolViewModel
                    {
                        Id = m.Id,
                        IdSistema = m.IdSistema,
                        Nombre = m.Nombre,      
                        Descripcion = m.Descripcion,
                        Activo = m.Estado
     
                    },
                    x => x.Id > expediente,
                    a => a.OrderBy(y => y.Id));

                return View(roles.ToList());

            }
            catch (Exception e)
            {
                TempData.MostrarAlerta(ViewModel.TipoAlerta.Exitosa, "Error"+e);
                return View("Index");

            }

        }
        public IActionResult RolEdit(int id) 
        {
            try
            {
                if (id == 0)
                {
                    RolViewModel roles = new RolViewModel();
                    return PartialView("~/Views/Administrador/_RolEdit.cshtml", roles);
                }
                else
                {
                    var query = entitiesDomain.RolRepositorio.ObtenerTodosEnOtraVista<RolViewModel>(

                        m => new RolViewModel
                        {
                            Id = m.Id,
                            IdSistema=m.IdSistema,
                            Nombre = m.Nombre,
                            Descripcion=m.Descripcion,
                            Activo = m.Estado
    
                        },
                        x => x.Id == id).FirstOrDefault();

                    if (query != null)
                    {
                        return PartialView("~/Views/Administrador/_RolEdit.cshtml", query);
                    }
                    else
                        return PartialView("~/Views/Administrador/_RolEdit.cshtml", new RolViewModel());
                }

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error");
                TempData.MostrarAlerta(ViewModel.TipoAlerta.Error, "Error! " + ex.Message);
                return PartialView("~/Views/Administrador/_RolEdit.cshtml", new RolViewModel());
            }
    }
        [HttpPost]
        public IActionResult RolEdit(RolViewModel item)
        {
            try
            {
                //if (ModelState.IsValid)
                // {
                // item.Id = item.Id == -1 ? null : item.Id;

                if (item.Id == 0)
                {
                    var rol = _mapper.Map<Rol>(item);

                    rol.Estado = item.Activo;
                    _mapper.AgregarDatosAuditoria(rol, HttpContext);
                    entitiesDomain.RolRepositorio.Insertar(rol);
                    entitiesDomain.GuardarTransacciones();

                    TempData.MostrarAlerta(ViewModel.TipoAlerta.Exitosa, "Información registrada.");
                }
                else
                {
                    var rol = _mapper.Map<Rol>(item);
                    rol.Estado = item.Activo;
                    _mapper.AgregarDatosAuditoria(rol, HttpContext);
                    entitiesDomain.RolRepositorio.Actualizar(rol);
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
            // return RedirectToAction(nameof(Index), new { expediente = 1 });
            return RedirectToAction("Roles", "Administrador");
        }
        public IActionResult RolDelete(int id)
        {
            try
            {
                Rol item = entitiesDomain.RolRepositorio.BuscarPor(x => x.Id == id).FirstOrDefault();
                return PartialView("~/Views/Administrador/_RolDelete.cshtml", item);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error");
                TempData.MostrarAlerta(ViewModel.TipoAlerta.Error, "Error!" + ex.Message);
            }
            return View();
        }
        [HttpPost]
        public IActionResult RolDelete(Rol item)
        {
            try
            {
                if (item != null)
                {
                    entitiesDomain.RolRepositorio.Eliminar(item);
                    entitiesDomain.GuardarTransacciones();
                    TempData.MostrarAlerta(ViewModel.TipoAlerta.Exitosa, "Información eliminada.");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error");
                TempData.MostrarAlerta(ViewModel.TipoAlerta.Error, "Error! " + ex.Message);
            }
            //return RedirectToAction(nameof(Index), new { expediente = 1 });
            return RedirectToAction("Roles", "Administrador");
        }
        public IActionResult Modulos() 
        {
            try
            {
                int expediente = 0;
                ViewBag.Expediente = expediente;

                List<ModuloViewModel> modulos = entitiesDomain.ModuloRepositorio.ObtenerTodosEnOtraVista(
                    m => new ModuloViewModel
                    {
                        Id = m.Id,
                        IdSistema = m.IdSistema,
                        Nombre = m.Nombre,
                        Descripcion = m.Descripcion,

                    },
                    x => x.Id > expediente,
                    a => a.OrderBy(y => y.Id));

                return View(modulos.ToList());

            }
            catch (Exception e)
            {
                TempData.MostrarAlerta(ViewModel.TipoAlerta.Exitosa, "Error" + e);
                return View("Index");

            }

            return View(); 
        }
        public IActionResult ModuloEdit(int id)
        {
            try
            {
                if (id == 0)
                {
                    ModuloViewModel modulo = new ModuloViewModel();
                    return PartialView("~/Views/Administrador/_ModuloEdit.cshtml", modulo);
                }
                else
                {
                    var query = entitiesDomain.ModuloRepositorio.ObtenerTodosEnOtraVista<ModuloViewModel>(

                        m => new ModuloViewModel
                        {
                            Id = m.Id,
                            IdSistema = m.IdSistema,
                            Nombre = m.Nombre,
                            Descripcion = m.Descripcion,

                        },
                        x => x.Id == id).FirstOrDefault();

                    if (query != null)
                    {
                        return PartialView("~/Views/Administrador/_ModuloEdit.cshtml", query);
                    }
                    else
                        return PartialView("~/Views/Administrador/_ModuloEdit.cshtml", new ModuloViewModel());
                }

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error");
                TempData.MostrarAlerta(ViewModel.TipoAlerta.Error, "Error! " + ex.Message);
                return PartialView("~/Views/Administrador/_RolEdit.cshtml", new ModuloViewModel());
            }
        }
        [HttpPost]
        public IActionResult ModuloEdit(ModuloViewModel item)
        {
            try
            {
                //if (ModelState.IsValid)
                // {
                // item.Id = item.Id == -1 ? null : item.Id;

                if (item.Id == 0)
                {
                    var modulo = _mapper.Map<Modulo>(item);

                  
                    _mapper.AgregarDatosAuditoria(modulo, HttpContext);
                    entitiesDomain.ModuloRepositorio.Insertar(modulo);
                    entitiesDomain.GuardarTransacciones();

                    TempData.MostrarAlerta(ViewModel.TipoAlerta.Exitosa, "Información registrada.");
                }
                else
                {
                    var modulo = _mapper.Map<Modulo>(item);
                   
                    _mapper.AgregarDatosAuditoria(modulo, HttpContext);
                    entitiesDomain.ModuloRepositorio.Actualizar(modulo);
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
            // return RedirectToAction(nameof(Index), new { expediente = 1 });
            return RedirectToAction("Modulos", "Administrador");
        }
        public IActionResult ModuloDelete(int id)
        {
            try
            {
                Modulo item = entitiesDomain.ModuloRepositorio.BuscarPor(x => x.Id == id).FirstOrDefault();
                return PartialView("~/Views/Administrador/_ModuloDelete.cshtml", item);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error");
                TempData.MostrarAlerta(ViewModel.TipoAlerta.Error, "Error!" + ex.Message);
            }
            return View();
        }
        [HttpPost]
        public IActionResult ModuloDelete(Modulo item)
        {
            try
            {
                if (item != null)
                {
                    entitiesDomain.ModuloRepositorio.Eliminar(item);
                    entitiesDomain.GuardarTransacciones();
                    TempData.MostrarAlerta(ViewModel.TipoAlerta.Exitosa, "Información eliminada.");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error");
                TempData.MostrarAlerta(ViewModel.TipoAlerta.Error, "Error! " + ex.Message);
            }
            //return RedirectToAction(nameof(Index), new { expediente = 1 });
            return RedirectToAction("Modulos", "Administrador");
        }
        public IActionResult Transacciones() 
        {
            try
            {
                int expediente = 0;
                ViewBag.Expediente = expediente;

                List<TransaccionViewModel> roles = entitiesDomain.TransaccionRepositorio.ObtenerTodosEnOtraVista(
                    m => new TransaccionViewModel
                    {
                        Id = m.Id,
                        Titulo = m.Titulo,
                        Descripcion = m.Descripcion,
                        Controlador = m.Controlador,
                        Accion = m.Accion,
                        IconClass = m.IconClass,
                    },
                    x => x.Id > expediente,
                    a => a.OrderBy(y => y.Id));

                return View(roles.ToList());

            }
            catch (Exception e)
            {
                TempData.MostrarAlerta(ViewModel.TipoAlerta.Exitosa, "Error" + e);
                return View("Index");

            }

        }
        public IActionResult TransaccionEdit(int id)
        {
            try
            {
                if (id == 0)
                {
                    TransaccionViewModel transacciones = new TransaccionViewModel();
                    transacciones.ListaModulos = entitiesDomain.ModuloRepositorio.ObtenerTodos();

                    return PartialView("~/Views/Administrador/_TransaccionEdit.cshtml", transacciones);
                }
                else
                {
                    var query = entitiesDomain.TransaccionRepositorio.ObtenerTodosEnOtraVista<TransaccionViewModel>(

                        m => new TransaccionViewModel
                        {
                            Id = m.Id,
                            IdPadre = m.IdPadre,    
                            IdModulo = m.IdModulo,
                            Titulo = m.Titulo,
                            Descripcion = m.Descripcion,
                            Accion=m.Accion,
                            Controlador = m.Controlador,  
                            Orden = m.Orden,
                            Estado = m.Estado,
                            Visible = m.Visible,
                            IconClass = m.IconClass,   
                            Activo = m.Activo,
                        },
                        x => x.Id == id).FirstOrDefault();

                    if (query != null)
                    {
                        query.ListaModulos = entitiesDomain.ModuloRepositorio.ObtenerTodos();
                        return PartialView("~/Views/Administrador/_TransaccionEdit.cshtml", query);
                    }
                    else
                        return PartialView("~/Views/Administrador/_TransaccionEdit.cshtml", new TransaccionViewModel());
                }

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error");
                TempData.MostrarAlerta(ViewModel.TipoAlerta.Error, "Error! " + ex.Message);
                return PartialView("~/Views/Administrador/_TransaccionEdit.cshtml", new TransaccionViewModel());
            }
        }
        [HttpPost]
        public IActionResult TransaccionEdit(TransaccionViewModel item)
        {
            try
            {
                // if (ModelState.IsValid)
                // {
                // item.Id = item.Id == -1 ? null : item.Id;

                if (item.Id == 0)
                {
                    var transaccion = _mapper.Map<Transaccion>(item);

                    _mapper.AgregarDatosAuditoria(transaccion, HttpContext);
                    entitiesDomain.TransaccionRepositorio.Insertar(transaccion);
                    entitiesDomain.GuardarTransacciones();

                    TempData.MostrarAlerta(ViewModel.TipoAlerta.Exitosa, "Información registrada.");
                }
                else
                {
                    var transaccion = _mapper.Map<Transaccion>(item);

                    _mapper.AgregarDatosAuditoria(transaccion, HttpContext);
                    entitiesDomain.TransaccionRepositorio.Actualizar(transaccion);
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

            return RedirectToAction("Transacciones", "Administrador");
        }
        public IActionResult TransaccionDelete(int id)
        {
            try
            {
                Transaccion item = entitiesDomain.TransaccionRepositorio.BuscarPor(x => x.Id == id).FirstOrDefault();
                return PartialView("~/Views/Administrador/_TransaccionDelete.cshtml", item);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error");
                TempData.MostrarAlerta(ViewModel.TipoAlerta.Error, "Error!" + ex.Message);
            }
            return View();
        }
        [HttpPost]
        public IActionResult TransaccionDelete(Transaccion item)
        {
            try
            {
                if (item != null)
                {
                    entitiesDomain.TransaccionRepositorio.Eliminar(item);
                    entitiesDomain.GuardarTransacciones();
                    TempData.MostrarAlerta(ViewModel.TipoAlerta.Exitosa, "Información eliminada.");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error");
                TempData.MostrarAlerta(ViewModel.TipoAlerta.Error, "Error! " + ex.Message);
            }
            //return RedirectToAction(nameof(Index), new { expediente = 1 });
            return RedirectToAction("Transacciones", "Administrador");
        }
        public IActionResult RolUsuario()
        {
            try
            {
                int expediente = 0;
                ViewBag.Expediente = expediente;

                List<RolUsuarioViewModel> roles = entitiesDomain.RolUsuarioRepositorio.ObtenerTodosEnOtraVista(
                    m => new RolUsuarioViewModel
                    {
                     IdRol = m.IdRol,   
                     IdUsuario = m.IdUsuario,   
                     Desde = m.Desde,
                     Hasta = m.Hasta,
                    },
                    x => x.IdRol > expediente,
                    a => a.OrderBy(y => y.IdRol));

                return View(roles.ToList());

            }
            catch (Exception e)
            {
                TempData.MostrarAlerta(ViewModel.TipoAlerta.Exitosa, "Error" + e);
                return View("Index");

            }

        }


    }
}
