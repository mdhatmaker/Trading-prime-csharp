using System;
using System.Collections.Generic;
using System.Linq;
using HitBtcApi.Model;
using Newtonsoft.Json;

namespace HitBtcApi
{
    internal static class Utilities
    {
        public static string JoinNonEmpty<T>(this IEnumerable<T> collection, string separator = ",")
        {
            if (collection == null)
            {
                return string.Empty;
            }

            return string.Join(separator, collection.Select(i => i.ToString()).Where(s => !string.IsNullOrWhiteSpace(s)).ToArray());
        }

        public static T ConverFromJason<T>(ApiResponse response) where T : class, new()
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(response.content);
            }
            catch (Exception)
            {
                return new T();
            }
        }

        public static Dictionary<string, Ticker> ConverFromJasonArray(ApiResponse response)
        {
            Dictionary<string, Ticker> result = new Dictionary<string, Ticker>();
            try
            {
                int length = response.content.Trim().Length;
                var tickers = response.content.Trim()
                    .Remove(length - 1)
                    .Remove(0, 1)
                    .Split(new string[] { "}," }, StringSplitOptions.None).Select(x => x[x.Length - 1] != '}' ? x + "}" : x).ToList();

                foreach (var ticker in tickers)
                {
                    int index = ticker.IndexOf(':');
                    string name = ticker.Remove(index, ticker.Length - index);
                    string data = ticker.Remove(0, index + 1);
                    result.Add(name, JsonConvert.DeserializeObject<Ticker>(data));
                }
            }
            catch (Exception)
            {
            }
            return result;
        }
    }
}
