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

namespace ApiConsultantAIMaven.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatGPTFineTunesController1 : ControllerBase
    {
        private readonly IConfiguration _ConnectionString;
        public ChatGPTFineTunesController1(IConfiguration Configuration)
        {
            this._ConnectionString = Configuration;
        }

        [HttpGet("GetFineTuneId/{Id}")]
        [AllowAnonymous]
        public async Task<ActionResult<AllFineTune>> GetFineTuneId(Int32 Id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                List<AllFineTune> resultado = new List<AllFineTune>();
                FineTunesDal ftd = new FineTunesDal(_ConnectionString.GetConnectionString("DefaultConnection"));
                resultado = await ftd.GetFineTuneId(Id);
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

        [HttpPost("DeleteFineTuneId/{Id}")]
        [AllowAnonymous]
        public async Task<ActionResult<RespuestaServicio>> DeleteFineTuneId(Int32 Id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                RespuestaServicio resultado = new RespuestaServicio();
                FineTunesDal ftd = new FineTunesDal(_ConnectionString.GetConnectionString("DefaultConnection"));
                resultado = await ftd.DeleteFineTuneId(Id);
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

        [HttpGet("GetAllFineTunes")]
        [AllowAnonymous]
        public async Task<ActionResult<List<AllFineTune>>> GetAllFineTunes()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                List<AllFineTune> resultado = new List<AllFineTune>();
                FineTunesDal mnu = new FineTunesDal(_ConnectionString.GetConnectionString("DefaultConnection"));
                resultado = await mnu.GetAllFineTunes();

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

        [HttpGet("StartFineTune")]
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

                string filePath = "archivoEntrenamiento_" + DateTime.Now.ToShortDateString().Replace("/", "_") + ".jsonl";

                using (StreamWriter fileWriter = new StreamWriter(filePath))
                {
                    foreach (var data in resultado)
                    {
                        string jsonData = JsonConvert.SerializeObject(data);
                        fileWriter.WriteLine(jsonData);
                    }
                }

                var api = new OpenAIAPI("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");

                OpenAI_API.Files.File result = new OpenAI_API.Files.File();
                result.Model = OpenAI_API.Models.Model.AdaText;
                result.Purpose = "fine-tune";
                result.Name = "archivoPruebaFineTune";
                result.Object = "fine-tune";
                result = await api.Files.UploadFileAsync(filePath);
                using (var httpClient = new HttpClient())
                {
                    using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://api.openai.com/v1/fine-tunes"))
                    {
                        request.Headers.TryAddWithoutValidation("Authorization", "Bearer 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");
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

        [HttpGet("GetStatusFineTune/{fineTune}")]
        [AllowAnonymous]
        public async Task<Root> GetStatusFineTune(string fineTune)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            Root data = new Root();

            try
            {
                using (var httpClient = new HttpClient())
                {
                    using (var request = new HttpRequestMessage(new HttpMethod("GET"), "https://api.openai.com/v1/fine-tunes/" + fineTune))
                    {
                        request.Headers.TryAddWithoutValidation("Authorization", "Bearer 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");

                        response = await httpClient.SendAsync(request);
                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            var respuesta = response.Content.ReadAsStringAsync();
                            data = JsonConvert.DeserializeObject<Root>(respuesta.Result.ToString());
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
            
            return data;
        }

        [HttpPost("PostFineTunes")]
        [AllowAnonymous]
        public async Task<ActionResult> PostFineTunes([FromBody] List<DataFineTune> FineTune)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                List<RespuestaServicio> resultado = new List<RespuestaServicio>();
                FineTunesDal ftd = new FineTunesDal(_ConnectionString.GetConnectionString("DefaultConnection"));
                resultado = await ftd.InsertAllFineTunes(FineTune);


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

    }
}
