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
