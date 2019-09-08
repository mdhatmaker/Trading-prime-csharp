using System;
using System.Net;
using System.Net.Http;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Tools
{
    public static class G
    {
        public static T GET<T>(string unformattedURL, params object[] p) where T : new()
        {
            string url;
            if (p.Length == 0)
                url = unformattedURL;
            else
                url = string.Format(unformattedURL, p);
            
            using (var webClient = new WebClient())
            {
                var jsonData = string.Empty;
                try
                {
                    jsonData = webClient.DownloadString(url);
                }
                catch (Exception ex) { Console.WriteLine(ex.Message); }
                return !string.IsNullOrEmpty(jsonData)
                           ? JsonConvert.DeserializeObject<T>(jsonData)
                           : new T();
            }
        }

        public static T Deserialize<T>(string jsonData)
        {
            return JsonConvert.DeserializeObject<T>(jsonData);
        }

        /*public static T GetJsonObject<T>(string url) where T : new()
        {
            string jsonData = GET(url);
            return !string.IsNullOrEmpty(jsonData)
                           ? JsonConvert.DeserializeObject<T>(jsonData)
                           : new T();
        }

        public static string GET(string url)
        {
            string html = string.Empty;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            //request.AutomaticDecompression = DecompressionMethods.GZip;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                html = reader.ReadToEnd();
            }
            return html;
        }*/

        public static string GetJSONWithDecompression(string baseUrl, string command)
        {
            string result = "";
            //Console.WriteLine("Making API Call...");
            using (var client = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate }))
            {
                client.BaseAddress = new Uri(baseUrl);
                HttpResponseMessage response = client.GetAsync(command).Result;
                response.EnsureSuccessStatusCode();
                result = response.Content.ReadAsStringAsync().Result;
                //Console.WriteLine("Result: " + result);
            }
            return result;
        }

        public static void cout(string format, params object[] obj)
        {
            Console.WriteLine(format, obj);
        }

        public static void cout(object obj)
        {
            var jsonString = JsonConvert.SerializeObject(obj, Formatting.Indented, new JsonConverter[] { new StringEnumConverter() });
            Console.WriteLine(jsonString);
        }

        public static void cout<T>(List<T> list)
        {
            foreach (var x in list)
                cout(x);
        }

        public static string LoadFile(string pathname)
        {
            string line;

            using (StreamReader reader = new StreamReader(pathname))
            {
                line = reader.ReadToEnd();
            }
            return line;
        }

    } // end of class
} // end of namespace
