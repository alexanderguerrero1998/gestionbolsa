using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Unach.DA.Empleo.Presentacion.CentralAdmin.Utils
{
    public class ConsolaHelpers
    {
        public static void ImprimirObjeto(object objeto)
        {
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(objeto, Newtonsoft.Json.Formatting.Indented));
        }
    }
}
