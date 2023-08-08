using System;
using System.Collections.Generic;
using DBA.FreshdeskSharp.Serialization;
using Newtonsoft.Json;

namespace DBA.FreshdeskSharp.Models
{
    public class FreshdeskCannedResponse
    {
        public FreshdeskCannedResponse()
        {
            Attachments = new List<object>();
            GroupIds = new List<ulong> { };
        }

        public ulong Id { get; set; }
        public string Title { get; set; }
        [JsonProperty("content_html")]
        public string ContentHtml { get; set; }
        public string Content { get; set; }
        [JsonProperty("folder_id")]
        public ulong FolderId { get; set; }
        public int Position { get; set; }
        [JsonProperty("group_ids")]
        public List<ulong> GroupIds { get; set; }
        public List<object> Attachments { get; set; }
        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }
        [JsonProperty("updated_at")]
        public DateTime UpdatedAt { get; set; }
    }
}
