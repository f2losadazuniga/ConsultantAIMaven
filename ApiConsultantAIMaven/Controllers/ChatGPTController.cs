
using Azure;
using ConsultantAIMavenSharedModel.Usuarios;
using LogicaNegocioServicio.Autenticacion;
using LogicaNegocioServicio.Comunes;
using LogicaNegocioServicio.FineTunes;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OpenAI_API;
using OpenAI_API.Completions;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using static EntregasLogyTechSharedModel.FineTune.FineTuneResponseModel;

namespace ApiConsultantAIMaven.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ChatGPTController : ControllerBase
    {
        private readonly IConfiguration _ConnectionString;
        public string apiKey = string.Empty;

        public ChatGPTController(IConfiguration Configuration)
        {
            this._ConnectionString = Configuration;
            apiKey = _ConnectionString.GetSection("ApiKey:key").Value;
        }

        [HttpGet("UseChatGPT")]
        [AllowAnonymous]
        public async Task<ActionResult> UseChatGPT(String query)
        {
            string fineTuneModel = string.Empty;
            try
            {
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
                            var respuestaString = response.Content.ReadAsStringAsync();
                            var data = JsonConvert.DeserializeObject<Root>(respuestaString.Result.ToString());
                            fineTuneModel = data.fine_tuned_model.ToString();
                        }
                    }
                }

                string respuestas = string.Empty;
                //var openai = new OpenAIAPI(apiKey);
                //CompletionRequest completion = new CompletionRequest();
                //completion.Prompt = query;
                //completion.Model = fineTuneModel;

                using (var httpClient = new HttpClient())
                {
                    using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://api.openai.com/v1/completions"))
                    {
                        request.Headers.TryAddWithoutValidation("Authorization", "Bearer " + apiKey);

                        request.Content = new StringContent("{\n    \"model\": \"" + fineTuneModel + "\",\n    \"prompt\": \"" + query + "\",\n    \"max_tokens\": 7,\n    \"temperature\": 0\n  }");
                        request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

                        var response = await httpClient.SendAsync(request);
                        var respuestaServicio = response.Content.ReadAsStringAsync();

                    }
                }

                //var respuesta = openai.Completions.CreateCompletionAsync(completion);
                //foreach (var complet in respuesta.Result.Completions)
                //{
                //    respuestas = complet.Text;
                //}
                return Ok(respuestas);

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
