using Azure.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace EntregasLogyTechSharedModel.Dynamic
{
    public class LeadDynamic
    {
        public string subject { get; set; }
        public string lastname { get; set; }
        public string firstname { get; set; }
        public string companyname { get; set; }
        public string emailaddress1 { get; set; }
        public string telephone1 { get; set; }
        public string mobilephone { get; set; }
        public string address1_line1 { get; set; }
        public string address1_city { get; set; }
        public string address1_stateorprovince { get; set; }
        public string address1_postalcode { get; set; }
        public string address1_country { get; set; }
        public LeadDynamic()
        {
            subject = string.Empty;
        }

    }
}
