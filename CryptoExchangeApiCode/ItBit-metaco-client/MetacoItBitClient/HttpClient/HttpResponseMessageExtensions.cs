using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;

namespace Metaco.ItBit
{
	public static class HttpResponseMessageExtensions
	{
		internal static async Task<T> ReadAsAsync<T>(this Task<HttpResponseMessage> task)
		{
			var response = await task;
			return await response.ReadAsAsync<T>(new MediaTypeFormatter[] {new JsonMediaTypeFormatter()});
		}

		internal static async Task<TResult> ReadAsAsync<TResult, TMediaTypeFormatter>(this Task<HttpResponseMessage> task) where TMediaTypeFormatter : MediaTypeFormatter, new()
		{
			var response = await task;
			return await response.ReadAsAsync<TResult>(new MediaTypeFormatter[] {new TMediaTypeFormatter()});
		}

		internal static async Task<T> ReadAsAsync<T>(this HttpResponseMessage response, params MediaTypeFormatter[] formatters)
		{
			if (!response.IsSuccessStatusCode)
			{
				var content = response.Content;
				var mediaTypes = new MediaTypeFormatter[] { new JsonMediaTypeFormatter() };
				var error = response.StatusCode == (HttpStatusCode)422
					? await content.ReadAsAsync<ValidationsError>(mediaTypes)
					: await content.ReadAsAsync<Error>(mediaTypes);
				throw new ItBitApiException(error, response.StatusCode, response.ReasonPhrase);
			}

			return await response.Content.ReadAsAsync<T>(formatters);
		}

	}
}
