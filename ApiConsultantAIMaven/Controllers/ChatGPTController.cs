
using ConsultantAIMavenSharedModel.Usuarios;
using LogicaNegocioServicio.Autenticacion;
using LogicaNegocioServicio.Comunes;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using OpenAI_API;
using OpenAI_API.Completions;
using System;
using System.Threading.Tasks;

namespace ApiConsultantAIMaven.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ChatGPTController : ControllerBase
    {
        private readonly IConfiguration _ConnectionString;
        public ChatGPTController(IConfiguration Configuration)
        {
            this._ConnectionString = Configuration;
        }
        [HttpGet("UseChatGPT")]
        [AllowAnonymous]
        public async Task<ActionResult> UseChatGPT(String query)
        {
            try
            {
                string respuestas = string.Empty;
                var openai = new OpenAIAPI("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");
                CompletionRequest completion = new CompletionRequest();
                completion.Prompt = query;
                completion.Model = OpenAI_API.Models.Model.DavinciText;

                var respuesta = openai.Completions.CreateCompletionAsync(completion);
                foreach (var complet in respuesta.Result.Completions)
                {
                    respuestas = complet.Text;
                }
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
