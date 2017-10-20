using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DBA.FreshdeskSharp.Models
{
    public class FreshdeskTicketBase
    {
        public FreshdeskTicketBase()
        {
            Attachments = new List<object>();
            CcEmails = new List<string>();
            ToEmails = new List<string>();
            FwdEmails = new List<string>();
            ReplyCcEmails = new List<string>();
            Tags = new List<string>();
            Status = FreshdeskTicketStatus.Open;
            Priority = FreshdeskTicketPriority.Low;
            Source = FreshdeskTicketSource.Phone;
        }

        public bool Deleted { get; set; }
        public List<object> Attachments { get; }
        [JsonProperty("cc_emails")]
        public List<string> CcEmails { get; }
        [JsonProperty("fwd_emails")]
        public List<string> FwdEmails { get; }
        [JsonProperty("reply_cc_emails")]
        public List<string> ReplyCcEmails { get; }
        [JsonProperty("email_config_id")]
        public ulong? EmailConfigId { get; set; }
        [JsonProperty("facebook_id")]
        public string FacebookId { get; set; }
        [JsonProperty("group_id")]
        public ulong? GroupId { get; set; }
        public FreshdeskTicketPriority Priority { get; set; }
        [JsonProperty("requester_id")]
        public ulong? RequesterId { get; set; }
        [JsonProperty("responder_id")]
        public ulong? ResponderId { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public FreshdeskTicketSource Source { get; set; }
        public FreshdeskTicketStatus Status { get; set; }
        public string Subject { get; set; }
        [JsonProperty("company_id")]
        public ulong? CompanyId { get; set; }
        public ulong Id { get; set; }
        public string Type { get; set; }
        [JsonProperty("to_emails")]
        public List<string> ToEmails { get; }
        [JsonProperty("product_id")]
        public ulong? ProductId { get; set; }
        [JsonProperty("fr_escalated")]
        public bool FrEscalated { get; set; }
        public bool Spam { get; set; }
        [JsonProperty("is_escalated")]
        public bool IsEscalated { get; set; }
        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }
        [JsonProperty("updated_at")]
        public DateTime UpdatedAt { get; set; }
        [JsonProperty("due_by")]
        public DateTime DueBy { get; set; }
        [JsonProperty("fr_due_by")]
        public DateTime FrDueBy { get; set; }
        [JsonProperty("description_text")]
        public string DescriptionText { get; set; }
        public string Description { get; set; }
        public List<string> Tags { get; }
    }
}