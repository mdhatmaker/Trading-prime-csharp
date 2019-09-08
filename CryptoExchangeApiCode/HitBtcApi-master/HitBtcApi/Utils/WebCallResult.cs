using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HitBtcApi.Utils
{
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

        public void LoadResultTo(HtmlDocument htmlDocument)
        {
            htmlDocument.LoadHtml(Response);
        }
    }
}
