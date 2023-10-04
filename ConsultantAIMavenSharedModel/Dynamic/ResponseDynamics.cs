using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EntregasLogyTechSharedModel.Dynamic
{
    public class ResponseDynamics
    {
        public string Token_type { get; set; }

        public string Expires_in { get; set; }
        public string Ext_expires_in { get; set; }
        public string Expires_on { get; set; }
        public string Not_before { get; set; }
        public string Resource { get; set; }
        public string access_token { get; set; }


        public ResponseDynamics()
        {
            access_token = string.Empty;
        }

    }
}
