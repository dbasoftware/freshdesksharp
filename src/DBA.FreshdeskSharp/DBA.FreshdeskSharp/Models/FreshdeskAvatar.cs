using System;
using Newtonsoft.Json;

namespace DBA.FreshdeskSharp.Models
{
    public class FreshdeskAvatar
    {
        [JsonProperty("avatar_url")]
        public string AvatarUrl { get; set; }
        [JsonProperty("content_type")]
        public string ContentType { get; set; }
        public ulong Id { get; set; }
        public string Name { get; set; }
        public uint Size { get; set; }
        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }
        [JsonProperty("updated_at")]
        public DateTime UpdatedAt { get; set; }
    }
}