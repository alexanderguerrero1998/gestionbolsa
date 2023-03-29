using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Unach.DA.Empleo.Presentacion.CentralAdmin.Utils.Files
{
    public class ManejadorArchivos
    {
       public static string GetValidFileName(string fileName)
        {
            // remove any invalid character from the filename.
            String ret = Regex.Replace(fileName.Trim(), "[^A-Za-z0-9_. ]+", "");
            return ret.Replace(" ", String.Empty);
        }


        /// <summary>
        /// Carga un archivo en el servidor de documentos
        /// </summary>
        /// <param name="archivo">Documento a cargar</param>
        /// <param name="serverPath">Ruta en el servidor del tipo: \\1.1.1.1\{EXPEDIENTE}\Directorio\{File.ext}</param>
        /// <param name="fileName">Nombre para el archivo a guardar sin extension</param>
        /// <returns></returns>
        public static bool GuardarArchivo(IFormFile archivo, string serverPath,string fileName=null)
        {
            bool resultado = false;

            string directory = Path.GetDirectoryName(serverPath);
            string extension = Path.GetExtension(archivo.FileName);

            string nombreArchivo = (fileName != null ? Path.GetFileNameWithoutExtension(fileName) : Path.GetFileNameWithoutExtension(serverPath)) + extension;
            

            string savePath = Path.Combine(directory,  nombreArchivo);


            
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(Path.GetFullPath(directory));


            using (FileStream fs = System.IO.File.Create(savePath))
            {
                archivo.CopyTo(fs);
                fs.Flush();
                resultado = true;
            }

            return resultado;
        }

        public static MemoryStream DescargarArchivo(string nombreArchivo)
        {
            if (!string.IsNullOrEmpty(nombreArchivo))
            {
                var path = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "Archivos", nombreArchivo);
                var memory = new MemoryStream();
                using (var stream = new FileStream(path, FileMode.Open))
                {
                    stream.CopyTo(memory);
                }
                memory.Position = 0;
                return memory;
            }
            return null;
        }
    }
}
