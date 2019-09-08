using RestSharp;
using System;
using System.Security.Cryptography;
using System.Text;

namespace Rokolab.BitstampClient
{
    public class RequestAuthenticator : IRequestAuthenticator
    {
        private readonly string _clientId;
        private readonly string _apiKey;
        private readonly string _apiSecret;
        private long _nonce;
        private object _lock = new object();

        public RequestAuthenticator(string apiKey, string apiSecret, string clientId)
        {
            _nonce = UnixTimeStampUtc();
            _clientId = clientId;
            _apiKey = apiKey;
            _apiSecret = apiSecret;
        }

        public void Authenticate(RestRequest request)
        {
            lock (_lock)
            {
                string nonce = _nonce.ToString();
                request.AddParameter("key", _apiKey);
                request.AddParameter("nonce", nonce);
                request.AddParameter("signature", CreateSignature(nonce));
                _nonce++;
            }
        }

        private string CreateSignature(string nonce)
        {
            var msg = string.Format("{0}{1}{2}", nonce, _clientId, _apiKey);
            return ByteArrayToString(SignHMACSHA256(_apiSecret, StringToByteArray(msg))).ToUpper();
        }

        private static byte[] SignHMACSHA256(string key, byte[] data)
        {
            var hashMaker = new HMACSHA256(Encoding.ASCII.GetBytes(key));
            return hashMaker.ComputeHash(data);
        }

        private static byte[] StringToByteArray(string str)
        {
            return Encoding.ASCII.GetBytes(str);
        }

        private static string ByteArrayToString(byte[] hash)
        {
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }

        private static long UnixTimeStampUtc()
        {
            int unixTimeStamp;
            var currentTime = DateTime.Now;
            var dt = currentTime.ToUniversalTime();
            var unixEpoch = new DateTime(1970, 1, 1);
            unixTimeStamp = (Int32)(dt.Subtract(unixEpoch)).TotalSeconds;
            return unixTimeStamp;
        }

        private static double GetTimeStamp(DateTime dt)
        {
            var unixEpoch = new DateTime(1970, 1, 1);
            return dt.Subtract(unixEpoch).TotalSeconds;
        }
    }
}