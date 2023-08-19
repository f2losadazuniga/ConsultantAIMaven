using ConsultantAIMavenSharedModel.Usuarios;
using LogicaNegocioServicio.Comunes;
using LogicaNegocioServicio.Usuarios;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using EntregasLogyTechSharedModel.Customer;
using EntregasLogyTechSharedModel.Conversation;
using LogicaNegocioServicio.Customers;
using System.Security.Claims;

namespace ApiConsultantAIMaven.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CustomerController : ControllerBase
    {
        private readonly IConfiguration _ConnectionString;
        public string apiKey = string.Empty;

        public CustomerController(IConfiguration Configuration)
        {
            this._ConnectionString = Configuration;
           
        }

        [HttpPost("CreateCustomern")]
        public async Task<ActionResult<List<AllConversations>>> InsertCustomern([FromBody] Customer customer)
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
                CustomerDALL cus = new CustomerDALL(_ConnectionString.GetConnectionString("DefaultConnection"));
                result = await cus.InsertCustomern(customer, idUsuario);
                return result;
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
