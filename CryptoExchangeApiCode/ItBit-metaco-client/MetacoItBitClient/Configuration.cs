using System;
using System.Configuration;

namespace Metaco.ItBit
{
	internal static class Configuration
	{
		public static Uri ApiUrl { 
			get
			{
				var itBitApiUrl = ConfigurationManager.AppSettings["itbit-rest-api-baseurl"];
				return new Uri(itBitApiUrl, UriKind.RelativeOrAbsolute);
			}
		}
	}
}