using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DBA.FreshdeskSharp.Models
{
    public class FreshdeskOutboundEmailBase

    {
        public FreshdeskOutboundEmailBase()
        {
            Tags = new List<string>();
            Status = FreshdeskTicketStatus.Open;
            Priority = FreshdeskTicketPriority.Low;
        }

        public string Name { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Type { get; set; }

        public FreshdeskTicketStatus Status { get; set; }

        public FreshdeskTicketPriority Priority { get; set; }

        public string Description { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<object> Attachments { get; set; }
        [JsonProperty("due_by")]
        public DateTime? DueBy { get; set; }
        [JsonProperty("email_config_id")]
        public ulong? EmailConfigId { get; set; }
        [JsonProperty("fr_due_by")]
        public DateTime? FrDueBy { get; set; }
        [JsonProperty("group_id")]
        public ulong? GroupId { get; set; }
        public List<string> Tags { get; }



    }
}