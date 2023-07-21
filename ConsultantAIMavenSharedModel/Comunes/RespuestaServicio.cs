using System;
using System.Collections.Generic;
using System.Text;

namespace ConsultantAIMavenSharedModel.Comunes
{
    public class RespuestaServicio
    {
        public string Codigo { get; set; }
        public string Mensaje { get; set; }
         public RespuestaServicio()
         {
            Codigo = "0";

         }
    }
}
