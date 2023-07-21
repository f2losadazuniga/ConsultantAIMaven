using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaNegocioServicio.Models
{
    public class MasivApiResponse
    {
         [JsonProperty("codigo")]
        public string Codigo { get; set; }
        [JsonProperty("mensaje")]
        public string Mensaje { get; set; }

        public MasivApiResponse()
        {

        }

        public MasivApiResponse(string code, string description)
        {
            Codigo = code;
            Mensaje = description;
        }

    }
}
