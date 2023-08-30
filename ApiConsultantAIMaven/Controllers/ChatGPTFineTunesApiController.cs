using LogicaNegocioServicio.Comunes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using OpenAI_API.Completions;
using OpenAI_API;
using System.Threading.Tasks;
using System;
using ConsultantAIMavenSharedModel.Usuarios;
using LogicaNegocioServicio.Usuarios;
using System.Collections.Generic;
using EntregasLogyTechSharedModel.FineTune;
using LogicaNegocioServicio.FineTunes;
using ConsultantAIMavenSharedModel.Comunes;
using OpenAI_API.Files;
using Microsoft.OpenApi.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using Azure;
using Newtonsoft.Json;
using System.IO;
using static EntregasLogyTechSharedModel.FineTune.FineTuneResponseModel;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ApiConsultantAIMaven.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatGPTFineTunesApiController : ControllerBase
    {
        private readonly IConfiguration _ConnectionString;
        public string apiKey = string.Empty;

        public ChatGPTFineTunesApiController(IConfiguration Configuration)
        {
            this._ConnectionString = Configuration;
            apiKey = ConfigValues.seleccionarConfigValue("ApikeyChatGPT", _ConnectionString.GetConnectionString("DefaultConnection"));
        }

        [HttpPost("DeleteFineTuneApi")]
        [AllowAnonymous]
        public async Task<ActionResult<RespuestaServicio>> DeleteFineTuneApi()
        {
            string Model = string.Empty;
            RespuestaServicio respuestaServicio = new RespuestaServicio();
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                Root resultado = new Root();
                FineTunesDal ftd = new FineTunesDal(_ConnectionString.GetConnectionString("DefaultConnection"));
                resultado = await ftd.GetTrainingLog();

                using (var httpClient = new HttpClient())
                {
                    using (var request = new HttpRequestMessage(new HttpMethod("GET"), "https://api.openai.com/v1/fine-tunes/" + resultado.id))
                    {
                        request.Headers.TryAddWithoutValidation("Authorization", "Bearer " + apiKey);

                        var response = await httpClient.SendAsync(request);
                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            var respuesta = response.Content.ReadAsStringAsync();
                            var data = JsonConvert.DeserializeObject<Root>(respuesta.Result.ToString());

                            if (data.fine_tuned_model == null)
                            {
                                respuestaServicio.Mensaje = "El modelo actual no se encuentra ";
                                return respuestaServicio;
                            }
                            else
                            {
                                Model = data.fine_tuned_model.ToString();
                            }
                        }
                    }
                }

                using (var httpClient = new HttpClient())
                {
                    using (var request = new HttpRequestMessage(new HttpMethod("DELETE"), "https://api.openai.com/v1/models/" + Model))
                    {
                        request.Headers.TryAddWithoutValidation("Authorization", "Bearer " + apiKey);

                        var response = await httpClient.SendAsync(request);
                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            var respuesta = response.Content.ReadAsStringAsync();
                        }
                    }
                }
                return Ok(resultado);

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

        [HttpPost("DeleteSpecifiedFineTuneApi/{fineTune}")]
        [AllowAnonymous]
        public async Task<ActionResult<DeletedFineTune>> DeleteSpecifiedFineTuneApi(string fineTune)
        {
            string Model = string.Empty;
            DeletedFineTune RespuestaServicio = new DeletedFineTune(); 
            
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                Root resultado = new Root();
                FineTunesDal ftd = new FineTunesDal(_ConnectionString.GetConnectionString("DefaultConnection"));
                resultado = await ftd.GetTrainingLog();

                using (var httpClient = new HttpClient())
                {
                    using (var request = new HttpRequestMessage(new HttpMethod("GET"), "https://api.openai.com/v1/fine-tunes/" + fineTune))
                    {
                        request.Headers.TryAddWithoutValidation("Authorization", "Bearer " + apiKey);

                        var response = await httpClient.SendAsync(request);
                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            var respuesta = response.Content.ReadAsStringAsync();
                            var data = JsonConvert.DeserializeObject<Root>(respuesta.Result.ToString());

                            if (data.fine_tuned_model == null)
                            {
                                RespuestaServicio.obj = "El modelo actual no se encuentra ";
                            }
                            else
                            {
                                Model = data.fine_tuned_model.ToString();
                            }
                        }
                    }
                }

                using (var httpClient = new HttpClient())
                {
                    using (var request = new HttpRequestMessage(new HttpMethod("DELETE"), "https://api.openai.com/v1/models/" + Model))
                    {
                        request.Headers.TryAddWithoutValidation("Authorization", "Bearer " + apiKey);

                        var response = await httpClient.SendAsync(request);
                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            var respuesta = response.Content.ReadAsStringAsync();
                            RespuestaServicio = JsonConvert.DeserializeObject<DeletedFineTune>(respuesta.Result.ToString());

                        }
                    }
                }

                using (var httpClient = new HttpClient())
                {
                    string apiUrl = $"https://api.openai.com/v1/fine-tunes/{fineTune}/cancel";
                    var request = new HttpRequestMessage(HttpMethod.Post, apiUrl);

                        request.Headers.TryAddWithoutValidation("Authorization", "Bearer " + apiKey);
                        var response = await httpClient.SendAsync(request);
                }
                return RespuestaServicio;

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

        [HttpGet("StartFineTuneApi")]
        [AllowAnonymous]
        public async Task<ActionResult<string>> StartFineTune()
        {
            string TrainingId = string.Empty;

            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                List<ModelJsonL> resultado = new List<ModelJsonL>();
                FineTunesDal ftd = new FineTunesDal(_ConnectionString.GetConnectionString("DefaultConnection"));
                resultado = await ftd.GetAllFineTunesToTraining();

                if (!Directory.Exists("tmp"))
                {
                    Directory.CreateDirectory("tmp");
                }

                string filePath = "tmp/archivoEntrenamiento_" + DateTime.Now.ToShortDateString().Replace("/", "_") + ".jsonl";

                using (StreamWriter fileWriter = new StreamWriter(filePath))
                {
                    foreach (var data in resultado)
                    {
                        string jsonData = JsonConvert.SerializeObject(data);
                        fileWriter.WriteLine(jsonData);
                    }
                }

                var api = new OpenAIAPI(apiKey);

                OpenAI_API.Files.File result = new OpenAI_API.Files.File();
                result.Model = OpenAI_API.Models.Model.GPT4;
                result.Purpose = "fine-tune";
                result.Name = "archivoPruebaFineTune";
                result.Object = "fine-tune";
                result = await api.Files.UploadFileAsync(filePath);
                using (var httpClient = new HttpClient())
                {
                    using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://api.openai.com/v1/fine-tunes"))
                    {
                        request.Headers.TryAddWithoutValidation("Authorization", "Bearer " + apiKey);
                        request.Content = new StringContent("{\"training_file\":  \"" + result.Id + "\", \"model\": \"ada\"}");
                        request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

                        var response = await httpClient.SendAsync(request);
                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            var respuesta = response.Content.ReadAsStringAsync();
                            Root data = JsonConvert.DeserializeObject<Root>(respuesta.Result.ToString());
                            TrainingId = data.id;
                        }
                    }
                }

                ftd = new FineTunesDal(_ConnectionString.GetConnectionString("DefaultConnection"));
                await ftd.RegisterLogTrainings(TrainingId);

                return "TrainingId: " + TrainingId;

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

        /// <summary>
        /// Gets the status of a "fine-tune" process from the API.
        /// </summary>
        /// <param name="fineTune">The ID of the "fine-tune" process to query.</param>
        /// <returns>An object of type Root containing the data of the "fine-tune" process.</returns>
        [HttpGet("GetStatusFineTuneApi/{fineTune}")]
        [AllowAnonymous]
        public async Task<Root> GetStatusFineTuneApi(string fineTune)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            Root data = new Root();

            try
            {
                using (var httpClient = new HttpClient())
                {
                    using (var request = new HttpRequestMessage(HttpMethod.Get, "https://api.openai.com/v1/fine-tunes/" + fineTune))
                    {
                        // Add the authorization header with the OpenAI API Key.
                        request.Headers.TryAddWithoutValidation("Authorization", "Bearer " + apiKey);

                        // Make the HTTP request to the OpenAI API.
                        response = await httpClient.SendAsync(request);

                        // If the response is successful (200 OK), process the response data.
                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            // Read the content of the response.
                            var respuesta = await response.Content.ReadAsStringAsync();

                            // Deserialize the JSON content into an object of type Root.
                            data = JsonConvert.DeserializeObject<Root>(respuesta);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // In case an exception occurs, create an ErrorDetails object to capture the error information.
                var emex = new ErrorDetails()
                {
                    StatusCode = 400,
                    Message = "Error: " + ex.Message.ToString()
                };
            }

            return data;
        }

        [HttpGet("GetApiFineTune")]
        [AllowAnonymous]
        public async Task<string> GetApiFineTune()
        {
            HttpResponseMessage response = new HttpResponseMessage();
            FineTuneResponseModel data = new FineTuneResponseModel();
            string RespuestaServicio = string.Empty;
            try
            {
                using (var httpClient = new HttpClient())
                {
                    using (var request = new HttpRequestMessage(new HttpMethod("GET"), "https://api.openai.com/v1/fine-tunes"))
                    {
                        request.Headers.TryAddWithoutValidation("Authorization", "Bearer " + apiKey);

                        response = await httpClient.SendAsync(request);
                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            var respuesta = response.Content.ReadAsStringAsync();
                            data = JsonConvert.DeserializeObject<FineTuneResponseModel>(respuesta.Result.ToString());
                            RespuestaServicio = respuesta.Result;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var emex = new ErrorDetails()
                {
                    StatusCode = 400,
                    Message = "Error:  " + ex.Message.ToString()
                };
            }

            return RespuestaServicio;

        }
    }
}
