using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Unach.DA.Empleo.Presentacion.CentralAdmin.Utils.Filters
{
    public class FuncionalidadMantenimientoFilter : ActionFilterAttribute
    {
        private bool esAmbienteDesarrollo;
        public FuncionalidadMantenimientoFilter(bool esDesarrollo)
        {
            esAmbienteDesarrollo = esDesarrollo;
        }


        public override void OnActionExecuting(ActionExecutingContext context)
        {

            if (!esAmbienteDesarrollo)
            {
                Microsoft.AspNetCore.Routing.RouteValueDictionary redirectTargetDictionary =
                                        new Microsoft.AspNetCore.Routing.RouteValueDictionary();
                redirectTargetDictionary.Add("action", "EnMantenimiento");
                redirectTargetDictionary.Add("controller", "Home");
                context.Result = new RedirectToRouteResult(redirectTargetDictionary);
                context.Result.ExecuteResultAsync(context); 
            }

            base.OnActionExecuting(context);
        }
    }
}
