using System;
using System.Collections.Generic;
using DBA.FreshdeskSharp.Serialization;
using Newtonsoft.Json;

namespace DBA.FreshdeskSharp.Models
{
    public class FreshdeskContactField
    {
        [JsonProperty("editable_in_signup")]
        public bool EditableInSignup { get; set; }
        public ulong Id { get; set; }
        public string Label { get; set; }
        public string Name { get; set; }
        public ulong Position { get; set; }
        public bool Default { get; set; }
        public string Type { get; set; }
        [JsonProperty("customers_can_edit")]
        public bool CustomersCanEdit { get; set; }
        [JsonProperty("label_for_customers")]
        public string LabelForCustomers { get; set; }
        [JsonProperty("required_for_customers")]
        public bool RequiredForCustomers { get; set; }
        [JsonProperty("displayed_for_customers")]
        public bool DisplayedForCustomers { get; set; }
        [JsonProperty("required_for_agents")]
        public bool RequiredForAgents { get; set; }
        [JsonConverter(typeof(FieldChoicesConverter))]
        public object Choices { get; set; }
        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }
        [JsonProperty("updated_at")]
        public DateTime UpdatedAt { get; set; }
    }
}
