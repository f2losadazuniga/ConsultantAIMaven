using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsultantAIMavenSharedModel.Usuarios
{
    public class UsuarioLogin
    {
        [Required(ErrorMessage = "El Usuario es obligatorio")]
        public string Usuario { get; set; }
        [Required(ErrorMessage = "El Password es obligatorio")]
        public string Password { get; set; }
    }
}
