using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Benchmark.Utils
{
    public class ListConverter<T> : JsonConverter<IEnumerable<T>>
    {
        public override bool CanConvert(Type typeToConvert)
         => typeof(IEnumerable<T>).IsAssignableFrom(typeToConvert);

        public override IEnumerable<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartArray)
                throw new JsonException();

            var value = new List<T>();

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndArray)
                    return value;

                var element = JsonSerializer.Deserialize<T>(ref reader, options);
                value.Add(element);
            }

            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, IEnumerable<T> value, JsonSerializerOptions options)
         => JsonSerializer.Serialize(writer, value);
    }
}
