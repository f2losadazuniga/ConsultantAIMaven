using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaNegocioServicio.Comunes
{
    public class LogServicios
    {
        public int IdLogServicios { get; set; }
        public string Servicio { get; set; }
        public string Peticion { get; set; }
        public string Respuesta { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}
