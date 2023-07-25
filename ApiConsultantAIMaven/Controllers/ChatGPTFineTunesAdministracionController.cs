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
    [Route("administracion/[controller]")]
    [ApiController]
    public class ChatGPTFineTunesAdministracionController : ControllerBase
    {
        private readonly IConfiguration _ConnectionString;
        public string apiKey = string.Empty;

        public ChatGPTFineTunesAdministracionController(IConfiguration Configuration)
        {
            this._ConnectionString = Configuration;
            apiKey = _ConnectionString.GetSection("ApiKey:key").Value;
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

        [HttpPost("InsertAllFineTunes")]
        [AllowAnonymous]
        public async Task<ActionResult> InsertAllFineTunes([FromBody] List<DataFineTune> FineTune)
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
