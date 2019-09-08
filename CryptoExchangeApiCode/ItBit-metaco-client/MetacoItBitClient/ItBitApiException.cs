using System;
using System.Net;

namespace Metaco.ItBit
{
	[Serializable]
	public class ItBitApiException : Exception
	{
		public Error Error { get; private set; }
		public HttpStatusCode HttpStatusCode { get; set; }
		public string Reason { get; set; }

		public ItBitApiException(Error error, HttpStatusCode httpStatusCode, string reason)
			:base(error.Description)
		{
			Error = error;
			HttpStatusCode = httpStatusCode;
			Reason = reason;
		}
	}
}