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
        public List<string> Domains { get; set; }
        public ulong Id { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        [JsonProperty("health_score")]
        public string HealthScore { get; set; }
        [JsonProperty("account_tier")]
        public string AccountTier { get; set; }
        [JsonProperty("renewal_date")]
        public DateTime? RenewalDate { get; set; }
        public string Industry { get; set; }
        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }
        [JsonProperty("updated_at")]
        public DateTime UpdatedAt { get; set; }
    }
}