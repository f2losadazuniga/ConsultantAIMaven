using ConnectionManagement.Data;
using EntregasLogyTechSharedModel.Dynamic;
using EntregasLogyTechSharedModel.FineTune;
using LogicaNegocioServicio.Comunes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LogicaNegocioServicio.Dynamic
{
    public class DynamicDal
    {
        public string ConectionString { get; set; }
        public DynamicDal(string conectionString)
        {
            ConectionString = conectionString;
        }
        public async Task<ResponseDynamics> GetToken(HttpClient _httpClient)
        {
            ResponseDynamics resultado = new ResponseDynamics();
         
            try
            {
                string TokenUrl = ConfigValues.seleccionarConfigValue("URLApiDynamicGettoken", ConectionString);
                ResponseDynamics result = new ResponseDynamics();
                string Dynamic_Grant_type = ConfigValues.seleccionarConfigValue("Dynamic_Grant_type", ConectionString);
                string Dynamic_Client_id = ConfigValues.seleccionarConfigValue("Dynamic_Client_id", ConectionString);
                string Dynamic_Client_secret = ConfigValues.seleccionarConfigValue("Dynamic_Client_secret", ConectionString);
                string Dynamic_Resource = ConfigValues.seleccionarConfigValue("Dynamic_Resource", ConectionString);

                var formData = new FormUrlEncodedContent(new[]
                {
                new KeyValuePair<string, string>("grant_type", Dynamic_Grant_type),
                new KeyValuePair<string, string>("client_id", Dynamic_Client_id),
                new KeyValuePair<string, string>("client_secret", Dynamic_Client_secret),
                        new KeyValuePair<string, string>("resource", Dynamic_Resource)
                    });

                var response = await _httpClient.PostAsync(TokenUrl, formData);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    // Deserialize the JSON response into ResponseDynamics
                    resultado = JsonConvert.DeserializeObject<ResponseDynamics>(content);
                    return resultado;
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Se generó un error al generar el token : " + ex.Message);
            }


            return resultado;
        }

    }
}
