using EntregasLogyTechSharedModel.Dynamic;
using LogicaNegocioServicio.Comunes;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ApiConsultantAIMaven.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class DynamicsController : ControllerBase
    {
        private readonly IConfiguration _ConnectionString;
        public string apiKey = string.Empty;
        private readonly HttpClient _httpClient;
        public DynamicsController(IConfiguration Configuration, HttpClient httpClient)
        {
            this._ConnectionString = Configuration;
            apiKey = ConfigValues.seleccionarConfigValue("ApikeyChatGPT", _ConnectionString.GetConnectionString("DefaultConnection"));
            _httpClient = httpClient;

        }
        [HttpGet("GetToken")]
        [AllowAnonymous]
        public async Task<ActionResult<ResponseDynamics>> Gettoken()
        {
            try
            {

                string TokenUrl = ConfigValues.seleccionarConfigValue("URLDynamicGettoken", _ConnectionString.GetConnectionString("DefaultConnection"));
                ResponseDynamics result = new ResponseDynamics();
                string Dynamic_Grant_type = ConfigValues.seleccionarConfigValue("Dynamic_Grant_type", _ConnectionString.GetConnectionString("DefaultConnection"));
                string Dynamic_Client_id = ConfigValues.seleccionarConfigValue("Dynamic_Client_id", _ConnectionString.GetConnectionString("DefaultConnection"));
                string Dynamic_Client_secret = ConfigValues.seleccionarConfigValue("Dynamic_Client_secret", _ConnectionString.GetConnectionString("DefaultConnection"));
                string Dynamic_Resource = ConfigValues.seleccionarConfigValue("Dynamic_Resource", _ConnectionString.GetConnectionString("DefaultConnection"));
                try
                {
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
                        var responseDynamics = JsonConvert.DeserializeObject<ResponseDynamics>(content);
                        return responseDynamics;
                    }
                    {
                        // Handle the error response here
                        var emex = new ErrorDetails()
                        {
                            StatusCode = 400,
                            Message = "Error:  " + "The Token could not be generated"
                        };
                        return BadRequest(new JsonResult(emex));
                    }
                }
                catch (Exception ex)
                {
                    var emex = new ErrorDetails()
                    {
                        StatusCode = 400,
                        Message = "Error:  " + ex.Message.ToString()
                    };
                    return BadRequest(new JsonResult(emex));
                }

            }
            catch (Exception ex)
            {
                var emex = new ErrorDetails()
                {
                    StatusCode = 400,
                    Message = "Error:  " + ex.Message.ToString()
                };
                return BadRequest(new JsonResult(emex));
            }

        }



    }
}
