using System.Collections.Generic;
using DBA.FreshdeskSharp.Serialization;
using Newtonsoft.Json;

namespace DBA.FreshdeskSharp.Models
{
    [JsonConverter(typeof(CustomFieldsConverter))]
    public class FreshdeskCustomFields : Dictionary<string, object>
    {
    }
}