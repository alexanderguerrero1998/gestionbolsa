using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Unach.DA.Empleo.Presistencia.Api
{
    public class ApiInfoAcademico
    {



        // Recuerda que el token tiene vigencia de 24 hora, importante actualizarlo
        // Ingresa aqui para generar el token: https://manage.auth0.com/dashboard/us/dev-bb4zah0ivws1wedd/apis/management/explorer
        // Aqui para hacer el SETOKEN: https://auth0.com/docs/api/management/v2
        // Ingresa aqui para generar la data del token: https://auth0.com/docs/api/management/v2#!/Client_Grants/get_client_grants
        // Aqui revisas si vale el token:https://jwt.io/


        private string token = "eyJhbGciOiJSUzI1NiIsImtpZCI6IjQ1NDA2M0FBMkVCREVGQzdBNTNGRDM4MDE0REYwMTFFMjkxQTAyNEYiLCJ0eXAiOiJhdCtqd3QiLCJ4NXQiOiJSVUJqcWk2OTc4ZWxQOU9BRk44Qkhpa2FBazgifQ.eyJuYmYiOjE2OTA5OTI3MjQsImV4cCI6MTY5MzU4NDcyNCwiaXNzIjoiaHR0cHM6Ly9wcnVlYmFzLnVuYWNoLmVkdS5lYzo0NDMwIiwiYXVkIjoiQWNhZGVtaWNvQVBJIiwiY2xpZW50X2lkIjoiNzZiYzgyMjEtZDA4ZC00ZjIwLTgwMDItM2I0YWNjN2FmYjVhIiwiY2xpZW50ZSI6IlNpc3RlbWEgZGUgVGl0dWxhY2nDs24iLCJhbWJpZW50ZSI6IkRlc2Fycm9sbG8iLCJqdGkiOiJOUS01V3JVRC1jdGc2VTlWcU85b2hBIiwic2NvcGUiOlsiYWNhZGVtaWNvLmFwaS5sZWN0dXJhIl19.t5SNn9qRpz0CVJuppggUQ6NTGfbus6yTQZGY2s4Ng9uFtkcYF-Itz9Ta-rwq-XDZbyihDCt0S7ewSOzD9jWAGtjc85aSrg5E-YVnFdXw2JZthIGAcXRCbZSUDjiQ73ZZrHgKoW10fOb-Gxx9UIGnK02moX43kpJvjKdjQBJPy--jcSjtAG4pu50OO-6cP11fKomrVJ98h1qqJ9JPnq0udPiRTwt1iLnPfTmPnXI4ZTVbZovRWm4Xag93xDVYqpi30JeHprxIzM36RPJeHiMhdw9DmT_KaQ1_E5YqFm1ukRh7BYMoA2rKMpiQGrbmb7vW1X91kzL3IygynBbNKtnfsG3jlpnG7SWWk7S_em3oEplh9-pxPSf6l6VKc-ww78GeanrP-JDwiJ9-4mh23hL0Z1OXfabCxjG_KeddVc0CPENC0uHSMavLJPlVdTIipfLZBkoZuHXfhLpPXxFRmkslIxgBA3d2cjiQxRwyLEIEaXI-FocLhrNl2LUMOGwwdP118t-9LLG5k2reuNQ2cC6I8QxxIeIxAoJIiWmMwAGYEDbHPpRAQ1Zfj3FCzl9aSjoV4fQNJhQwTnl84JwXqxzv3o4g6tdqvl5sgl7BRw5ufh796UQt1OJBuZh_Xnh1yz7HdHMSPsyu48A-kjgklA4Z6hmO905sQObTFxp9DWUfjGw";
        public string Error { get; set; }

        public ApiInfoAcademico(string token)
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
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "eyJhbGciOiJSUzI1NiIsImtpZCI6IjQ1NDA2M0FBMkVCREVGQzdBNTNGRDM4MDE0REYwMTFFMjkxQTAyNEYiLCJ0eXAiOiJhdCtqd3QiLCJ4NXQiOiJSVUJqcWk2OTc4ZWxQOU9BRk44Qkhpa2FBazgifQ.eyJuYmYiOjE2OTA5OTI3MjQsImV4cCI6MTY5MzU4NDcyNCwiaXNzIjoiaHR0cHM6Ly9wcnVlYmFzLnVuYWNoLmVkdS5lYzo0NDMwIiwiYXVkIjoiQWNhZGVtaWNvQVBJIiwiY2xpZW50X2lkIjoiNzZiYzgyMjEtZDA4ZC00ZjIwLTgwMDItM2I0YWNjN2FmYjVhIiwiY2xpZW50ZSI6IlNpc3RlbWEgZGUgVGl0dWxhY2nDs24iLCJhbWJpZW50ZSI6IkRlc2Fycm9sbG8iLCJqdGkiOiJOUS01V3JVRC1jdGc2VTlWcU85b2hBIiwic2NvcGUiOlsiYWNhZGVtaWNvLmFwaS5sZWN0dXJhIl19.t5SNn9qRpz0CVJuppggUQ6NTGfbus6yTQZGY2s4Ng9uFtkcYF-Itz9Ta-rwq-XDZbyihDCt0S7ewSOzD9jWAGtjc85aSrg5E-YVnFdXw2JZthIGAcXRCbZSUDjiQ73ZZrHgKoW10fOb-Gxx9UIGnK02moX43kpJvjKdjQBJPy--jcSjtAG4pu50OO-6cP11fKomrVJ98h1qqJ9JPnq0udPiRTwt1iLnPfTmPnXI4ZTVbZovRWm4Xag93xDVYqpi30JeHprxIzM36RPJeHiMhdw9DmT_KaQ1_E5YqFm1ukRh7BYMoA2rKMpiQGrbmb7vW1X91kzL3IygynBbNKtnfsG3jlpnG7SWWk7S_em3oEplh9-pxPSf6l6VKc-ww78GeanrP-JDwiJ9-4mh23hL0Z1OXfabCxjG_KeddVc0CPENC0uHSMavLJPlVdTIipfLZBkoZuHXfhLpPXxFRmkslIxgBA3d2cjiQxRwyLEIEaXI-FocLhrNl2LUMOGwwdP118t-9LLG5k2reuNQ2cC6I8QxxIeIxAoJIiWmMwAGYEDbHPpRAQ1Zfj3FCzl9aSjoV4fQNJhQwTnl84JwXqxzv3o4g6tdqvl5sgl7BRw5ufh796UQt1OJBuZh_Xnh1yz7HdHMSPsyu48A-kjgklA4Z6hmO905sQObTFxp9DWUfjGw");

                    try { 
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
                    }catch (Exception ex) { Error = "Error al obtener la respuesta de la ApiInfoAcademico: " + ex.Message; }
                }
            }
            return result;
        }

       

    }
}
