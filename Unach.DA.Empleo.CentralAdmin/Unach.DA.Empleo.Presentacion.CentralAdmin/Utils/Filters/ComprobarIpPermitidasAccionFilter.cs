//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Filters;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Logging;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Threading.Tasks;

//namespace Unach.Codesi.Cgrni.Cooperacion.Presentation.CentralAdmin.Utils.Filters
//{
//    public static class DireccionIpExtension 
//    {
//        public static TipoIP GetTipoIp(this string ip)
//        {
//            TipoIP tipoIp = TipoIP.NoValida;
//            var octetos = ip.Split('.').ToList();
            
//            tipoIp = octetos.Count switch 
//            {
//                4 => TipoIP.DireccionIp,
//                1 => TipoIP.NoValida,
//                _ => TipoIP.NoValida
//            };
//            return tipoIp;
//        }
//    }
//    /// <summary>
//    /// 
//    /// </summary>
//    public enum TipoIP
//    {
//        NoValida,
//        DireccionIp,
//        SubRed
//    }

//    public class DireccionIP
//    {
//        public string IP { get; set; }
//        public TipoIP Tipo { get; set; }
//    }
//    public class ComprobarIpPermitidasAccionFilter : ActionFilterAttribute
//    {
//        private readonly ILogger _logger;
//        private readonly string _safelist;

//        private List<string[]> ipsPermitidas;

//        EntitiesDomain entitiesDomain;

//        public ComprobarIpPermitidasAccionFilter(string safelist,
//            DbContextOptions<DathGestionContext> options,
//            ILoggerFactory log)
//        {
//            _safelist = safelist;
//            _logger = log.CreateLogger(typeof(ComprobarIpPermitidasAccionFilter));
//            entitiesDomain = new EntitiesDomain(options);

//            #region Normalizar IPs configuradas
//            List<string> listaIpsPermitidas = _safelist.Split(";").ToList();
//            ipsPermitidas = new List<string[]>();
//            foreach (var item in listaIpsPermitidas)
//            {
//                var ip = item.Split(".");
//                if (ip.Length == 1)
//                {
//                    if (item == "*")
//                    {
//                        string[] newElement = Enumerable.Repeat("*", 4).ToArray();
//                        ipsPermitidas.Add(newElement);
//                    }
//                }
//                else if (ip.Length == 4)
//                {
//                    ipsPermitidas.Add(ip);
//                }
//                else if (ip.Length == 2 || ip.Length == 3)
//                {
//                    List<string> vs = new List<string>();
//                    vs.AddRange(ip.ToList());

//                    for (int i = ip.Length; i < 4; i++)
//                    {
//                        vs.Add("*");
//                    }
//                    ipsPermitidas.Add(vs.ToArray());
//                }
//            } 
//            #endregion
//        }

//        public override void OnActionExecuting(ActionExecutingContext context)
//        {
//            var remoteIp = context.HttpContext.Connection.RemoteIpAddress;
//            _logger.LogInformation("Remote IpAddress: {RemoteIp}", remoteIp);

//            bool esDocentePeriodoActual = false;

//            try
//            {
//                var query = entitiesDomain.ExecuteStoredProcedureSingleFieldResult<int>("Dath.ComprobarEsDocentePeriodoActual", 
//                    ("emailServidor", context.HttpContext.ServidorAutenticado().Email));
//                esDocentePeriodoActual = query.FirstOrDefault() == 1;
//            }
//            catch (Exception errSp)
//            {
//                _logger.LogError(errSp, "Error - ComprobarEsDocentePeriodoActual");
//            }

//            if (!esDocentePeriodoActual)
//            {
//                var valido = false;
//                if (ipsPermitidas.Count > 0)
//                {
//                    try
//                    {
//                        if (remoteIp.IsIPv4MappedToIPv6)
//                        {
//                            remoteIp = remoteIp.MapToIPv4();
//                        }

//                        var octetosIp = remoteIp.ToString().Split(".");
//                        // IP v4 válida
//                        if (octetosIp?.Length == 4)
//                        {
//                            valido = ipsPermitidas.Where(
//                                    filtro => filtro[0] == "*"
//                                        || (octetosIp[0] == filtro[0]
//                                        && (octetosIp[1] == filtro[1] || filtro[1] == "*")
//                                        && (octetosIp[2] == filtro[2] || filtro[2] == "*")
//                                        && (octetosIp[3] == filtro[3] || filtro[3] == "*"))
//                                    ).Count() > 0;
//                        }

//                        if (!valido)
//                        {
//                            _logger.LogError($"FORBIDDEN REQUEST. No se ha permitido el acceso a la IP: {remoteIp} para el usuario: {context.HttpContext.ServidorAutenticado()?.Email}");
//                            //(context.Controller as Controller).TempData.MostrarAlerta(ViewModel.TipoAlerta.Advertencia, "PANTALLA EN MANTENIMIENTO");

//                            //context.Result = new StatusCodeResult(StatusCodes.Status401Unauthorized);
//                            //context.HttpContext.Response.Redirect("/Home/AccesoRestringidoRed");
//                            //return;


//                            Microsoft.AspNetCore.Routing.RouteValueDictionary redirectTargetDictionary = 
//                                new Microsoft.AspNetCore.Routing.RouteValueDictionary();
//                            redirectTargetDictionary.Add("action", "AccesoRestringidoRed");
//                            redirectTargetDictionary.Add("controller", "Home");
//                            context.Result = new RedirectToRouteResult(redirectTargetDictionary);
//                            context.Result.ExecuteResultAsync(context);
//                        }
//                    }
//                    catch (Exception err)
//                    {
//                        _logger.LogError(err, "Error");
//                    }
//                } 
//            }
//            base.OnActionExecuting(context);
//        }
//    }
//}
