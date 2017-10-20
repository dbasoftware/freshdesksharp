using System;
using System.Collections.Generic;
using DBA.FreshdeskSharp.Serialization;
using Newtonsoft.Json;

namespace DBA.FreshdeskSharp.Models
{
    public class FreshdeskCompanyField
    {
        public ulong Id { get; set; }
        public string Label { get; set; }
        public string Name { get; set; }
        public ulong Position { get; set; }
        public bool Default { get; set; }
        public string Type { get; set; }
        [JsonConverter(typeof(FieldChoicesConverter))]
        public object Choices { get; set; }
        [JsonProperty("required_for_agent")]
        public bool RequiredForAgent { get; set; }
        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }
        [JsonProperty("updated_at")]
        public DateTime UpdatedAt { get; set; }
    }
}