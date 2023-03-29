using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Unach.DA.Empleo.Presentacion.CentralAdmin.Helpers
{
    public class ImageHelpers : Controller
    {
        // string rutaSinUsuario = @"\\192.168.199.25\comun\no_user.jpg";
        string rutaSinUsuario = $"{Directory.GetCurrentDirectory()}{@"\wwwroot\public\images\no_user.jpg"}";


        public static byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
        public byte[] GetImageBytes(string urlFoto)
        {
            FileStream image = null;
            try
            {
                if (urlFoto != null && urlFoto.Trim().Length > 0)
                {
                    image = System.IO.File.OpenRead(urlFoto);
                }
                else
                {
                    image = System.IO.File.OpenRead(rutaSinUsuario);
                }
            }
            catch (Exception ex)
            {
                image = System.IO.File.OpenRead(rutaSinUsuario);
            }
            return ReadFully(image);
        }


        public FileStreamResult GetImage(string urlFoto)
        {
            FileStream image = null;
            try
            {
                if (urlFoto != null && urlFoto.Trim().Length > 0)
                {
                    image = System.IO.File.OpenRead(urlFoto);
                }
                else
                {
                    image = System.IO.File.OpenRead(rutaSinUsuario);
                }
            }
            catch (Exception ex)
            {
                image = System.IO.File.OpenRead(rutaSinUsuario);
            }
            return File(image, "image/jpeg");
        }
    }
}
