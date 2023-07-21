using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaNegocioServicio.Usuarios
{
    public class UserRecover
    {
        public int idUsuario{ get; set; }
        public string pwd { get; set; }
        public string token { get; set; }
    }
}
