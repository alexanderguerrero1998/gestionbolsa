using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Unach.DA.Empleo.Presistencia.Api
{
    public class ApiInformacionBasicaPorCriterio
    {



        // Recuerda que el token tiene vigencia de 24 hora, importante actualizarlo
        // Ingresa aqui para generar el token: https://manage.auth0.com/dashboard/us/dev-bb4zah0ivws1wedd/apis/management/explorer
        // Aqui para hacer el SETOKEN: https://auth0.com/docs/api/management/v2
        // Ingresa aqui para generar la data del token: https://auth0.com/docs/api/management/v2#!/Client_Grants/get_client_grants
        // Aqui revisas si vale el token:https://jwt.io/


        private string token = "eyJhbGciOiJSUzI1NiIsImtpZCI6IjQ1NDA2M0FBMkVCREVGQzdBNTNGRDM4MDE0REYwMTFFMjkxQTAyNEYiLCJ0eXAiOiJhdCtqd3QiLCJ4NXQiOiJSVUJqcWk2OTc4ZWxQOU9BRk44Qkhpa2FBazgifQ.eyJuYmYiOjE2OTMyMzEwNDUsImV4cCI6MTY5NTgyMzA0NSwiaXNzIjoiaHR0cHM6Ly9wcnVlYmFzLnVuYWNoLmVkdS5lYzo0NDMwIiwiYXVkIjoiQWNhZGVtaWNvQVBJIiwiY2xpZW50X2lkIjoiMDBjODJmNjctNmFkYy00ZTJiLTllODItMWE5M2MzMWE2ZjM2IiwiY2xpZW50ZSI6IkJvbHNhIGRlIEVtcGxlbyIsImFtYmllbnRlIjoiRGVzYXJyb2xsbyIsImp0aSI6IkNVSEFSei1QR2dGRWlPaGRmYk5hb1EiLCJzY29wZSI6WyJhY2FkZW1pY28uYXBpLmxlY3R1cmEiXX0.R6UL9713ZoMo8Ytbi_MZy8dm1_imrwmrYomyAPK3mYShUdqomjFACfM8LApyHFvwnhf9-nx5ajEto8NQY-o6eHAe6bokMUnJ_s99l8xRoHkS17f4mTwL8m5N4Qj-9fQNf4VFEpJW7VyLFexf1ZWIVBrote-jHgqu242d0rL0soEVuq554Y9JbpDE0i3J7GPF5Lx5ZO77DCxxL-kZrK6kqZf14-8uwClLqqSZPb0BkVAl0cNDDnzURbL9XQJoPIhMmfv03TnTt36GoToZQZmr7CdNoTZ1zayAhvXYOG_3KdvLLdwJckx-G583SoxReVQWQJgGWmFy71a6LjuXulR7YMeoSUGIRjJmn_gyKf0GiUA72QYUmmmhoyLGaAyMv1xEQPUXhR1tS5NLFTg3aRlobp-UDQufRzuuWMH-UQsfa6y9fDwdWXu3-LxA3rpeZr_mMPzl9C45fBZvgIZO9sbnHu2l49fpQme1PcwvV0CeIe6Ovpfox76Wa5m3dc_4iGtl_-qeRu1MT-z9iDatz7bnx0fJxs6jm-COFGes8JHgI0w1WsWyNN0XCU8KHaZ9FEJLV9QDTNCYdFIZIWOSluxlwSVLY_9_JnqGeiREkWfI-p_VpTbxL4FeNn8dRp_rFC4odgYrwiWVayIbfdxxPTJwaJMdiygz1Fh7snIwBLylhLU";
        public string Error { get; set; }

        public ApiInformacionBasicaPorCriterio(string token)
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
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "eyJhbGciOiJSUzI1NiIsImtpZCI6IjQ1NDA2M0FBMkVCREVGQzdBNTNGRDM4MDE0REYwMTFFMjkxQTAyNEYiLCJ0eXAiOiJhdCtqd3QiLCJ4NXQiOiJSVUJqcWk2OTc4ZWxQOU9BRk44Qkhpa2FBazgifQ.eyJuYmYiOjE2OTMyMzEwNDUsImV4cCI6MTY5NTgyMzA0NSwiaXNzIjoiaHR0cHM6Ly9wcnVlYmFzLnVuYWNoLmVkdS5lYzo0NDMwIiwiYXVkIjoiQWNhZGVtaWNvQVBJIiwiY2xpZW50X2lkIjoiMDBjODJmNjctNmFkYy00ZTJiLTllODItMWE5M2MzMWE2ZjM2IiwiY2xpZW50ZSI6IkJvbHNhIGRlIEVtcGxlbyIsImFtYmllbnRlIjoiRGVzYXJyb2xsbyIsImp0aSI6IkNVSEFSei1QR2dGRWlPaGRmYk5hb1EiLCJzY29wZSI6WyJhY2FkZW1pY28uYXBpLmxlY3R1cmEiXX0.R6UL9713ZoMo8Ytbi_MZy8dm1_imrwmrYomyAPK3mYShUdqomjFACfM8LApyHFvwnhf9-nx5ajEto8NQY-o6eHAe6bokMUnJ_s99l8xRoHkS17f4mTwL8m5N4Qj-9fQNf4VFEpJW7VyLFexf1ZWIVBrote-jHgqu242d0rL0soEVuq554Y9JbpDE0i3J7GPF5Lx5ZO77DCxxL-kZrK6kqZf14-8uwClLqqSZPb0BkVAl0cNDDnzURbL9XQJoPIhMmfv03TnTt36GoToZQZmr7CdNoTZ1zayAhvXYOG_3KdvLLdwJckx-G583SoxReVQWQJgGWmFy71a6LjuXulR7YMeoSUGIRjJmn_gyKf0GiUA72QYUmmmhoyLGaAyMv1xEQPUXhR1tS5NLFTg3aRlobp-UDQufRzuuWMH-UQsfa6y9fDwdWXu3-LxA3rpeZr_mMPzl9C45fBZvgIZO9sbnHu2l49fpQme1PcwvV0CeIe6Ovpfox76Wa5m3dc_4iGtl_-qeRu1MT-z9iDatz7bnx0fJxs6jm-COFGes8JHgI0w1WsWyNN0XCU8KHaZ9FEJLV9QDTNCYdFIZIWOSluxlwSVLY_9_JnqGeiREkWfI-p_VpTbxL4FeNn8dRp_rFC4odgYrwiWVayIbfdxxPTJwaJMdiygz1Fh7snIwBLylhLU");

                    var response = client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result;
                    Error = string.Empty;
                    switch (response.StatusCode)
                    {
                        case HttpStatusCode.OK:
                            {
                                var data = response.Content.ReadAsStringAsync().Result;

                                // Convertimos un vector de objetos JSON a un objeto JSON 
                                var otra = ConvertArrayToObject(data);
                                result = deserializar(otra);
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

        public string ConvertArrayToObject(string jsonArray)
        {
            // Deserializar el vector de objetos JSON en una lista de objetos
            List<object> objectList = JsonConvert.DeserializeObject<List<object>>(jsonArray);

            // Crear un nuevo objeto JObject para combinar los objetos
            JObject combinedObject = new JObject();

            // Combinar los objetos en un solo objeto
            foreach (var obj in objectList)
            {
                var properties = JObject.FromObject(obj);
                combinedObject.Merge(properties, new JsonMergeSettings
                {
                    MergeArrayHandling = MergeArrayHandling.Concat
                });
            }

            // Serializar el objeto combinado en formato JSON
            string result = combinedObject.ToString();

            return result;
        }
    }
}
