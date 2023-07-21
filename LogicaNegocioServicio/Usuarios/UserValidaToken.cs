using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaNegocioServicio.Usuarios
{
    public class UserValidaToken
    {
        public int idUsuario{ get; set; }
        public string contrasenaActual { get; set; }
        public string Usuario { get; set; }
        public int result { get; set; }
        public string message { get; set; }
    }
}
