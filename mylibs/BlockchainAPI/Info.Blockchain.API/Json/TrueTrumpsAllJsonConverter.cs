using System;
using Newtonsoft.Json;

namespace Info.Blockchain.API.Json
{
	internal class TrueTrumpsAllJsonConverter : JsonConverter
	{
		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(bool);
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			//value always true if true already
			if(existingValue is bool && (bool)existingValue)
			{
				return true;
			}
			if (reader.Value is bool)
			{
				return (bool)reader.Value;
			}
			return false;
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			writer.WriteValue(value);
		}
	}
}
