using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaNegocioServicio.Autenticacion
{
    public class UserRequest
    {
        [Required(ErrorMessage = "El Usuario es obligatorio")]
        public string Usuario { get; set; }
        
    }
}
