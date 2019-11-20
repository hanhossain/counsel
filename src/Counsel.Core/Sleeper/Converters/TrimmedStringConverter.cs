using System;
using Newtonsoft.Json;

namespace Counsel.Core.Sleeper.Converters
{
	public class TrimmedStringConverter : JsonConverter
	{
		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(string);
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			if (reader.TokenType != JsonToken.String)
			{
				return null;
			}

			return (reader.Value as string)?.Trim();
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			throw new NotImplementedException();
		}
	}
}
