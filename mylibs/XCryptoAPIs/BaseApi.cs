using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using RestSharp;
using Tools;
using Newtonsoft.Json;
using System.Globalization;
using System.Net;
using System.IO;
using static Tools.G;

namespace CryptoAPIs
{
    public abstract class BaseApi
    {
        public abstract string BaseUrl { get; }
        public abstract string Name { get; }

        protected string ApiKey { get; set; }
        protected string ApiSecret { get; set; }

        protected System.Diagnostics.Stopwatch m_stopWatch = new System.Diagnostics.Stopwatch();


        public BaseApi()
        {
             //m_restClient = new RestClient(BaseUrl);
        }

    } // end of abstract class BaseApi

} // end of namespace
