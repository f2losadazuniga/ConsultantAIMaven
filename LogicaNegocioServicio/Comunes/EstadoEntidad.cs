using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaNegocioServicio.Comunes
{
    public class EstadoEntidad
    {
        public int IdEstado { get; set; }
        public string Estado { get; set; }
        public EstadoEntidad()
        {
            IdEstado = 0;
        }
    }
}
