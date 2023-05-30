using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Unach.DA.Empleo.Presistencia.Api
{
    public class ClienteApi
    {

        // Recuerda que el token tiene vigencia de 24 hora, importante actualizarlo
        // Ingresa aqui para generar el token: https://manage.auth0.com/dashboard/us/dev-bb4zah0ivws1wedd/apis/management/explorer
        // Aqui para hacer el SETOKEN: https://auth0.com/docs/api/management/v2
        // Ingresa aqui para generar la data del token: https://auth0.com/docs/api/management/v2#!/Client_Grants/get_client_grants
        // Aqui revisas si vale el token:https://jwt.io/


        private string token = "eyJhbGciOiJSUzI1NiIsImtpZCI6IjQ1NDA2M0FBMkVCREVGQzdBNTNGRDM4MDE0REYwMTFFMjkxQTAyNEYiLCJ0eXAiOiJhdCtqd3QiLCJ4NXQiOiJSVUJqcWk2OTc4ZWxQOU9BRk44Qkhpa2FBazgifQ.eyJuYmYiOjE2ODUzOTMwMTgsImV4cCI6MTY4NzEyMTAxOCwiaXNzIjoiaHR0cHM6Ly9wcnVlYmFzLnVuYWNoLmVkdS5lYzo0NDMwIiwiYXVkIjoiQWNhZGVtaWNvQVBJIiwiY2xpZW50X2lkIjoiMDBjODJmNjctNmFkYy00ZTJiLTllODItMWE5M2MzMWE2ZjM2IiwiY2xpZW50ZSI6IkJvbHNhIGRlIEVtcGxlbyIsImFtYmllbnRlIjoiRGVzYXJyb2xsbyIsImp0aSI6IjBzZVkwWG54V2pKS1otMFhTOGJPenciLCJzY29wZSI6WyJhY2FkZW1pY28uYXBpLmxlY3R1cmEiXX0.Nb0eYr4twydZC6ULgXRqpO3VQ7yx-41FcRgs3BDTVfWwO7oZDV_1-Xtd5HmHoR93HC4KFHPVY9i3n8a_Dg9DxUdcpcEmpGdWPjFdoPHVlCQy5JgD6Nqxl_NNprUG8lE2kR68W4H-CqMY-o0B6M7rYuiPBYsuSHpkl99SfpI13AmEl9Gx9ssPPeHP9ziaR9njBqroet5yYnliIkIC7p2piuN3IE_-GAbtujzum7tq8iwGC-lDyoSDzU-jQFqOvr8W3L85sjt989pi5NYSbYFodpaizGBCycjElxJ4iyn1rX4WjILHtnjkJWzMpwRMb4HIL48h5heYm8wtgi1mgMr2mq7g-FTXGssHoPqcsxT5ubbHwE8B48I-6GcVjG9uoKWT0EUrlYt5QDhWL_0BcAJbkNXpJxgiqcnO36mCJK6-9mIJTtJ88cl11EgIy_tie5ueR9nYP86uvkSebBqXzEOenGKwox3TtO2QdmzIpyq_t58719gVVYGZMijtrT4VhDetnJjxSh-x-cHZQ7M-WtEdbvVTBJhWPw6zYxFexV3ETRpKqw-oHMR8kaYgk67aI95m7wyzYOnbNxmBhQ3_HK6Dq-QB3PXW5wn1dd-LZ6w-_ilxj9x3HUfi-2oJEDzXlHakmXLHUguaenkulaChMRfBNoFceuX_fNsEUKNP0VhpNyc";
        public string Error { get; set; }

        public ClienteApi(string token)
        {
            this.token = token;
        }

        /// <summary>
        /// Obtener datos de la Api
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <returns></returns>
        public T Get<T>(string url) where T : class
        {
            T result = null;
            result = GetGeneric(url, result, data => JsonConvert.DeserializeObject<T>(data));
            return result;
        }


        /// <summary>
        /// Obtener un resultado booleano desde la api
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public bool GetBoolean(string url)
        {
            bool result = false;
            result = GetGeneric<bool>(url, result, x => JsonConvert.DeserializeObject<bool>(x));
            return result;
        }

        private T GetGeneric<T>(string url, T result, Func<string, T> deserializar)
        {
            //bool result = false;
            using (var httpClientHandler = new HttpClientHandler())
            {
                ////Para aceptar el certificado de pruebas
                httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };

                using (HttpClient client = new HttpClient())
                {
                    // Cabecera con token de autorización
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "eyJhbGciOiJSUzI1NiIsImtpZCI6IjQ1NDA2M0FBMkVCREVGQzdBNTNGRDM4MDE0REYwMTFFMjkxQTAyNEYiLCJ0eXAiOiJhdCtqd3QiLCJ4NXQiOiJSVUJqcWk2OTc4ZWxQOU9BRk44Qkhpa2FBazgifQ.eyJuYmYiOjE2ODUzOTMwMTgsImV4cCI6MTY4NzEyMTAxOCwiaXNzIjoiaHR0cHM6Ly9wcnVlYmFzLnVuYWNoLmVkdS5lYzo0NDMwIiwiYXVkIjoiQWNhZGVtaWNvQVBJIiwiY2xpZW50X2lkIjoiMDBjODJmNjctNmFkYy00ZTJiLTllODItMWE5M2MzMWE2ZjM2IiwiY2xpZW50ZSI6IkJvbHNhIGRlIEVtcGxlbyIsImFtYmllbnRlIjoiRGVzYXJyb2xsbyIsImp0aSI6IjBzZVkwWG54V2pKS1otMFhTOGJPenciLCJzY29wZSI6WyJhY2FkZW1pY28uYXBpLmxlY3R1cmEiXX0.Nb0eYr4twydZC6ULgXRqpO3VQ7yx-41FcRgs3BDTVfWwO7oZDV_1-Xtd5HmHoR93HC4KFHPVY9i3n8a_Dg9DxUdcpcEmpGdWPjFdoPHVlCQy5JgD6Nqxl_NNprUG8lE2kR68W4H-CqMY-o0B6M7rYuiPBYsuSHpkl99SfpI13AmEl9Gx9ssPPeHP9ziaR9njBqroet5yYnliIkIC7p2piuN3IE_-GAbtujzum7tq8iwGC-lDyoSDzU-jQFqOvr8W3L85sjt989pi5NYSbYFodpaizGBCycjElxJ4iyn1rX4WjILHtnjkJWzMpwRMb4HIL48h5heYm8wtgi1mgMr2mq7g-FTXGssHoPqcsxT5ubbHwE8B48I-6GcVjG9uoKWT0EUrlYt5QDhWL_0BcAJbkNXpJxgiqcnO36mCJK6-9mIJTtJ88cl11EgIy_tie5ueR9nYP86uvkSebBqXzEOenGKwox3TtO2QdmzIpyq_t58719gVVYGZMijtrT4VhDetnJjxSh-x-cHZQ7M-WtEdbvVTBJhWPw6zYxFexV3ETRpKqw-oHMR8kaYgk67aI95m7wyzYOnbNxmBhQ3_HK6Dq-QB3PXW5wn1dd-LZ6w-_ilxj9x3HUfi-2oJEDzXlHakmXLHUguaenkulaChMRfBNoFceuX_fNsEUKNP0VhpNyc");

                    var response = client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result;
                    Error = string.Empty;
                    switch (response.StatusCode)
                    {
                        case HttpStatusCode.OK:
                            {
                                var data = response.Content.ReadAsStringAsync().Result;
                                result = deserializar(data);
                            }
                            break;
                        case HttpStatusCode.Unauthorized:
                            Error = "No se tiene acceso a la Api (No autorizado)";
                            break;
                        case HttpStatusCode.Forbidden:
                            Error = "No se tiene acceso al servicio de validación (No autorizado al método)";
                            break;
                        case HttpStatusCode.InternalServerError:
                            Error = "Error 500 al consultar la Api";
                            break;
                        case HttpStatusCode.NotFound:
                            {
                                var data = response.Content.ReadAsStringAsync().Result;
                                var t = JsonConvert.DeserializeObject<string>(data);
                                Error = "No es un documento válido";
                            }
                            break;
                    }
                }
            }
            return result;
        }

    }
}
