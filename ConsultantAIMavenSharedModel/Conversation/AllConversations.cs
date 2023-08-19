using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EntregasLogyTechSharedModel.Conversation
{
    public class AllConversations
    {

        public int IdConversation { get; set; }
        public string Ask { get; set; }
        public string Answer { get; set; }
        public bool? Reaction { get; set; }
        public string Message { get; set; }

        public AllConversations()
        {
            Ask = string.Empty;
            Reaction = null;
        }
    }
}
