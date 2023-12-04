
using ConsultantAIMavenSharedModel.Comunes;
using EntregasLogyTechSharedModel.Conversation;
using EntregasLogyTechSharedModel.Customer;
using EntregasLogyTechSharedModel.FineTune;
using LogicaNegocioServicio.Comunes;
using LogicaNegocioServicio.Conversation;
using LogicaNegocioServicio.Customers;
using LogicaNegocioServicio.FineTunes;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OpenAI_API;
using OpenAI_API.Completions;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
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
            apiKey = ConfigValues.seleccionarConfigValue("ApikeyChatGPT", _ConnectionString.GetConnectionString("DefaultConnection"));
        }

        [HttpPost("UseChatGPT")]
        //[AllowAnonymous]
        public async Task<ActionResult<Answer>> UseChatGPT(Chat query )
        {
            Answer result = new Answer();
            QuestionClass questionData = new QuestionClass();
            questionData.question = query.Message;
            string jsonContent = JsonConvert.SerializeObject(questionData);
            string url = ConfigValues.seleccionarConfigValue("URLApiChatcanvas" + query.Estado.ToString(), _ConnectionString.GetConnectionString("DefaultConnection"));
            if (string.IsNullOrWhiteSpace(url))
            {
                var em = new ErrorDetails()
                {
                    StatusCode = 400,
                    Message = "Error: Could not set the url to start the conversation "
                };
                return new JsonResult(em);
            }

            var identity = (ClaimsIdentity)User.Identity;
            var claims = identity.FindFirst(c => c.Type == "IdUsuario");
            var idUsuario = Convert.ToInt32(claims.Value);
            if (idUsuario <= 0)
            {
                var em = new ErrorDetails()
                {
                    StatusCode = 400,
                    Message = "Error: The user is not authorized "
                };
                return new JsonResult(em);
            }
            try
            {
                using (var httpClient = new HttpClient())
                {
                    // Configurar el encabezado de contenido JSON
                    httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
                    var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                    // Enviar la solicitud POST al servicio
                    HttpResponseMessage response = await httpClient.PostAsync(url, content);
                    // Verificar si la solicitud fue exitosa (código de estado HTTP 200 OK)
                    if (response.IsSuccessStatusCode)
                    {
                        // Leer la respuesta como cadena JSON
                        string responseContent = await response.Content.ReadAsStringAsync();
                        responseContent = responseContent.Replace("\"","");
                        ChatDALL chat = new ChatDALL(_ConnectionString.GetConnectionString("DefaultConnection"));
                        await chat.InsertConversation(query, idUsuario, responseContent);
                        result.answer = responseContent;
                        return result;
                    }
                    else
                    {
                        var emex = new ErrorDetails()
                        {
                            StatusCode = 400,
                            Message = "a conversation could not be established"
                        };
                        return BadRequest(new JsonResult(emex));
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
                return BadRequest(new JsonResult(emex));
            }
        }


        //public async Task<ActionResult<Answer>> UseChatGPT(Chat query)
        //{
        //    Answer result = new Answer();

        //    string fineTuneModel = string.Empty;
        //    var identity = (ClaimsIdentity)User.Identity;
        //    var claims = identity.FindFirst(c => c.Type == "IdUsuario");
        //    var idUsuario = Convert.ToInt32(claims.Value);
        //    if (idUsuario <= 0)
        //    {
        //        var em = new ErrorDetails()
        //        {
        //            StatusCode = 400,
        //            Message = "Error: The user is not authorized "
        //        };
        //        return new JsonResult(em);
        //    }

        //    try
        //    {
        //        Root resultado = new Root();
        //        FineTunesDal ftd = new FineTunesDal(_ConnectionString.GetConnectionString("DefaultConnection"));
        //        resultado = await ftd.GetTrainingLog();

        //        using (var httpClient = new HttpClient())
        //        {
        //            using (var request = new HttpRequestMessage(new HttpMethod("GET"), "https://api.openai.com/v1/fine-tunes/" + resultado.id))
        //            {
        //                request.Headers.TryAddWithoutValidation("Authorization", "Bearer " + apiKey);

        //                var response = await httpClient.SendAsync(request);
        //                if (response.StatusCode == System.Net.HttpStatusCode.OK)
        //                {
        //                    var respuestaString = response.Content.ReadAsStringAsync();
        //                    var data = JsonConvert.DeserializeObject<Root>(respuestaString.Result.ToString());
        //                    if (data.fine_tuned_model == null)
        //                    {
        //                        var emex = new ErrorDetails()
        //                        {
        //                            StatusCode = 400,
        //                            Message = "No available trained model found"
        //                        };
        //                        return BadRequest(new JsonResult(emex));
        //                    }
        //                    else
        //                    {
        //                        fineTuneModel = data.fine_tuned_model.ToString();

        //                    }

        //                }
        //            }
        //        }

        //        string respuestas = string.Empty;


        //        if (fineTuneModel != null)
        //        {
        //            var openai = new OpenAIAPI(apiKey);
        //            CompletionRequest completion = new CompletionRequest();
        //            completion.Prompt = query.Message;
        //            completion.Model = fineTuneModel;
        //            var respuesta = openai.Completions.CreateCompletionAsync(completion);
        //            foreach (var complet in respuesta.Result.Completions)
        //            {
        //                if (respuestas == null || respuestas == "")
        //                {
        //                    respuestas = complet.Text;
        //                }
        //                else
        //                {
        //                    respuestas = respuestas + " " + complet.Text;
        //                }
        //            }
        //            ChatDALL chat = new ChatDALL(_ConnectionString.GetConnectionString("DefaultConnection"));
        //            await chat.InsertConversation(query, idUsuario, respuestas);
        //            result.answer = respuestas;
        //            return result;
        //        }
        //        else
        //        {
        //            var emex = new ErrorDetails()
        //            {
        //                StatusCode = 400,
        //                Message = "No se encontró modelo activo"
        //            };
        //            return BadRequest(new JsonResult(emex));
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        var emex = new ErrorDetails()
        //        {
        //            StatusCode = 400,
        //            Message = "Error:  " + ex.Message.ToString()
        //        };
        //        return BadRequest(new JsonResult(emex));
        //    }

        //}

        [HttpGet("GetAllChat")]
        //[AllowAnonymous]
        public async Task<ActionResult<List<AllConversations>>> GetAllChat(string email)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var identity = (ClaimsIdentity)User.Identity;
                var claims = identity.FindFirst(c => c.Type == "IdUsuario");
                var idUsuario = Convert.ToInt32(claims.Value);
                if (idUsuario <= 0)
                {
                    var em = new ErrorDetails()
                    {
                        StatusCode = 400,
                        Message = "Error: The user is not authorized "
                    };
                    return new JsonResult(em);
                }
                List<AllConversations> result = new List<AllConversations>();
                ChatDALL objChat = new ChatDALL(_ConnectionString.GetConnectionString("DefaultConnection"));
                result = await objChat.GetAllChat(email, idUsuario);

                return Ok(result);
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
        /// Insert a reaction record.
        /// </summary>
        /// <param name="Reaction">The reaction record to insert.</param>
        /// <returns>An ActionResult containing the response of the insertion.</returns>
        [HttpPost("InsertReaction")]
        public async Task<ActionResult<RespuestaServicio>> InsertReaction([FromBody] RecordReaction Reaction)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Get the user's ID from claims
                var identity = (ClaimsIdentity)User.Identity;
                var claims = identity.FindFirst(c => c.Type == "IdUsuario");
                var idUsuario = Convert.ToInt32(claims.Value);

                // Check user authorization
                if (idUsuario <= 0)
                {
                    return BadRequest(new RespuestaServicio
                    {
                        Codigo = "400",
                        Mensaje = "Error: The user is not authorized"
                    });
                }
                // Initialize ChatDALL using the provided connection string
                var objChat = new ChatDALL(_ConnectionString.GetConnectionString("DefaultConnection"));
                // Insert the reaction using the ChatDALL service
                var result = await objChat.InsertReaction(Reaction, idUsuario);
                // Return the successful result
                return result;
            }
            catch (Exception ex)
            {
                return BadRequest(new RespuestaServicio
                {
                    Codigo = "400",
                    Mensaje = $"Error: {ex.Message}"
                });
            }
        }

    }
}
