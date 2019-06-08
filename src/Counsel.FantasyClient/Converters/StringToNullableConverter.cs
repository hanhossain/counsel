using System;
using Newtonsoft.Json;

namespace Counsel.FantasyClient.Converters
{
    public class StringToNullableConverter<T> : JsonConverter<T?> where T : struct
    {
        public override T? ReadJson(JsonReader reader, Type objectType, T? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            // The NFL Fantasy Football API will return false instead of null so only deserialize if it's a string.
            if (reader.TokenType == JsonToken.String)
            {
                return serializer.Deserialize<T?>(reader);
            }

            return null;
        }

        public override void WriteJson(JsonWriter writer, T? value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
