using System;
using System.Collections.Generic;
using System.Text;

namespace EntregasLogyTechSharedModel.Dynamic
{
    public class DynamicOpportunities
    {
        public string name { get; set; } 
        public int estimatedvalue { get; set; } 
        public string estimatedclosedate { get; set; } 
        public string description { get; set; } 

        [Newtonsoft.Json.JsonProperty("customerid_account@odata.bind")]
        public string customerid_account_odata_bind { get; set; } 

        [Newtonsoft.Json.JsonProperty("customerid_contact@odata.bind")]
        public string customerid_contact_odata_bind { get; set; } 

        [Newtonsoft.Json.JsonProperty("customerid_opportunity@odata.bind")]
        public string customerid_opportunity_odata_bind { get; set; } 
        public int statuscode { get; set; } 
        public int opportunityratingcode { get; set; }
        public string customerneed { get; set; } 
        public int salesstagecode { get; set; } 
        public string actualclosedate { get; set; } 
        public int budgetamount { get; set; } 
        public int closeprobability { get; set; } 
        public int freightamount { get; set; } 
        public string opportunityid { get; set; } 
        public string transactioncurrencyid_odata_bind { get; set; } 
        public string ownerid_odata_bind { get; set; } 
        public string customerid_accountname { get; set; } 
        public string customerid_contactname { get; set; } 
        public string customerid_opportunityname { get; set; }

        public DynamicOpportunities()
        {
            opportunityid = string.Empty;
        }
    }
}
