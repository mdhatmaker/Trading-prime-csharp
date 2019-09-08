using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net.WebSockets;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using CryptoApis.SharedModels;
using CryptoApis.RestApi;
using GDAXSharp.Network.Authentication;
using GDAXSharp.Shared.Types;
//using CryptoTools.Cryptography;
//using static CryptoTools.Cryptography.Cryptography;

namespace Aggregator
{
    public class TradeGdax
    {
        //static Credentials m_creds;
        static GdaxRestApi m_api;

        static TradeGdax()
        {
            //SetTradeSizes(1.0M);
            //InitializeArbPositions();
        }

        public static void InitializeApi(string encryptedCredentialsFile, string password)
        {
            //m_creds = Credentials.LoadFromFile(encryptedCredentialsFile, password);
            //var cred = m_creds["GDAX"];
            //m_api = new GdaxApi(cred.Key, cred.Secret, "mickey+mouse");
            //m_api = GdaxRestApi.Create(encryptedCredentialsFile, password);
            //m_api = ApiFactory.Get("GDAX") as GdaxRestApi;
        }

        public static void Test()
        {
            //var task = m_api.Test();
            //task.Wait();                // Blocks current thread until GetFooAsync task completes

            var prodtype = ProductType.BtcUsd;
            var taskTicker = m_api.GetTicker(prodtype);
            taskTicker.Wait();

            var ticker = taskTicker.Result;

            Console.WriteLine("{0} b:{1} a:{2}", prodtype.ToString(), ticker.Bid, ticker.Ask);
            //var result = task.Result;
            //return result;
        }

    } // end of class TradeGdax

} // end of namespace

