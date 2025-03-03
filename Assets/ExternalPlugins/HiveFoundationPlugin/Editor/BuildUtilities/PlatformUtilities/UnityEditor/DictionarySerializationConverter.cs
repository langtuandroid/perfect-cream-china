using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Modules.Hive.Editor.BuildUtilities
{
    public class DictionarySerializationConverter<TKey, TValue> : JsonConverter
    {
        public override bool CanConvert(Type objectType) => objectType == typeof(Dictionary<TKey, TValue>);

        
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) => 
            serializer.Serialize(writer, ((Dictionary<TKey, TValue>)value).ToList());

        
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) => 
            serializer.Deserialize<KeyValuePair<TKey, TValue>[]>(reader).ToDictionary(
                kv => kv.Key, 
                kv => kv.Value);
    }
}