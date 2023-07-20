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


        private string token = "eyJhbGciOiJSUzI1NiIsImtpZCI6IjQ1NDA2M0FBMkVCREVGQzdBNTNGRDM4MDE0REYwMTFFMjkxQTAyNEYiLCJ0eXAiOiJhdCtqd3QiLCJ4NXQiOiJSVUJqcWk2OTc4ZWxQOU9BRk44Qkhpa2FBazgifQ.eyJuYmYiOjE2ODkxNzM3MjIsImV4cCI6MTY5MTc2NTcyMiwiaXNzIjoiaHR0cHM6Ly9wcnVlYmFzLnVuYWNoLmVkdS5lYzo0NDMwIiwiYXVkIjoiQWNhZGVtaWNvQVBJIiwiY2xpZW50X2lkIjoiMDBjODJmNjctNmFkYy00ZTJiLTllODItMWE5M2MzMWE2ZjM2IiwiY2xpZW50ZSI6IkJvbHNhIGRlIEVtcGxlbyIsImFtYmllbnRlIjoiRGVzYXJyb2xsbyIsImp0aSI6InNxQm9UMzZwVk5fZF9KTm9ReWJaTlEiLCJzY29wZSI6WyJhY2FkZW1pY28uYXBpLmxlY3R1cmEiXX0.rEPL5ZZfFaeN5ZTukdg0zfRQzJb11mnkQ3XiG7xLfKSUf5g9bxBKrCNBAPnKLgjr_boCiSosaaPKywhHwQoPxr_T6DQC8FV2xI_8pLUmh4hHsHgZYQ63H4mPrTsVem16OW_Rof6NHEXma3cngcoJ9gWN2LL6cwalXIdnI1s_3BquvzwW5pyr_A3PhVU4_CnbmiBgIq_iPFEqAZM6nwlpVeNlX8PBMoIIbAxwx5bKhv5u8LzWwj0nRuHjHQv7Z5s8YbA23L7J5OgM5MSGSCMnLRnG61VuxYGds9dJSOFp1EcRiyLNA-WuXZkqJkIV82cogndwV-SfcLainrpVP0vv9lvlwLn95m5tsKt3qzw4isKVmlq907DhtSucQcTLiBMzcamQ9HLGQWFt03oNao6wfPZnWws30LTTIySo3gsMEXuNwQTmw_K8U4AIYF0fQo-2KxjJoNIhYyzylkP13BBOMGBsWJd_UaG83GVCcgS3FB2VBHCAv9qgTzyYdhUvYC-TlRudRg6fPtZTSr_Y52r0GAp333M_nDyajFyxUs05f6mxOVONNGN85QQhKaKjiys9EuWUuk4hvrg6kFjNRBne0lNeOp_5BFTymLoUEzBxkzQNae-AYXdJJZCihHeFa50W-an9oYPEeZDv7Ls44zr1KSJ1vT4q0C7eADWoKtD9bws";
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
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "eyJhbGciOiJSUzI1NiIsImtpZCI6IjQ1NDA2M0FBMkVCREVGQzdBNTNGRDM4MDE0REYwMTFFMjkxQTAyNEYiLCJ0eXAiOiJhdCtqd3QiLCJ4NXQiOiJSVUJqcWk2OTc4ZWxQOU9BRk44Qkhpa2FBazgifQ.eyJuYmYiOjE2ODkxNzM3MjIsImV4cCI6MTY5MTc2NTcyMiwiaXNzIjoiaHR0cHM6Ly9wcnVlYmFzLnVuYWNoLmVkdS5lYzo0NDMwIiwiYXVkIjoiQWNhZGVtaWNvQVBJIiwiY2xpZW50X2lkIjoiMDBjODJmNjctNmFkYy00ZTJiLTllODItMWE5M2MzMWE2ZjM2IiwiY2xpZW50ZSI6IkJvbHNhIGRlIEVtcGxlbyIsImFtYmllbnRlIjoiRGVzYXJyb2xsbyIsImp0aSI6InNxQm9UMzZwVk5fZF9KTm9ReWJaTlEiLCJzY29wZSI6WyJhY2FkZW1pY28uYXBpLmxlY3R1cmEiXX0.rEPL5ZZfFaeN5ZTukdg0zfRQzJb11mnkQ3XiG7xLfKSUf5g9bxBKrCNBAPnKLgjr_boCiSosaaPKywhHwQoPxr_T6DQC8FV2xI_8pLUmh4hHsHgZYQ63H4mPrTsVem16OW_Rof6NHEXma3cngcoJ9gWN2LL6cwalXIdnI1s_3BquvzwW5pyr_A3PhVU4_CnbmiBgIq_iPFEqAZM6nwlpVeNlX8PBMoIIbAxwx5bKhv5u8LzWwj0nRuHjHQv7Z5s8YbA23L7J5OgM5MSGSCMnLRnG61VuxYGds9dJSOFp1EcRiyLNA-WuXZkqJkIV82cogndwV-SfcLainrpVP0vv9lvlwLn95m5tsKt3qzw4isKVmlq907DhtSucQcTLiBMzcamQ9HLGQWFt03oNao6wfPZnWws30LTTIySo3gsMEXuNwQTmw_K8U4AIYF0fQo-2KxjJoNIhYyzylkP13BBOMGBsWJd_UaG83GVCcgS3FB2VBHCAv9qgTzyYdhUvYC-TlRudRg6fPtZTSr_Y52r0GAp333M_nDyajFyxUs05f6mxOVONNGN85QQhKaKjiys9EuWUuk4hvrg6kFjNRBne0lNeOp_5BFTymLoUEzBxkzQNae-AYXdJJZCihHeFa50W-an9oYPEeZDv7Ls44zr1KSJ1vT4q0C7eADWoKtD9bws");

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
