using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EntregasLogyTechSharedModel.FineTune
{
    public class AllFineTune
    {
        public Int32 Id { get; set; }

        [Required(ErrorMessage = "El id Perfil es obligatorio")]
        public string Prompt { get; set; }
        [Required(ErrorMessage = "El id Perfil es obligatorio")]
        public string Completion { get; set; }
      
        public AllFineTune()
        {
            Prompt =string.Empty;
        }
    }
}
