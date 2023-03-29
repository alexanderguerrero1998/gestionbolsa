using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Unach.DA.Empleo.Presentacion.CentralAdmin.Extensions
{
    public static class ExceptionHandlingExtension
    {
        /// <summary>
        /// Extensión para registrar el ExceptionHandler
        /// </summary>
        /// <param name="app">IApplicationBuilder</param>
        /// <param name="env">IHostingEnvironment</param>
        /// <param name="log">ILoggerFactory</param>
        public static void UseCustomExceptionHandler(this IApplicationBuilder app,
            IWebHostEnvironment env,
            ILoggerFactory log)
        {
            app.UseExceptionHandler(
                config =>
                {
                    config.Run(async context =>
                    {
                        // logger
                        var logger = log.CreateLogger(typeof(ExceptionHandlerExtensions));
                        try
                        {
                            // obtener la excepcion
                            var handler = context.Features.Get<IExceptionHandlerFeature>();
                            var exception = handler?.Error;
                            var remoteIpAddress = context.Connection?.RemoteIpAddress?.ToString();

                            if (exception != null)
                            {
                                logger.LogError($" ******* EXCEPCIÓN NO CONTROLADA ({remoteIpAddress}) ******* { exception.Message}  ");

                                //context.Response.ContentType = "application/json; charset=utf-8";

                                //switch (exception)
                                //{
                                //    case AppException _:
                                //        context.Response.StatusCode = (exception as AppException).StatusCode;
                                //        if ((exception as AppException).Exception != null)
                                //            logger.LogError($" ** Details: { (exception as AppException).Exception } ******");

                                //        await context.Response.WriteAsync(JsonConvert.SerializeObject(exception.Message));
                                //        break;
                                //    case SqlException _:
                                //        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                                //        await context.Response.WriteAsync(JsonConvert.SerializeObject("Hay problemas con el servidor de BD"));
                                //        break;
                                //    case SecurityTokenExpiredException _:
                                //        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                                //        await context.Response.WriteAsync(JsonConvert.SerializeObject("El token ha expirado"));
                                //        break;
                                //    case SecurityTokenException _:
                                //        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                                //        await context.Response.WriteAsync(JsonConvert.SerializeObject("Error validando token"));
                                //        break;
                                //    default:
                                //        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                                //        await context.Response.WriteAsync(JsonConvert.SerializeObject(string.Empty));
                                //        break;
                                //}
                            }
                            context.Response.Redirect("/Home/Error/0");
                        }
                        catch (Exception exception)
                        {
                            logger.LogCritical($"ERROR CRÍTICO{exception}");
                            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                            context.Response.ContentType = "application/json; charset=utf-8";
                            await context.Response.WriteAsync(JsonConvert.SerializeObject("Ocurrió un error, inténtelo más tarde"));
                        }
                    });
                });
        }
    }
}
