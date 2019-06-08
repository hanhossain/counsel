using System;
using Counsel.FantasyClient.Models;
using Newtonsoft.Json;

namespace Counsel.FantasyClient.Converters
{
    public class PositionConverter : JsonConverter<Position>
    {
        public override Position ReadJson(JsonReader reader, Type objectType, Position existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            string position = serializer.Deserialize<string>(reader);
            return PositionHelper.FromString(position);
        }

        public override void WriteJson(JsonWriter writer, Position value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
