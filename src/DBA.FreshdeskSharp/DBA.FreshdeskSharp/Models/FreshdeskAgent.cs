using Newtonsoft.Json;
using System;

namespace DBA.FreshdeskSharp.Models
{
    public class FreshdeskAgent
    {
        public bool Available { get; set; }
        [JsonProperty("available_since")]
        public DateTime? AvailableSince { get; set; }
        public ulong Id { get; set; }
        public bool Occasional { get; set; }
        public string Signature { get; set; }
        [JsonProperty("ticket_scope")]
        public byte TicketScope { get; set; }
        public string Type { get; set; }
        [JsonProperty("skill_ids")]
        public ulong[] SkillIds { get; set; }
        [JsonProperty("group_ids")]
        public ulong[] GroupIds { get; set; }
        [JsonProperty("role_ids")]
        public ulong[] RoleIds { get; set; }
        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }
        [JsonProperty("updated_at")]
        public DateTime UpdatedAt { get; set; }
        public FreshdeskContact Contact { get; set; }
    }
}
