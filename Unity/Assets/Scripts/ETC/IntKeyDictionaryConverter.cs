using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntKeyDictionaryConverter<TValue> : JsonConverter<Dictionary<int, TValue>>
{
    public override Dictionary<int, TValue> ReadJson(JsonReader reader, Type objectType, Dictionary<int, TValue> existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var token = JToken.Load(reader);
        var dict = new Dictionary<int, TValue>();

        if (token.Type == JTokenType.Array)
        {
            // [ null, {...}, {...}, ... ]
            var arr = (JArray)token;
            for (int i = 0; i < arr.Count; i++)
            {
                var elem = arr[i];
                if (elem != null && elem.Type != JTokenType.Null)
                {
                    var value = elem.ToObject<TValue>(serializer);
                    dict[i] = value;
                }
            }
        }
        else if (token.Type == JTokenType.Object)
        {
            // { "3": {...}, "7": {...}, ... }
            var obj = (JObject)token;
            foreach (var prop in obj.Properties())
            {
                if (int.TryParse(prop.Name, out int key))
                {
                    var value = prop.Value.ToObject<TValue>(serializer);
                    dict[key] = value;
                }
            }
        }

        return dict;
    }

    public override void WriteJson(JsonWriter writer, Dictionary<int, TValue> value, JsonSerializer serializer)
    {
        writer.WriteStartObject();
        foreach (var kvp in value)
        {
            writer.WritePropertyName(kvp.Key.ToString());
            serializer.Serialize(writer, kvp.Value);
        }
        writer.WriteEndObject();
    }
}
