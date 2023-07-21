using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EntregasLogyTechSharedModel.FineTune
{
    public class DataFineTune
    {
        [Required(ErrorMessage = "Prompt required")]
        public string Prompt { get; set; }

        [Required(ErrorMessage = "Completion required")]
        public string Completion { get; set; }

        public int IdUsuario { get; set; }
      
        public DataFineTune()
        {
            Prompt =string.Empty;
        }
    }
}
