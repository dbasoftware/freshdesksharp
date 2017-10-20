using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DBA.FreshdeskSharp.Serialization
{
    public class FieldChoicesConverter : JsonConverter
    {
        public override bool CanRead => true;

        public override bool CanWrite => false;

        public override bool CanConvert(Type objectType) => objectType == typeof(object);

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type type, object value, JsonSerializer serializer)
        {
            switch (reader.TokenType)
            {
                case JsonToken.StartObject:
                    var jObject = JObject.Load(reader);
                    var dict = new Dictionary<string, object>();
                    serializer.Populate(jObject.CreateReader(), dict);
                    return SpecifyDict(serializer, dict);
                case JsonToken.StartArray:
                    var jArray = JArray.Load(reader);
                    var list = new List<object>();
                    serializer.Populate(jArray.CreateReader(), list);
                    return SpecifyList(list);
                default:
                    throw new ArgumentOutOfRangeException(nameof(reader.TokenType), "Expected StartObject or StartArray");
            }
        }

        private static object SpecifyDict(JsonSerializer serializer, Dictionary<string, object> dict)
        {
            var allStr = false;
            var allBool = false;
            var allLong = false;
            var allPosLong = false;
            var allArray = false;
            var first = true;
            foreach (var item in dict)
            {
                allStr = item.Value is string && (first || allStr);
                allBool = item.Value is bool && (first || allBool);
                allLong = item.Value is long && (first || allLong);
                allPosLong = allLong && (long)item.Value > 0 && (first || allPosLong);
                allArray = item.Value is JArray && (first || allArray);
                first = false;
            }
            if (allStr)
            {
                return dict.ToDictionary(item => item.Key, item => (string) item.Value);
            }
            if (allBool)
            {
                return dict.ToDictionary(item => item.Key, item => (bool)item.Value);
            }
            if (allPosLong)
            {
                return dict.ToDictionary(item => item.Key, item => (ulong)(long)item.Value);
            }
            if (allLong)
            {
                return dict.ToDictionary(item => item.Key, item => (long)item.Value);
            }
            if (allArray)
            {
                first = true;
                var d = new Dictionary<string, object>();
                foreach (var item in dict)
                {
                    var list = new List<object>();
                    serializer.Populate(((JArray)item.Value).CreateReader(), list);
                    foreach (var listItem in list)
                    {
                        allStr = listItem is string && (first || allStr);
                        allBool = listItem is bool && (first || allBool);
                        allLong = listItem is long && (first || allLong);
                        allPosLong = allLong && (long)listItem > 0 && (first || allPosLong);
                        first = false;
                    }
                    d.Add(item.Key, SpecifyList(list));
                }
                if (allStr)
                {
                    return d.ToDictionary(dictItem => dictItem.Key, dictItem => (List<string>) dictItem.Value);
                }
                if (allBool)
                {
                    return d.ToDictionary(dictItem => dictItem.Key, dictItem => (List<bool>)dictItem.Value);
                }
                if (allPosLong)
                {
                    return d.ToDictionary(dictItem => dictItem.Key, dictItem => (List<ulong>)dictItem.Value);
                }
                if (allLong)
                {
                    return d.ToDictionary(dictItem => dictItem.Key, dictItem => (List<long>)dictItem.Value);
                }
                return d;
            }
            return dict;
        }

        private static object SpecifyList(IReadOnlyCollection<object> list)
        {
            var allStr = false;
            var allBool = false;
            var allLong = false;
            var allPosLong = false;
            var first = true;
            foreach (var item in list)
            {
                allStr = item is string && (first || allStr);
                allBool = item is bool && (first || allBool);
                allLong = item is long && (first || allLong);
                allPosLong = allLong && (long)item > 0 && (first || allPosLong);
                first = false;
            }
            if (allStr)
            {
                return list.Select(item => item as string).ToList();
            }
            if (allBool)
            {
                return list.Select(item => (bool)item).ToList();
            }
            if (allPosLong)
            {
                return list.Select(item => (ulong)(long)item).ToList();
            }
            if (allLong)
            {
                return list.Select(item => (long)item).ToList();
            }
            return list;
        }
    }
}