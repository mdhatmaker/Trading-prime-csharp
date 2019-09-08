using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CryptoAPIs.Exchange.Clients.HitBTC
{
    public class Errors
    {
        //403   Invalid API key API key doesn’t exist or API key is currently used on another endpoint(max last 15 min)
        //403   Nonce has been used     Nonce is not monotonous
        //403   Nonce is not valid  Too big number or not a number
        //403   Wrong signature     Specified signature is not correct
        //500   Internal error  Internal error.Try again later
    }

    public class Browser
    {
        /// <summary>
        /// Адрес хоста
        /// </summary>
        private string _host;
        /// <summary>
        /// Порт
        /// </summary>
        private int? _port;

        /// <summary>
        /// Получение json по url-адресу
        /// </summary>
        /// <param name="url">Адрес получения json</param>
        /// <returns>Строка в формате json</returns>
        public string GetJson(string url)
        {
            var separatorPosition = url.IndexOf('?');
            var methodUrl = separatorPosition < 0 ? url : url.Substring(0, separatorPosition);
            var parameters = separatorPosition < 0 ? string.Empty : url.Substring(separatorPosition + 1);

            return WebCall.MakeCall(methodUrl, _host, _port).Response;
        }
    }

    internal sealed class Cookies
    {
        public CookieContainer Container { get; private set; }

        public Cookies()
        {
            Container = new CookieContainer();
        }

        public void AddFrom(Uri responseUrl, CookieCollection cookies)
        {
            foreach (Cookie cookie in cookies)
                Container.Add(responseUrl, cookie);

            BugFixCookieDomain();
        }

        private void BugFixCookieDomain()
        {
            var table =
                (Hashtable)
                    Container.GetType()
                        .InvokeMember("m_domainTable", BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Instance, null, Container, new object[] { });

            foreach (var key in table.Keys.OfType<string>().ToList())
            {
                if (key[0] == '.')
                {
                    var newKey = key.Remove(0, 1);
                    if (!table.ContainsKey(newKey))
                        table[newKey] = table[key];
                }
            }
        }
    }

    /// <summary>
    /// Параметры запроса к ВКонтакте.
    /// </summary>
    [Serializable]
    public partial class Parameters : Dictionary<string, string>
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="VkParameters"/>.
        /// </summary>
        public Parameters()
        {
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="VkParameters"/> на основе словаря.
        /// </summary>
        /// <param name="parameters">
        /// Параметры запроса.
        /// </param>
        public Parameters(IDictionary<string, string> parameters) : base(parameters)
        {
        }

        /// <summary>
        /// Параметры для запроса без параметров.
        /// </summary>
        public static Parameters Empty
        {
            get { return new Parameters(); }
        }

        /// <summary>
        /// Добавляет параметр запроса.
        /// </summary>
        /// <typeparam name="T">Тип значения параметра запроса.</typeparam>
        /// <param name="name">Имя параметра запроса.</param>
        /// <param name="value">Значение параметра запроса.</param>
        public void Add<T>(string name, T value)
        {
            if (value == null)
                return;

            if (typeof(T).IsEnum)
            {
                Add(name, (int)(object)value);
                return;
            }

            var stringValue = value.ToString();
            if (!string.IsNullOrEmpty(stringValue))
                base.Add(name, stringValue);
        }

        /// <summary>
        /// Добавляет параметр запроса, представляющий собой последовательность элементов заданного типа.
        /// Последовательность добавляется в виде строкового значения, содержащего ее элементы, разделенные запятой.
        /// </summary>
        /// <typeparam name="T">Имя типа элементов последовательности.</typeparam>
        /// <param name="name">Имя параметра запроса.</param>
        /// <param name="collection">Последовательность, представляющая значение параметра запроса.</param>
        public void Add<T>(string name, IEnumerable<T> collection)
        {
            var value = collection.JoinNonEmpty();
            Add(name, value);
        }

        /// <summary>
        /// Добавляет именованный параметр запроса, представляющий собой коллекцию элементов заданного типа.
        /// Коллекция добавляетсяв виде строкового значения, содержащего ее элементы, разделенные запятой.
        /// </summary>
        /// <typeparam name="T">Имя типа элементов коллекции.</typeparam>
        /// <param name="name">Имя параметра запроса.</param>
        /// <param name="collection">Коллекция, представляющая значение параметра запроса.</param>
        public void Add<T>(string name, List<T> collection)
        {
            Add(name, (IEnumerable<T>)collection);
        }

        /// <summary>
        /// Добавляет параметр запроса.
        /// Если передан null, то добавление параметра не производится.
        /// </summary>
        /// <typeparam name="T">Тип значения параметра запроса.</typeparam>
        /// <param name="name">Имя параметра запроса.</param>
        /// <param name="nullableValue">Значение параметра запроса.</param>
        public void Add<T>(string name, T? nullableValue) where T : struct
        {
            if (!nullableValue.HasValue)
                return;

            Add(name, nullableValue.Value);
        }

        /// <summary>
        /// Добавляет параметр-дату.
        /// Если передан null, то добавление не производится.
        /// </summary>
        /// <param name="name">Имя параметра запроса.</param>
        /// <param name="nullableDateTime">Значение параметра.</param>
        public void Add(string name, DateTime? nullableDateTime)
        {
            if (!nullableDateTime.HasValue)
                return;

            //var offset = DateTime.Now - nullableDateTime.Value;
            var totalSeconds = (nullableDateTime.Value.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
            var offset = Convert.ToInt64(totalSeconds);

            Add(name, offset);
        }

        /// <summary>
        /// Добавляет логический параметр.
        /// Если передан null или значение параметра false, то добавление не производится.
        /// </summary>
        /// <param name="name">Имя параметра запроса.</param>
        /// <param name="nullableValue">Значение параметра.</param>
        public void Add(string name, bool? nullableValue)
        {
            if (!nullableValue.HasValue || !nullableValue.Value)
                return;

            base.Add(name, "1");
        }

        /// <summary>
        /// Добавляет логический параметр.
        /// Если передан null, то добавление не производится.
        /// </summary>
        /// <param name="name">Имя параметра запроса.</param>
        /// <param name="value">Значение параметра.</param>
        public void Add(string name, bool value)
        {
            base.Add(name, value ? "1" : "0");
        }
    }

    public static class Parser
    {
        public static T Parse<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
            //return JsonConvert.DeserializeObject<dynamic>(json);
        }
    }

    internal sealed class WebCall
    {
        private HttpWebRequest Request { get; set; }

        private WebCallResult Result { get; set; }

        private WebCall(string url, Cookies cookies, string host = null, int? port = null)
        {
            Request = (HttpWebRequest)WebRequest.Create(url);
            Request.Accept = "text/html";
            Request.UserAgent = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.1; Trident/6.0)";
            Request.CookieContainer = cookies.Container;

            if (host != null && port != null)
                Request.Proxy = new WebProxy(host, port.Value);

            Result = new WebCallResult(url, cookies);
        }

        public static WebCallResult MakeCall(string url, string host = null, int? port = null)
        {
            var call = new WebCall(url, new Cookies(), host, port);

            return call.MakeRequest(host, port);
        }

        //public static WebCallResult PostCall(string url, string parameters, string host = null, int? port = null)
        //{
        //    var call = new WebCall(url, new Cookies(), host, port);
        //    call.Request.Method = "POST";
        //    call.Request.ContentType = "application/x-www-form-urlencoded";
        //    var data = Encoding.UTF8.GetBytes(parameters);
        //    call.Request.ContentLength = data.Length;
        //    using (var requestStream = call.Request.GetRequestStream())
        //        requestStream.Write(data, 0, data.Length);
        //    return call.MakeRequest(host, port);
        //}

        //public static WebCallResult Post(WebForm form, string host = null, int? port = null)
        //{
        //    var call = new WebCall(form.ActionUrl, form.Cookies, host, port);
        //    var request = call.Request;
        //    request.Method = "POST";
        //    request.ContentType = "application/x-www-form-urlencoded";
        //    var formRequest = form.GetRequest();
        //    request.ContentLength = formRequest.Length;
        //    request.Referer = form.OriginalUrl;
        //    request.GetRequestStream().Write(formRequest, 0, formRequest.Length);
        //    request.AllowAutoRedirect = false;
        //    return call.MakeRequest(host, port);
        //}

        private WebCallResult RedirectTo(string url, string host = null, int? port = null)
        {
            var call = new WebCall(url, Result.Cookies, host, port);

            var request = call.Request;
            request.Method = "GET";
            request.ContentType = "text/html";
            request.Referer = Request.Referer;

            return call.MakeRequest(host, port);
        }

        private WebCallResult MakeRequest(string host = null, int? port = null)
        {
            using (var response = GetResponse())
            using (var stream = response.GetResponseStream())
            {
                if (stream == null)
                    throw new System.Exception("Response is null.");

                var encoding = string.IsNullOrEmpty(response.CharacterSet) ? Encoding.UTF8 : Encoding.GetEncoding(response.CharacterSet);
                Result.SaveResponse(response.ResponseUri, stream, encoding);

                Result.SaveCookies(response.Cookies);

                if (response.StatusCode == HttpStatusCode.Redirect)
                    return RedirectTo(response.Headers["Location"], host, port);

                return Result;
            }
        }

        private HttpWebResponse GetResponse()
        {
            try
            {
                return (HttpWebResponse)Request.GetResponse();
            }
            catch (WebException ex)
            {
                throw new System.Exception(ex.Message, ex);
            }
        }
    }

    internal sealed class WebCallResult
    {
        public Uri RequestUrl { get; private set; }

        public Cookies Cookies { get; private set; }

        public Uri ResponseUrl { get; set; }

        public string Response { get; private set; }

        public WebCallResult(string url, Cookies cookies)
        {
            RequestUrl = new Uri(url);
            Cookies = cookies;
            Response = string.Empty;
        }

        public void SaveCookies(CookieCollection cookies)
        {
            Cookies.AddFrom(ResponseUrl, cookies);
        }

        public void SaveResponse(Uri responseUrl, Stream stream, Encoding encoding)
        {
            ResponseUrl = responseUrl;

            using (var reader = new StreamReader(stream, encoding))
                Response = reader.ReadToEnd();
        }

        /*public void LoadResultTo(HtmlDocument htmlDocument)
        {
            htmlDocument.LoadHtml(Response);
        }*/
    }

    /*internal sealed class WebForm
    {
        private readonly HtmlDocument _html;

        private readonly Dictionary<string, string> _inputs;

        private string _lastName;

        private readonly string _originalUrl;

        private readonly string _responceBaseUrl; // if form has relative url

        public Cookies Cookies { get; private set; }

        private WebForm(WebCallResult result)
        {
            Cookies = result.Cookies;
            _originalUrl = result.RequestUrl.OriginalString;

            _html = new HtmlDocument();
            result.LoadResultTo(_html);

            _responceBaseUrl = result.ResponseUrl.GetLeftPart(UriPartial.Authority);

            _inputs = ParseInputs();
        }

        public static WebForm From(WebCallResult result)
        {
            return new WebForm(result);
        }

        public WebForm And()
        {
            return this;
        }

        public WebForm WithField(string name)
        {
            _lastName = name;

            return this;
        }

        public WebForm FilledWith(string value)
        {
            if (string.IsNullOrEmpty(_lastName))
                throw new InvalidOperationException("Field name not set!");

            var encodedValue = System.Web.HttpUtility.UrlEncode(value);
            if (_inputs.ContainsKey(_lastName))
                _inputs[_lastName] = encodedValue;
            else
                _inputs.Add(_lastName, encodedValue);

            return this;
        }

        public string ActionUrl
        {
            get
            {
                var formNode = GetFormNode();

                if (formNode.Attributes["action"] == null)
                    return OriginalUrl;

                var link = formNode.Attributes["action"].Value;
                if (!string.IsNullOrEmpty(link) && !link.StartsWith("http")) // relative url
                {
                    link = _responceBaseUrl + link;
                }

                return link;
                // absolute path
                //return formNode.Attributes["action"] != null ? formNode.Attributes["action"].Value : OriginalUrl;
            }
        }

        public string OriginalUrl
        {
            get { return _originalUrl; }
        }

        public object HttpUtility { get; private set; }

        public byte[] GetRequest()
        {
            var uri = _inputs.Select(x => string.Format("{0}={1}", x.Key, x.Value)).JoinNonEmpty("&");
            return Encoding.UTF8.GetBytes(uri);
        }

        private Dictionary<string, string> ParseInputs()
        {
            var inputs = new Dictionary<string, string>();

            var form = GetFormNode();
            foreach (var node in form.SelectNodes("//input"))
            {
                var nameAttribute = node.Attributes["name"];
                var valueAttribute = node.Attributes["value"];

                var name = nameAttribute != null ? nameAttribute.Value : string.Empty;
                var value = valueAttribute != null ? valueAttribute.Value : string.Empty;

                if (string.IsNullOrEmpty(name))
                    continue;

                inputs.Add(name, System.Web.HttpUtility.UrlEncode(value));
            }

            return inputs;
        }

        private HtmlNode GetFormNode()
        {
            HtmlNode.ElementsFlags.Remove("form");
            var form = _html.DocumentNode.SelectSingleNode("//form");
            if (form == null)
                throw new System.Exception("Form element not found.");

            return form;
        }
    }*/

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



} // end of namespace
