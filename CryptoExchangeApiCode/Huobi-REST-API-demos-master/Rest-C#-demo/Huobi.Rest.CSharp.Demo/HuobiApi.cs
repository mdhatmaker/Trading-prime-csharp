using Huobi.Rest.CSharp.Demo.Model;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web;
/// <summary>
/// GitHub:https://github.com/CryptocurrencyToolKits/Huobi.Rest.CSharp.Demo
/// </summary>
namespace Huobi.Rest.CSharp.Demo
{
    public class HuobiApi
    {

        #region HuoBiApi配置信息
        /// <summary>
        /// API域名名称
        /// </summary>
        private readonly string HUOBI_HOST = string.Empty;
        /// <summary>
        /// APi域名地址
        /// </summary>
        private readonly string HUOBI_HOST_URL = string.Empty;
        /// <summary>
        /// 加密方法
        /// </summary>
        private const string HUOBI_SIGNATURE_METHOD = "HmacSHA256";
        /// <summary>
        /// API版本
        /// </summary>
        private const int HUOBI_SIGNATURE_VERSION = 2;
        /// <summary>
        /// ACCESS_KEY
        /// </summary>
        private readonly string ACCESS_KEY = string.Empty;
        /// <summary>
        /// SECRET_KEY()
        /// </summary>
        private readonly string SECRET_KEY = string.Empty;
        #endregion

        #region HuoBiApi接口地址
        private const string API_ACCOUNBT_BALANCE = "/v1/account/accounts/{0}/balance";
        private const string API_ACCOUNBT_ALL = "/v1/account/accounts";
        private const string API_ORDERS_PLACE = "/v1/order/orders/place";
        #endregion

        #region 构造函数
        private RestClient client;//http请求客户端
        public HuobiApi(string accessKey, string secretKey, string huobi_host = "api.huobi.pro")
        {
            ACCESS_KEY = accessKey;
            SECRET_KEY = secretKey;
            HUOBI_HOST = huobi_host;
            HUOBI_HOST_URL = "https://" + HUOBI_HOST;
            if (string.IsNullOrEmpty(ACCESS_KEY))
                throw new ArgumentException("ACCESS_KEY Cannt Be Null Or Empty");
            if (string.IsNullOrEmpty(SECRET_KEY))
                throw new ArgumentException("SECRET_KEY  Cannt Be Null Or Empty");
            if (string.IsNullOrEmpty(HUOBI_HOST))
                throw new ArgumentException("HUOBI_HOST  Cannt Be Null Or Empty");
            client = new RestClient(HUOBI_HOST_URL);
            client.AddDefaultHeader("Content-Type", "application/json");
            client.AddDefaultHeader("User-Agent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/39.0.2171.71 Safari/537.36");
        }
        #endregion

        #region HuoBiApi方法
        public List<Account> GetAllAccount()
        {
            var result = SendRequest<List<Account>>(API_ACCOUNBT_ALL);
            return result.Data;
        }
        public HBResponse<long> OrderPlace(OrderPlaceRequest req)
        {
            var bodyParas = new Dictionary<string, string>();
            var result = SendRequest<HBResponse<long>, OrderPlaceRequest>(API_ORDERS_PLACE, req);
            return result.Data;
        }
        #endregion

        #region HTTP请求方法
        /// <summary>
        /// 发起Http请求
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="resourcePath"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private HBResponse<T> SendRequest<T>(string resourcePath, string parameters = "") where T : class, new()
        {
            parameters = UriEncodeParameterValue(GetCommonParameters() + parameters);//请求参数
            var sign = GetSignatureStr(Method.GET, HUOBI_HOST, resourcePath, parameters);//签名
            parameters += $"&Signature={sign}";

            var url = $"{HUOBI_HOST_URL}{resourcePath}?{parameters}";
            Console.WriteLine(url);
            var request = new RestRequest(url, Method.GET);
            var result = client.Execute<HBResponse<T>>(request);
            return result.Data;
        }
        private HBResponse<T> SendRequest<T, P>(string resourcePath, P postParaneters) where T : class, new()
        {
            var parameters = UriEncodeParameterValue(GetCommonParameters());//请求参数
            var sign = GetSignatureStr(Method.POST, HUOBI_HOST, resourcePath, parameters);//签名
            parameters += $"&Signature={sign}";

            var url = $"{HUOBI_HOST_URL}{resourcePath}?{parameters}";
            Console.WriteLine(url);
            var request = new RestRequest(url, Method.POST);
            request.AddObject(postParaneters);
            var result = client.Execute<HBResponse<T>>(request);
            return result.Data;
        }
        /// <summary>
        /// 获取通用签名参数
        /// </summary>
        /// <returns></returns>
        private string GetCommonParameters()
        {
            return $"AccessKeyId={ACCESS_KEY}&SignatureMethod={HUOBI_SIGNATURE_METHOD}&SignatureVersion={HUOBI_SIGNATURE_VERSION}&Timestamp={DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss")}";
        }
        /// <summary>
        /// Uri参数值进行转义
        /// </summary>
        /// <param name="parameters">参数字符串</param>
        /// <returns></returns>
        private string UriEncodeParameterValue(string parameters)
        {
            var sb = new StringBuilder();
            var paraArray = parameters.Split('&');
            var sortDic = new SortedDictionary<string, string>();
            foreach (var item in paraArray)
            {
                var para = item.Split('=');
                sortDic.Add(para.First(), UrlEncode(para.Last()));
            }
            foreach (var item in sortDic)
            {
                sb.Append(item.Key).Append("=").Append(item.Value).Append("&");
            }
            return sb.ToString().TrimEnd('&');
        }
        /// <summary>
        /// 转义字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string UrlEncode(string str)
        {
            StringBuilder builder = new StringBuilder();
            foreach (char c in str)
            {
                if (HttpUtility.UrlEncode(c.ToString(), Encoding.UTF8).Length > 1)
                {
                    builder.Append(HttpUtility.UrlEncode(c.ToString(), Encoding.UTF8).ToUpper());
                }
                else
                {
                    builder.Append(c);
                }
            }
            return builder.ToString();
        }
        /// <summary>
        /// Hmacsha256加密
        /// </summary>
        /// <param name="text"></param>
        /// <param name="secretKey"></param>
        /// <returns></returns>
        private static string CalculateSignature256(string text, string secretKey)
        {
            using (var hmacsha256 = new HMACSHA256(Encoding.UTF8.GetBytes(secretKey)))
            {
                byte[] hashmessage = hmacsha256.ComputeHash(Encoding.UTF8.GetBytes(text));
                return Convert.ToBase64String(hashmessage);
            }
        }
        /// <summary>
        /// 请求参数签名
        /// </summary>
        /// <param name="method">请求方法</param>
        /// <param name="host">API域名</param>
        /// <param name="resourcePath">资源地址</param>
        /// <param name="parameters">请求参数</param>
        /// <returns></returns>
        private string GetSignatureStr(Method method, string host, string resourcePath, string parameters)
        {
            var sign = string.Empty;
            StringBuilder sb = new StringBuilder();
            SortedDictionary<string, string> sortDic = new SortedDictionary<string, string>();
            sb.Append(method.ToString().ToUpper()).Append("\n")
                .Append(host).Append("\n")
                .Append(resourcePath).Append("\n");
            //参数排序
            var paraArray = parameters.Split('&');
            foreach (var item in paraArray)
            {
                sortDic.Add(item.Split('=').First(), item.Split('=').Last());
            }
            foreach (var item in sortDic)
            {
                sb.Append(item.Key).Append("=").Append(item.Value).Append("&");
            }
            sign = sb.ToString().TrimEnd('&');
            //计算签名，将以下两个参数传入加密哈希函数
            sign = CalculateSignature256(sign, SECRET_KEY);
            return UrlEncode(sign);
        }
        #endregion



    }
}
