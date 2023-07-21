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
        [HttpGet("GetFineTunes")]
        [AllowAnonymous]
        public async Task<ActionResult> GetFineTunes()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                List<Users> resultado = new List<Users>();
                UsersDal mnu = new UsersDal(_ConnectionString.GetConnectionString("DefaultConnection"));
                resultado = await mnu.GetAllUsers();
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
        public async Task<ActionResult> PostFineTunes([FromBody] DataFineTune FineTune)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                List<Users> resultado = new List<Users>();
                UsersDal mnu = new UsersDal(_ConnectionString.GetConnectionString("DefaultConnection"));
                resultado = await mnu.GetAllUsers();
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
