using System;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;

namespace Info.Blockchain.API.Tests
{
	public static class ReflectionUtil
	{
		public static T DeserializeFile<T>(string fileName, Func<string, T> customDeserialization = null)
		{
			Assembly assembly = typeof(ReflectionUtil).GetTypeInfo().Assembly;

			string resourceName = $"Info.Blockchain.API.Tests.JsonObjects.{fileName}.json";

			using (Stream stream = assembly.GetManifestResourceStream(resourceName))
			{
				if (stream == null)
				{
					throw new Exception($"Embedded resource with the name {resourceName} does not exist.");
				}
				using (StreamReader reader = new StreamReader(stream))
				{
					string json = reader.ReadToEnd();
					return customDeserialization == null ? JsonConvert.DeserializeObject<T>(json) : customDeserialization(json);
				}
			}
		}
	}
}
