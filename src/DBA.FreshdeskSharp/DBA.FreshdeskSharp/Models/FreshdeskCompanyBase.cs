using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DBA.FreshdeskSharp.Models
{
    public class FreshdeskCompanyBase 
    {
        public FreshdeskCompanyBase()
        {
            Domains = new List<string>();
        }

        public string Description { get; set; }
        public List<string> Domains { get; }
        public ulong Id { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }
        [JsonProperty("updated_at")]
        public DateTime UpdatedAt { get; set; }
    }
}