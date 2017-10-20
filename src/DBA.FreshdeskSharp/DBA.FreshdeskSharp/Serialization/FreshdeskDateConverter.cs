using System;
using Newtonsoft.Json;

namespace DBA.FreshdeskSharp.Serialization
{
    public class FreshdeskDateConverter: JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            switch (value)
            {
                case null:
                    writer.WriteNull();
                    break;
                case DateTime dateValue:
                    writer.WriteValue($"{dateValue.Year:0000}-{dateValue.Month:00}-{dateValue.Day:00}");
                    break;
                default:
                    writer.WriteValue(value);
                    break;
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanRead => false;

        public override bool CanWrite => true;

        public override bool CanConvert(Type objectType) => objectType == typeof(DateTime);
    }
}
