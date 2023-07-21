using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaNegocioServicio.Models
{
    public class SmsRequestInfo
    {
        public string To { get; set; }
        [JsonProperty("text")]
        public string Text { get; set; }
        public string CustomData { get; set; }
        public bool IsPremium { get; set; }
        public bool IsFlash { get; set; }
        public bool Longmessage { get; set; }
        public string Url { get; set; }
        [JsonProperty("domainshorturl")]
        public string Domainshorturl { get; set; }
    }
}
