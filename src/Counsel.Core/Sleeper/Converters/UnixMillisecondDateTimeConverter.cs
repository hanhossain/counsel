using System;
using Newtonsoft.Json;

namespace Counsel.Core.Sleeper.Converters
{
	public class UnixMillisecondDateTimeConverter : JsonConverter
	{
		private static readonly DateTime _epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(DateTime) || objectType == typeof(DateTime?);
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			if (reader.Value == null)
			{
				return null;
			}

			var msSinceEpoch = (long)reader.Value;
			return _epoch.AddMilliseconds(msSinceEpoch);
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			if (value != null)
			{
				DateTime timestamp = (DateTime) value;

				var msSinceEpoch = (timestamp - _epoch).TotalMilliseconds;
				writer.WriteValue(msSinceEpoch);
			}
		}
	}
}
