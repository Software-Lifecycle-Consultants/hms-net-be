using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace HMS.Utilities
{
  

    public class DictionaryStringIntJsonConverter : JsonConverter<Dictionary<int, int>>
    {
        public override Dictionary<int, int> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var dictionary = new Dictionary<int, int>();

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                    return dictionary;

                string key = reader.GetString();
                reader.Read();
                int value = reader.GetInt32();

                if (int.TryParse(key, out int intKey))
                {
                    dictionary[intKey] = value;
                }
            }

            return dictionary;
        }

        public override void Write(Utf8JsonWriter writer, Dictionary<int, int> value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            foreach (var kvp in value)
            {
                writer.WritePropertyName(kvp.Key.ToString());
                writer.WriteNumberValue(kvp.Value);
            }
            writer.WriteEndObject();
        }
    }

}
