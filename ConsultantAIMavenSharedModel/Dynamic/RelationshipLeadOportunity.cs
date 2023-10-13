using System;
using System.Collections.Generic;
using System.Text;

namespace EntregasLogyTechSharedModel.Dynamic
{
    public class RelationshipLeadOportunity
    {
        public Relationship Relationship { get; set; }
        public List<RelatedEntity> RelatedEntities { get; set; }
        public RelationshipLeadOportunity()
        {
            Relationship = new Relationship();
            RelatedEntities = new List<RelatedEntity>();
        }
    }
    public class Relationship
    {
        [Newtonsoft.Json.JsonProperty("@odata.type")]
        public string odatatype { get; set; }
        public string PrimaryEntityRole { get; set; }
        public string SchemaName { get; set; }
        public Relationship()
        {
            PrimaryEntityRole = string.Empty;
        }
    }

    public class RelatedEntity
    {
        [Newtonsoft.Json.JsonProperty("@odata.type")]
        public string odatatype { get; set; }
        public string id { get; set; }
        public RelatedEntity()
        {
            odatatype = string.Empty;
        }
    }
}
