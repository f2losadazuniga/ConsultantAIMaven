using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Configuration;
using LogicaNegocioServicio.Comunes;
using ConsultantAIMavenSharedModel.Perfil;
using LogicaNegocioServicio.Perfil;

namespace ApiIntegracionEntregasLogyTech.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PerfilesController : ControllerBase
    {
        private readonly IConfiguration _ConnectionString;

        public PerfilesController(IConfiguration Configuration)
        {
            this._ConnectionString = Configuration;

        }

        [HttpGet("ConsultaPerfiles")]      
        public async Task<ActionResult<List<Perfiles>>> ConsultarPerfiles()
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
                        Message = "Error: Se requiere un parametro de busqueda "
                    };
                    return new JsonResult(em);
                }

                List<Perfiles> resultado = new List<Perfiles>();
                PerfilesDal mnu = new PerfilesDal(_ConnectionString.GetConnectionString("DefaultConnection"));
                resultado = await mnu.GetAllPerfiles();
                return resultado;
            }
            catch (Exception ex)
            {
                var emex = new ErrorDetails()
                {
                    StatusCode = 400,
                    Message = "Error:  " + ex.Message.ToString()
                };
                return new JsonResult(emex);
            }
        }
    }
}
