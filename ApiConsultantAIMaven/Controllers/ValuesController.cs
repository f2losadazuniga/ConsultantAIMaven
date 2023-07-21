using System;
using System.Collections.Generic;
using LogicaNegocioServicio.Comunes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace ApiIntegracionEntregasLogyTech.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IConfiguration _ConnectionString;
        public ValuesController(IConfiguration Configuration)
        {
            this._ConnectionString = Configuration;

        }
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

   }
}
