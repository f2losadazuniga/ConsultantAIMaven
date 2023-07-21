using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using LogicaNegocioServicio.Models;

namespace LogicaNegocioServicio.Comunes
{
    public class CommonTools
    {

        public static async Task<ApiRestRespuesta> InvocarServicioRest(string metodo, string urlBase, string api, object objeto = null, AuthenticationHeaderValue autorizacion = null)
        {
            var res = new ApiRestRespuesta();

            try
            {
                if (urlBase.Substring(urlBase.Length - 1) != "/") { urlBase += "/"; }

                HttpClient client = new HttpClient
                {
                    BaseAddress = new Uri(urlBase)
                };
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                if (autorizacion != null) { client.DefaultRequestHeaders.Authorization = autorizacion; }

                string contenido = "";

                if (objeto != null) { contenido = JsonConvert.SerializeObject(objeto, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore }); }

                var buffer = System.Text.Encoding.UTF8.GetBytes(contenido);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                HttpResponseMessage response = null;

                switch (metodo)
                {
                    case "POST":
                        response = await client.PostAsync(api, byteContent);
                        break;
                    case "PUT":
                        response = await client.PutAsync(api, byteContent);
                        break;
                    case "GET":
                        response = await client.GetAsync(api);
                        break;
                }

                if (response.Content != null) { res.Datos = await response.Content.ReadAsStringAsync(); }

                if (response.IsSuccessStatusCode)
                {
                    res.Exitoso = true;
                    res.Mensaje = "Servicio invocado exitosamente.";
                }
                else
                {
                    res.Exitoso = false;
                    res.Mensaje = "Error al invocar servicio. Codigo Estado: " + response.StatusCode.ToString() + " Descripción: " + response.ReasonPhrase;
                }
            }
            catch (Exception e)
            {
                res = new ApiRestRespuesta { Exitoso = false, Mensaje = "Error al invocar servicio. " + e.Message };
            }

            return res;
        }

        public static async Task<MasivApiResponse> EnviarSMSMasiv(string userToken, string apiToken, string apiPath, SmsRequestInfo mensaje) {

            var respuesta = new MasivApiResponse("404","Proceso no ejecutado");

            try
            {
                var credentials = userToken + ":" + apiToken;
                var basicToken = Base64Encode(credentials);
                var auth = new AuthenticationHeaderValue("Basic", basicToken);
                var resultado = await InvocarServicioRest("POST", "https://api-sms.masivapp.com/smsv3/sms/", "messages", mensaje, auth);
                if (resultado != null) {
                    if (resultado.Exitoso)
                    {

                        if (resultado.Datos != null)
                        {
                             respuesta = new MasivApiResponse("200", resultado.Mensaje);
                            //respuesta = JsonConvert.DeserializeObject<dynamic>(resultado.Datos.ToString());
                        }
                        else {
                            respuesta = new MasivApiResponse("500", "Error al tratar de enviar SMS. No se recibieron los datos de la respuesta");
                        }
                    }
                    else {
                        respuesta = new MasivApiResponse("500", "Error al tratar de enviar SMS. " + resultado.Mensaje);
                    }
                }
                else {
                    respuesta = new MasivApiResponse("500", "Error al tratar de enviar SMS. No se recibió respuesta del servicio");
                }
            }
            catch (Exception ex) {
                respuesta = new MasivApiResponse("500", "Error al tratar de enviar SMS. " + ex.Message);
            }

            return respuesta;
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
         public static string ObtenerValorConfiguracion(string key)
        {
            string value = "";

            var builder = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json");

            var conf = builder.Build();

            value = conf[key];

            return value ?? "";
        }
    }
}
