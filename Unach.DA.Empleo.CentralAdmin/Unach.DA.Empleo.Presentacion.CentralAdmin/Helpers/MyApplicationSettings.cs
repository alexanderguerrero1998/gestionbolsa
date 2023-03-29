using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace Unach.DA.Empleo.Presentacion.CentralAdmin.Helpers
{
    public class MyApplicationSettings
    {
        private static volatile MyApplicationSettings instance;
        private static object syncRoot = new Object();
        private MyApplicationSettings() { }

        public static MyApplicationSettings Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new MyApplicationSettings();
                    }
                }
                return instance;
            }
        }


        private string urlFotoServidorDocumentos;

        public string UrlFotoServidorDocumentos
        {
            get { return urlFotoServidorDocumentos; }
            set { urlFotoServidorDocumentos = value; }
        }


        public string UrlServidorDocumentos { get; set; }
        

    }
}
