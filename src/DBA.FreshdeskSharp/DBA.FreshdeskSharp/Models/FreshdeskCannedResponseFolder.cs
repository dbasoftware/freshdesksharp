using System;
using System.Collections.Generic;
using DBA.FreshdeskSharp.Serialization;
using Newtonsoft.Json;

namespace DBA.FreshdeskSharp.Models
{
    public class FreshdeskCannedResponseFolder
    {
        public FreshdeskCannedResponseFolder()
        {
            CannedResponses = new List<FreshdeskCannedResponse>();
        }

        public ulong Id { get; set; }
        public string Name { get; set; }
        [JsonProperty("canned_responses")]
        public List<FreshdeskCannedResponse> CannedResponses { get; set; }
        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        
    }
}
