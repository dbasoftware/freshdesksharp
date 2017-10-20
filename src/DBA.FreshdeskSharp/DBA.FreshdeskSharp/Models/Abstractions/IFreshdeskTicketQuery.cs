using System;
using Newtonsoft.Json;

namespace DBA.FreshdeskSharp.Models.Abstractions
{
    public interface IFreshdeskTicketQuery
    {
        FreshdeskTicketStatus Status { get; }
        string Tag { get; }
        [JsonProperty("group_id")]
        ulong GroupId { get; }
        FreshdeskTicketPriority Priority { get; }
        string Type { get; }
        [JsonProperty("due_by")]
        DateTime DueBy { get; }
        [JsonProperty("fr_due_by")]
        DateTime FrDueBy { get; }
        [JsonProperty("created_at")]
        DateTime CreatedAt { get; }
        [JsonProperty("updated_at")]
        DateTime UpdatedAt { get; }
    }
}