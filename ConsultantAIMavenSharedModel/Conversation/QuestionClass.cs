using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EntregasLogyTechSharedModel.Conversation
{
    public class QuestionClass
    {
        [Required(ErrorMessage = "Email required")]
        public string question { get; set; }



        public QuestionClass()
        {
            question = string.Empty;
        }
    }
}
