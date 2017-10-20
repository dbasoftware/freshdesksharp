using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DBA.FreshdeskSharp.Serialization
{
    public class CustomFieldsConverter : JsonConverter
    {
        public override bool CanRead => false;

        public override bool CanWrite => true;

        public override bool CanConvert(Type objectType) => objectType == typeof(Dictionary<string, object>);

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (!(value is Dictionary<string, object> dict))
            {
                writer.WriteNull();
                return;
            }
            writer.WriteStartObject();
            foreach (var item in dict)
            {
                writer.WritePropertyName(item.Key);
                switch (item.Value)
                {
                    case null:
                        writer.WriteNull();
                        break;
                    case DateTime dateValue:
                        writer.WriteValue($"{dateValue.Year}-{dateValue.Month:00}-{dateValue.Day:00}");
                        break;
                    default:
                        writer.WriteValue(item.Value);
                        break;
                }
            } 
            writer.WriteEndObject();
        }

        public override object ReadJson(JsonReader reader, Type type, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
