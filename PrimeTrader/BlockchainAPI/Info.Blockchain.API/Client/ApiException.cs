using System;
using System.Net;

namespace Info.Blockchain.API.Client
{
	/// <summary>
	/// The base exception for the BlockChain Api. Its only use is to detect if the
	/// exception came from the api rather that another source
	/// </summary>
	public abstract class ApiExceptionBase : Exception
	{
		protected ApiExceptionBase(string message)
			: base(message)
		{
		}
	}

	/// <summary>
	/// The exception that is thrown when an error happens in the code for the Blockchain Api
	/// library, not the on the server that the code calls
	/// </summary>
	public class ClientApiException : ApiExceptionBase
	{
		public ClientApiException(string message) : base(message)
		{
		}
	}

	/// <summary>
	/// The class `ApiException` represents a failed call to the Blockchain API. Whenever
	/// the server is unable to process a request (usually due to parameter validation errors),
	/// an instance of this class is thrown.
	/// </summary>
	public class ServerApiException : ApiExceptionBase
	{
		public HttpStatusCode StatusCode { get; }
		public ServerApiException(string message, HttpStatusCode statusCode = HttpStatusCode.InternalServerError) : base(message)
		{
			this.StatusCode = statusCode;
		}
	}
}
