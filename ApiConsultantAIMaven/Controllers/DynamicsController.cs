using EntregasLogyTechSharedModel.Dynamic;
using LogicaNegocioServicio.Comunes;
using LogicaNegocioServicio.Dynamic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OpenAI_API.Moderation;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
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
                ResponseDynamics resultado = new ResponseDynamics();
                DynamicDal dynami = new DynamicDal(_ConnectionString.GetConnectionString("DefaultConnection"));
                resultado = await dynami.GetToken(_httpClient);
                if (String.IsNullOrEmpty(resultado.access_token))
                {
                    // Handle the error response here
                    var emex = new ErrorDetails()
                    {
                        StatusCode = 400,
                        Message = "Error:  " + "The Token could not be generated"
                    };
                    return BadRequest(new JsonResult(emex));
                }
                return resultado;

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
        [HttpPost("SaveLead")]
        [AllowAnonymous]
        public async Task<ActionResult<bool>> SaveLead(LeadDynamic pet)
        {
            ResponseDynamics resultado = new ResponseDynamics();
            try
            {
                string apiUrl = ConfigValues.seleccionarConfigValue("URLApiSaveLead", _ConnectionString.GetConnectionString("DefaultConnection"));
                DynamicDal dynami = new DynamicDal(_ConnectionString.GetConnectionString("DefaultConnection"));
                resultado = await dynami.GetToken(_httpClient);
                if (String.IsNullOrEmpty(resultado.access_token))
                {
                    // Handle the error response here
                    var emex = new ErrorDetails()
                    {
                        StatusCode = 400,
                        Message = "Error:  " + "The Token could not be generated"
                    };
                    return BadRequest(new JsonResult(emex));
                }
                string bearerToken = resultado.access_token;
                using (var httpClient = new HttpClient())
                {
                    // Configurar el encabezado de autorización Bearer
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

                    // Serializar el objeto Contact a JSON
                    string jsonContent = JsonConvert.SerializeObject(pet);

                    // Configurar el contenido de la solicitud POST
                    var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                    // Realizar la solicitud POST al servicio
                    HttpResponseMessage response = await httpClient.PostAsync(apiUrl, content);

                    // Verificar si la solicitud fue exitosa (código de estado HTTP 200 OK)
                    if (response.IsSuccessStatusCode)
                    {
                        // La solicitud se completó con éxito
                        return true;
                    }
                    else
                    {
                        // La solicitud falló
                        Console.WriteLine($"Error: {response.StatusCode}");
                        var emex = new ErrorDetails()
                        {
                            StatusCode = 400,
                            Message = "Error:  " + response.StatusCode
                        };
                        return BadRequest(new JsonResult(emex));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                var emex = new ErrorDetails()
                {
                    StatusCode = 400,
                    Message = "Error:  " + ex.Message.ToString()
                };
                return BadRequest(new JsonResult(emex));
            }
        }

        [HttpGet("GetLead")]
        [AllowAnonymous]
        public async Task<ActionResult<bool>> GetLead(string emailaddress)
        {
            ResponseDynamics resultado = new ResponseDynamics();
            try
            {
                string apiUrl = ConfigValues.seleccionarConfigValue("URLApiGetLead", _ConnectionString.GetConnectionString("DefaultConnection"));
                DynamicDal dynami = new DynamicDal(_ConnectionString.GetConnectionString("DefaultConnection"));
                resultado = await dynami.GetToken(_httpClient);
                if (String.IsNullOrEmpty(resultado.access_token))
                {
                    // Handle the error response here
                    var emex = new ErrorDetails()
                    {
                        StatusCode = 400,
                        Message = "Error:  " + "The Token could not be generated"
                    };
                    return BadRequest(new JsonResult(emex));
                }
                string bearerToken = resultado.access_token;
                using (var httpClient = new HttpClient())
                {
                    // Realizar la solicitud GET
                    HttpResponseMessage response = await _httpClient.GetAsync(apiUrl + emailaddress);
                    if (response.IsSuccessStatusCode)
                    {
                        string content = await response.Content.ReadAsStringAsync();
                        return BadRequest(new JsonResult(content));
                    }
                    else
                    {
                        // La solicitud falló
                        Console.WriteLine($"Error: {response.StatusCode}");
                        var emex = new ErrorDetails()
                        {
                            StatusCode = 400,
                            Message = "Error:  " + response.StatusCode
                        };
                        return BadRequest(new JsonResult(emex));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                var emex = new ErrorDetails()
                {
                    StatusCode = 400,
                    Message = "Error:  " + ex.Message.ToString()
                };
                return BadRequest(new JsonResult(emex));
            }
        }
        [HttpPost("SaveOpportunities")]
        [AllowAnonymous]
        public async Task<ActionResult<bool>> SaveOpportunities(DynamicOpportunities pet)
        {
            ResponseDynamics resultado = new ResponseDynamics();
            try
            {
                string apiUrl = ConfigValues.seleccionarConfigValue("URLApiSaveOpportunities", _ConnectionString.GetConnectionString("DefaultConnection"));
                DynamicDal dynami = new DynamicDal(_ConnectionString.GetConnectionString("DefaultConnection"));
                resultado = await dynami.GetToken(_httpClient);
                if (String.IsNullOrEmpty(resultado.access_token))
                {
                    // Handle the error response here
                    var emex = new ErrorDetails()
                    {
                        StatusCode = 400,
                        Message = "Error:  " + "The Token could not be generated"
                    };
                    return BadRequest(new JsonResult(emex));
                }
                string bearerToken = resultado.access_token;
                using (var httpClient = new HttpClient())
                {
                    // Configurar el encabezado de autorización Bearer
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

                    // Serializar el objeto Contact a JSON
                    string jsonContent = JsonConvert.SerializeObject(pet);

                    // Configurar el contenido de la solicitud POST
                    var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                    // Realizar la solicitud POST al servicio
                    HttpResponseMessage response = await httpClient.PostAsync(apiUrl, content);

                    // Verificar si la solicitud fue exitosa (código de estado HTTP 200 OK)
                    if (response.IsSuccessStatusCode)
                    {
                        // La solicitud se completó con éxito
                        return true;
                    }
                    else
                    {
                        // La solicitud falló
                        Console.WriteLine($"Error: {response.StatusCode}");
                        var emex = new ErrorDetails()
                        {
                            StatusCode = 400,
                            Message = "Error:  " + response.StatusCode
                        };
                        return BadRequest(new JsonResult(emex));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                var emex = new ErrorDetails()
                {
                    StatusCode = 400,
                    Message = "Error:  " + ex.Message.ToString()
                };
                return BadRequest(new JsonResult(emex));
            }
        }

        [HttpGet("GetOpportunities")]
        [AllowAnonymous]
        public async Task<ActionResult<bool>> GetOpportunities(string opportunityId)
        {
            ResponseDynamics resultado = new ResponseDynamics();
            try
            {
                string apiUrl = ConfigValues.seleccionarConfigValue("URLApiGetOpportunities", _ConnectionString.GetConnectionString("DefaultConnection"));
                string requestUrl = $"opportunities({opportunityId})?$select=customerid_account";
                DynamicDal dynami = new DynamicDal(_ConnectionString.GetConnectionString("DefaultConnection"));
                resultado = await dynami.GetToken(_httpClient);
                if (String.IsNullOrEmpty(resultado.access_token))
                {
                    // Handle the error response here
                    var emex = new ErrorDetails()
                    {
                        StatusCode = 400,
                        Message = "Error:  " + "The Token could not be generated"
                    };
                    return BadRequest(new JsonResult(emex));
                }
                string bearerToken = resultado.access_token;
                using (var httpClient = new HttpClient())
                {
                    // Realizar la solicitud GET
                    HttpResponseMessage response = await _httpClient.GetAsync(apiUrl + requestUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        string content = await response.Content.ReadAsStringAsync();
                        return BadRequest(new JsonResult(content));
                    }
                    else
                    {
                        // La solicitud falló
                        Console.WriteLine($"Error: {response.StatusCode}");
                        var emex = new ErrorDetails()
                        {
                            StatusCode = 400,
                            Message = "Error:  " + response.StatusCode
                        };
                        return BadRequest(new JsonResult(emex));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
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
