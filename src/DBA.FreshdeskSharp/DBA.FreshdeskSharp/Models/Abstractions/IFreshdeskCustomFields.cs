using Newtonsoft.Json;

namespace DBA.FreshdeskSharp.Models
{
    public interface IFreshdeskCustomFields<TCustomFieldObject> where TCustomFieldObject : class
    {
        [JsonProperty("custom_fields")]
        TCustomFieldObject CustomFields { get; set; }
    }
}