using System;
using System.Collections.Generic;
using DBA.FreshdeskSharp.Serialization;
using Newtonsoft.Json;

namespace DBA.FreshdeskSharp.Models
{
    public class FreshdeskTicketField
    {        
        public ulong Id { get; set; }
        public string Description { get; set; }
        public string Label { get; set; }
        public string Name { get; set; }
        public int Position { get; set; }
        [JsonProperty("portal_cc")]
        public bool PortalCc { get; set; }
        [JsonProperty("portal_cc_to")]
        public string PortalCcTo { get; set; }
        public string Type { get; set; }
        [JsonProperty("label_for_customers")]
        public string LabelForCustomers { get; set; }
        public bool Default { get; set; }
        [JsonProperty("required_for_closure")]
        public bool RequiredForClosure { get; set; }
        [JsonProperty("required_for_agents")]
        public bool RequiredForAgents { get; set; }
        [JsonProperty("required_for_customers")]
        public bool RequiredForCustomers { get; set; }
        [JsonProperty("customers_can_edit")]
        public bool CustomersCanEdit { get; set; }
        [JsonProperty("displayed_to_customers")]
        public bool DisplayedToCustomers { get; set; }
        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }
        [JsonProperty("updated_at")]
        public DateTime UpdatedAt { get; set; }
        [JsonConverter(typeof(FieldChoicesConverter))]
        public object Choices { get; set; }
        [JsonProperty("nested_ticket_fields")]
        public List<FreshdeskTicketField> NestedTicketFields { get; set; }
    }
}