using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EntregasLogyTechSharedModel.Conversation
{
    public class RecordReaction
    {
        [Required(ErrorMessage = "idConversation required")]
        public int idConversation { get; set; }
        [Required(ErrorMessage = "Reaction required")]
        public bool? Reaction { get; set; }
        public string Message { get; set; }        
        public RecordReaction()
        {
            Message = string.Empty;
        }

    }
}
