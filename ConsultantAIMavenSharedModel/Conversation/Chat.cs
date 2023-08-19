using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EntregasLogyTechSharedModel.Conversation
{
    public class Chat
    {
        [Required(ErrorMessage = "Email required")]
        [EmailAddress(ErrorMessage = "The email address is not valid.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "message required")]
        public string Message { get; set; }

        public Chat()
        {
            Email = string.Empty;
        }
    }
}
