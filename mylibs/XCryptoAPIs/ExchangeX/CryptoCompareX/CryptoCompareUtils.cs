using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace CryptoAPIs.ExchangeX.CryptoCompareX
{

    public static class DateTimeExtensions
    {
        private static readonly DateTimeOffset epoch = new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero);

        /// <summary>
        /// Convert a Unix tick to a <see cref="DateTimeOffset"/> with UTC offset
        /// </summary>
        /// <param name="unixTime">UTC tick</param>
        public static DateTimeOffset FromUnixTime(this long unixTime)
        {
            return epoch.AddSeconds(unixTime);
        }

        /// <summary>
        /// Convert <see cref="DateTimeOffset"/> with UTC offset to a Unix tick
        /// </summary>
        /// <param name="date">Date Time with UTC offset</param>
        public static long ToUnixTime(this DateTimeOffset date)
        {
            return Convert.ToInt64((date.ToUniversalTime() - epoch).TotalSeconds);
        }
    }

    internal static class EnumerableExtensions
    {
        public static string ToJoinedList([NotNull] this IEnumerable<string> list)
        {
            Check.NotEmpty(list, nameof(list));
            return string.Join(",", list);
        }
    }

    public static class UriExtensions
    {
        public static Uri ApplyParameters(this Uri uri, IDictionary<string, string> parameters)
        {
            Ensure.ArgumentNotNull(uri, "uri");

            if (parameters == null || !parameters.Any()) return uri;

            // to prevent values being persisted across requests
            // use a temporary dictionary which combines new and existing parameters
            IDictionary<string, string> p = new Dictionary<string, string>(parameters);

            string queryString;
            if (uri.IsAbsoluteUri)
            {
                queryString = uri.Query;
            }
            else
            {
                var hasQueryString = uri.OriginalString.IndexOf("?", StringComparison.Ordinal);
                queryString = hasQueryString == -1
                    ? ""
                    : uri.OriginalString.Substring(hasQueryString);
            }

            var values = queryString.Replace("?", "")
                                    .Split(new[] { '&' }, StringSplitOptions.RemoveEmptyEntries);

            var existingParameters = values.ToDictionary(
                        key => key.Substring(0, key.IndexOf('=')),
                        value => value.Substring(value.IndexOf('=') + 1));

            foreach (var existing in existingParameters)
            {
                if (!p.ContainsKey(existing.Key))
                {
                    p.Add(existing);
                }
            }

            Func<string, string, string> mapValueFunc = (key, value) =>
            {
                if (key == "q") return value;
                return Uri.EscapeDataString(value);
            };

            string query = String.Join("&", p.Select(kvp => kvp.Key + "=" + mapValueFunc(kvp.Key, kvp.Value)));
            if (uri.IsAbsoluteUri)
            {
                var uriBuilder = new UriBuilder(uri)
                {
                    Query = query
                };
                return uriBuilder.Uri;
            }

            return new Uri(uri + "?" + query, UriKind.Relative);
        }
    } // end of class UriExtensions


    /// <summary>
    ///   Ensure input parameters
    /// </summary>
    internal static class Ensure
    {
        /// <summary>
        /// Checks an argument to ensure it isn't null.
        /// </summary>
        /// <param name = "value">The argument value to check</param>
        /// <param name = "name">The name of the argument</param>
        public static void ArgumentNotNull([ValidatedNotNull]object value, string name)
        {
            if (value != null) return;

            throw new ArgumentNullException(name);
        }

        /// <summary>
        /// Checks a string argument to ensure it isn't null or empty.
        /// </summary>
        /// <param name = "value">The argument value to check</param>
        /// <param name = "name">The name of the argument</param>
        public static void ArgumentNotNullOrEmptyString([ValidatedNotNull]string value, string name)
        {
            ArgumentNotNull(value, name);
            if (!string.IsNullOrWhiteSpace(value)) return;

            throw new ArgumentException("String cannot be empty", name);
        }
    }

    [AttributeUsage(AttributeTargets.Parameter)]
    internal sealed class ValidatedNotNullAttribute : Attribute
    {
    }


    public class IsoDateTimeWithFormatConverter : IsoDateTimeConverter
    {
        public IsoDateTimeWithFormatConverter(string format)
        {
            this.DateTimeFormat = format;
        }
    }

    internal class StringToSubConverter : JsonConverter
    {
        /// <summary>
        /// Determines whether this instance can convert the specified object type.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <returns>
        /// <c>true</c> if this instance can convert the specified object type; otherwise,
        /// <c>false</c>.
        /// </returns>
        /// <seealso cref="M:Newtonsoft.Json.JsonConverter.CanConvert(Type)"/>
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(IReadOnlyList<Sub>) || objectType == typeof(Sub);
        }

        /// <summary>
        /// Reads the JSON representation of the object.
        /// </summary>
        /// <param name="reader">The <see cref="T:Newtonsoft.Json.JsonReader" /> to read from.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="existingValue">The existing value of object being read.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <returns>
        /// The object value.
        /// </returns>
        /// <seealso cref="M:Newtonsoft.Json.JsonConverter.ReadJson(JsonReader,Type,object,JsonSerializer)"/>
        public override object ReadJson(
            JsonReader reader,
            Type objectType,
            object existingValue,
            JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.String)
            {
                return this.GetTokenFromString(reader.Value.ToString());
            }

            if (reader.TokenType == JsonToken.StartArray)
            {
                var tokens = JArray.Load(reader);
                if (tokens?.HasValues ?? false)
                {
                    return tokens.Values().Select(token => this.GetTokenFromString(token.ToString())).ToList();
                }
            }

            return null;
        }

        /// <summary>
        /// Writes the JSON representation of the object.
        /// </summary>
        /// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter" /> to write to.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <seealso cref="M:Newtonsoft.Json.JsonConverter.WriteJson(JsonWriter,object,JsonSerializer)"/>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        private Sub GetTokenFromString(string token)
        {
            var values = token.Split('~');
            if (values.Length == 4)
            {
                Enum.TryParse(values.ElementAtOrDefault(0), out SubId subId);
                return new Sub(
                    values.ElementAtOrDefault(1),
                    values.ElementAtOrDefault(2),
                    subId,
                    values.ElementAtOrDefault(3));
            }
            return default(Sub);
        }
    }

    internal class UnixTimeConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(DateTime) || objectType == typeof(DateTime?)
                                                  || objectType == typeof(DateTimeOffset)
                                                  || objectType == typeof(DateTimeOffset?);
        }

        public override object ReadJson(
            JsonReader reader,
            Type objectType,
            object existingValue,
            JsonSerializer serializer)
        {
            if (string.IsNullOrWhiteSpace(reader.Value?.ToString()))
            {
                return null;
            }

            return Convert.ToInt64(reader.Value).FromUnixTime();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }

    internal static class ApiUrls
    {
        private const string RateLimitsUrl = "/stats/rate/{0}/limit";

        public static readonly Uri MinApiEndpoint = new Uri(
            "https://min-api.cryptocompare.com/data/",
            UriKind.Absolute);

        public static readonly Uri SiteApiEndpoint = new Uri(
            "https://www.cryptocompare.com/api/data/",
            UriKind.Absolute);

        public static Uri AllCoins() => new Uri(MinApiEndpoint, "all/coinlist");

        public static Uri AllExchanges() => new Uri(MinApiEndpoint, "all/exchanges");

        public static Uri CoinSnapshot([NotNull] string fsym, [NotNull] string tsym)
        {
            Check.NotNull(tsym, nameof(tsym));
            Check.NotNull(fsym, nameof(fsym));
            return new Uri(SiteApiEndpoint, "coinsnapshot").ApplyParameters(
                new Dictionary<string, string>
                {
                    { nameof(fsym), fsym },
                    { nameof(tsym), tsym }
                });
        }

        public static Uri CoinSnapshotFull(int id)
        {
            return new Uri(SiteApiEndpoint, "coinsnapshotfullbyid").ApplyParameters(
                new Dictionary<string, string>
                {
                    { nameof(id), id.ToString() }
                });
        }

        public static Uri History(
            string method,
            string fsym,
            string tsym,
            int? limit,
            string e,
            DateTimeOffset? toTs,
            bool? allData,
            int? aggregate,
            bool? tryConversion)
        {
            Check.NotNullOrWhiteSpace(fsym, nameof(fsym));
            Check.NotNullOrWhiteSpace(tsym, nameof(tsym));

            return new Uri(MinApiEndpoint, $"histo{method}").ApplyParameters(
                new Dictionary<string, string>
                {
                    { nameof(fsym), fsym },
                    { nameof(tsym), tsym },
                    { nameof(limit), limit.ToString() },
                    {
                        nameof(toTs),
                        toTs?.ToUnixTime().ToString(CultureInfo.InvariantCulture)
                    },
                    { nameof(tryConversion), tryConversion?.ToString() },
                    { nameof(e), e },
                    { nameof(allData), allData?.ToString() },
                    { nameof(aggregate), aggregate?.ToString() }
                });
        }

        public static Uri PriceAverage(
            [NotNull] string fsym,
            [NotNull] string tsym,
            [NotNull] IEnumerable<string> e,
            bool? tryConversion)
        {
            Check.NotNullOrWhiteSpace(fsym, nameof(fsym));
            Check.NotNullOrWhiteSpace(tsym, nameof(tsym));
            Check.NotEmpty(e, nameof(e));

            return new Uri(MinApiEndpoint, "generateAvg").ApplyParameters(
                new Dictionary<string, string>
                {
                    { nameof(fsym), fsym },
                    { nameof(tsym), tsym },
                    { nameof(e), e?.ToJoinedList() },
                    { nameof(tryConversion), tryConversion?.ToString() }
                });
        }

        public static Uri PriceHistorical(
            string fsym,
            IEnumerable<string> tsyms,
            IEnumerable<string> markets,
            DateTimeOffset ts,
            CalculationType? calculationType,
            bool? tryConversion)
        {
            return new Uri(MinApiEndpoint, "pricehistorical").ApplyParameters(
                new Dictionary<string, string>
                {
                    { nameof(fsym), fsym },
                    { nameof(tsyms), tsyms.ToJoinedList() },
                    {
                        nameof(ts),
                        ts.ToUnixTime().ToString(CultureInfo.InvariantCulture)
                    },
                    { nameof(markets), markets?.ToJoinedList() },
                    { nameof(calculationType), calculationType?.ToString("G") },
                    { nameof(tryConversion), tryConversion?.ToString() }

                });
        }

        public static Uri PriceMulti(
            [NotNull] IEnumerable<string> fsyms,
            [NotNull] IEnumerable<string> tsyms,
            bool? tryConversion,
            string e)
        {
            Check.NotEmpty(fsyms, nameof(fsyms));
            Check.NotEmpty(tsyms, nameof(tsyms));

            return new Uri(MinApiEndpoint, "pricemulti").ApplyParameters(
                new Dictionary<string, string>
                {
                    { nameof(fsyms), fsyms.ToJoinedList() },
                    { nameof(tsyms), tsyms.ToJoinedList() },
                    { nameof(tryConversion), tryConversion?.ToString() },
                    { nameof(e), e }
                });
        }

        public static Uri PriceMultiFull(
            IEnumerable<string> fsyms,
            IEnumerable<string> tsyms,
            bool? tryConversion,
            string e)
        {
            Check.NotEmpty(fsyms, nameof(fsyms));
            Check.NotEmpty(tsyms, nameof(tsyms));

            return new Uri(MinApiEndpoint, "pricemultifull").ApplyParameters(
                new Dictionary<string, string>
                {
                    { nameof(fsyms), fsyms.ToJoinedList() },
                    { nameof(tsyms), tsyms.ToJoinedList() },
                    { nameof(tryConversion), tryConversion?.ToString() },
                    { nameof(e), e }
                });
        }

        public static Uri PriceSingle(
            [NotNull] string fsym,
            [NotNull] IEnumerable<string> tsyms,
            bool? tryConversion,
            string e)
        {
            Check.NotNullOrWhiteSpace(fsym, nameof(fsym));
            Check.NotEmpty(tsyms, nameof(tsyms));

            return new Uri(MinApiEndpoint, "price").ApplyParameters(
                new Dictionary<string, string>
                {
                    { nameof(fsym), fsym },
                    { nameof(tsyms), tsyms.ToJoinedList() },
                    { nameof(tryConversion), tryConversion?.ToString() },
                    { nameof(e), e }
                });
        }

        public static Uri RateLimitsByHour() => new Uri(MinApiEndpoint, string.Format(RateLimitsUrl, "hour"));

        public static Uri RateLimitsByMinute() => new Uri(MinApiEndpoint, string.Format(RateLimitsUrl, "minute"));

        public static Uri RateLimitsBySecond() => new Uri(MinApiEndpoint, string.Format(RateLimitsUrl, "second"));

        public static Uri SubsList([NotNull] string fsym, [NotNull] IEnumerable<string> tsyms)
        {
            Check.NotEmpty(tsyms, nameof(tsyms));
            Check.NotNull(fsym, nameof(fsym));
            return new Uri(MinApiEndpoint, "subs").ApplyParameters(
                new Dictionary<string, string>
                {
                    { nameof(fsym), fsym },
                    { nameof(tsyms), tsyms.ToJoinedList() }
                });
        }

        public static Uri TopExchanges([NotNull] string fsym, [NotNull] string tsym, int? limit)
        {
            Check.NotNullOrWhiteSpace(tsym, nameof(tsym));
            Check.NotNullOrWhiteSpace(fsym, nameof(fsym));
            return new Uri(MinApiEndpoint, "top/exchanges").ApplyParameters(
                new Dictionary<string, string>
                {
                    { nameof(fsym), fsym },
                    { nameof(tsym), tsym },
                    { nameof(limit), limit.ToString() }
                });
        }

        public static Uri TopPairs([NotNull] string fsym, int? limit)
        {
            Check.NotNullOrWhiteSpace(fsym, nameof(fsym));
            return new Uri(MinApiEndpoint, "top/pairs").ApplyParameters(
                new Dictionary<string, string>
                {
                    { nameof(fsym), fsym },
                    { nameof(limit), limit.ToString() }
                });
        }

        public static Uri TopVolumes([NotNull] string tsym, int? limit)
        {
            Check.NotNullOrWhiteSpace(tsym, nameof(tsym));
            return new Uri(MinApiEndpoint, "top/volumes").ApplyParameters(
                new Dictionary<string, string>
                {
                    { nameof(tsym), tsym },
                    { nameof(limit), limit.ToString() }
                });
        }


        public static Uri NewsProviders()
        {
            return new Uri(MinApiEndpoint, "news/providers");
        }

        public static Uri News(string lang = null, long? lTs = null, string[] feeds = null, bool? sign = null)
        {
            return new Uri(MinApiEndpoint, "news/").ApplyParameters(new Dictionary<string, string>
            {
                {nameof(lang),lang },
                {nameof(lTs),lTs?.ToString() },
                {nameof(feeds),feeds != null ? string.Join(",", feeds) : null },
                {nameof(sign),sign?.ToString() },
            });
        }

        public static Uri SocialStats([NotNull] int id)
        {
            Check.NotNull(id, nameof(id));
            return new Uri(SiteApiEndpoint, "socialstats").ApplyParameters(
                new Dictionary<string, string>
                {
                    { nameof(id), id.ToString() }
                });
        }
    }

    internal static class Constants
    {
        public const string ResponseErrorStatus = "Error";
    }

    /// <summary>
    /// Global Exception for signalling cryptocompare api errors.
    /// </summary>
    /// <seealso cref="T:System.Exception"/>
    public class CryptoCompareException : HttpRequestException
    {
        /// <summary>
        /// Initializes a new instance of the CryptoCompare.Exceptions.CryptoCompareException
        /// class.
        /// </summary>
        /// <param name="apiResponse">Reason of api failure.</param>
        public CryptoCompareException(BaseApiResponse apiResponse)
            : base(FormatErrorMessage(apiResponse))
        {
        }

        /// <summary>
        /// Initializes a new instance of the CryptoCompare.Exceptions.CryptoCompareException
        /// class.
        /// </summary>
        /// <param name="apiResponse">Reason of api failure.</param>
        /// <param name="innerException">The inner exception.</param>
        public CryptoCompareException(BaseApiResponse apiResponse, Exception innerException)
            : base(FormatErrorMessage(apiResponse), innerException)
        {
        }

        private static string FormatErrorMessage(BaseApiResponse apiResponse)
        {
            var reason = string.Empty;

            if (apiResponse != null)
            {
                reason =
                    $"{apiResponse.StatusType} : {apiResponse.StatusMessage} {apiResponse.ErrorsSummary} {apiResponse.Path}";
            }

            return reason;
        }
    }

    /// <summary>
    /// Null checking utilities.
    /// </summary>
    [DebuggerStepThrough]
    internal static class Check
    {
        /// <summary>
        /// Checks null enmerable.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown when one or more arguments have unsupported or
        /// illegal values.</exception>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="value">The value.</param>
        /// <param name="parameterName">Name of the parameter. This cannot be null.</param>
        [ContractAnnotation("value:null => halt")]
        public static IEnumerable<T> NotEmpty<T>(
            IEnumerable<T> value,
            [InvokerParameterName] [NotNull] string parameterName)
        {
            NotNull(value, parameterName);

            if (!value.Any())
            {
                NotNullOrWhiteSpace(parameterName, nameof(parameterName));
                throw new ArgumentException(parameterName);
            }

            return value;
        }

        /// <summary>
        /// Checks null arguments.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when one or more required arguments are null.</exception>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="value">The value.</param>
        /// <param name="parameterName">Name of the parameter. This cannot be null.</param>
        /// <returns>
        /// Checked object.
        /// </returns>
        [ContractAnnotation("value:null => halt")]
        public static T NotNull<T>(T value, [InvokerParameterName] [NotNull] string parameterName)
        {
            if (ReferenceEquals(value, null))
            {
                NotNullOrWhiteSpace(parameterName, nameof(parameterName));
                throw new ArgumentNullException(parameterName);
            }
            return value;
        }

        /// <summary>
        /// Checks null or white space string arguments.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when one or more required arguments are null.</exception>
        /// <param name="value">The value.</param>
        /// <param name="parameterName">Name of the parameter. This cannot be null.</param>
        /// <returns>
        /// Checked object.
        /// </returns>
        [ContractAnnotation("value:null => halt")]
        public static string NotNullOrWhiteSpace(string value, [InvokerParameterName] [NotNull] string parameterName)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                NotNullOrWhiteSpace(parameterName, nameof(parameterName));
                throw new ArgumentNullException(parameterName);
            }
            return value;
        }
    }



} // end of namespace
