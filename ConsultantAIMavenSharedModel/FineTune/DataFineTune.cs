using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EntregasLogyTechSharedModel.FineTune
{
    internal class DataFineTune
    {
        [Required(ErrorMessage = "El id Perfil es obligatorio")]
        public string Prompt { get; set; }
        [Required(ErrorMessage = "El id Perfil es obligatorio")]
        public string completion { get; set; }
      
        public DataFineTune()
        {
            Prompt =string.Empty;
        }
    }
}
