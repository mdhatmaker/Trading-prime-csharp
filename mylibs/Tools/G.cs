using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
//using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Net;
using System.Net.Http;
using System.Drawing;
using System.Security.Cryptography;

namespace Tools
{
    public delegate void OutputHandler(MessageArgs e);

    public enum HashType : int { MD5, SHA1, SHA256, SHA384, SHA512 }

    public class MessageArgs : EventArgs
    {
        public string Text { get; private set; }

        public MessageArgs(string text)
        {
            this.Text = text;
        }
    } // end of class

    static public class G
    {
        static public event OutputHandler COutput;
        static public event OutputHandler DOutput;
        static public event OutputHandler ErrorOutput;

        #region Color Palettes
        private static List<Color> ColorPalette0 = new List<Color> { Color.DarkGray, Color.Cyan, Color.Green, Color.Orange, Color.DodgerBlue, Color.MediumPurple, Color.Brown, Color.Magenta, Color.Red, Color.Yellow };
        private static List<Color> ColorPalette1 = new List<Color> { Color.DarkGray, Color.Cyan, Color.Green, Color.Orange, Color.DodgerBlue, Color.MediumPurple, Color.Brown, Color.Magenta, Color.Red, Color.Yellow };
        private static List<Color> ColorPalette2 = new List<Color> { Color.DarkGray, Color.Cyan, Color.Green, Color.Orange, Color.DodgerBlue, Color.MediumPurple, Color.Brown, Color.Magenta, Color.Red, Color.Yellow };

        public static Color ColorPalette(int paletteIndex, int colorIndex)
        {
            if (paletteIndex == 2)
                return ColorPalette2[colorIndex % ColorPalette2.Count];
            else if (paletteIndex == 1)
                return ColorPalette1[colorIndex % ColorPalette1.Count];
            else
                return ColorPalette0[colorIndex % ColorPalette0.Count];     // default to using Palette zero
        }
        #endregion


        // Output to Console
        public static void cout(string text, params object[] values)
        {
            if (values.Length == 0)
            {
                Console.WriteLine(text);
                COutput?.Invoke(new MessageArgs(text));
            }
            else
            {
                Console.WriteLine(string.Format(text, values));
                COutput?.Invoke(new MessageArgs(string.Format(text, values)));
            }
        }

        public static void cout(object obj)
        {
            var jsonString = JsonConvert.SerializeObject(obj, Formatting.Indented, new JsonConverter[] { new StringEnumConverter() });
            Console.WriteLine(jsonString);
        }

        public static void cout<T>(T[] array)
        {
            cout(array.ToList<T>());
        }

        public static void cout<T>(List<T> list)
        {
            foreach (var x in list)
                cout(x);
        }

        public static void cout<T1, T2>(Dictionary<T1, T2> dict)
        {
            cout("{0} items in dictionary:", dict.Count);
            foreach (var k in dict.Keys)
            {
                cout("{0}=> {1}", k, dict[k]);
            }
        }

        // Fire COutput event (but don't actually print with Console.WriteLine)
        public static void coutFire(string text, params object[] values)
        {
            if (values.Length == 0)
            {
                G.COutput?.Invoke(new MessageArgs(text));
            }
            else
            {
                COutput?.Invoke(new MessageArgs(string.Format(text, values)));
            }
        }

        // Output to Debug
        public static void dout(string text, params object[] values)
        {
            if (values.Length == 0)
            {
                Debug.WriteLine(text);
                DOutput?.Invoke(new MessageArgs(text));
            }
            else
            {
                Debug.WriteLine(string.Format(text, values));
                DOutput?.Invoke(new MessageArgs(string.Format(text, values)));
            }
        }

        public static void dout(object obj)
        {
            var jsonString = JsonConvert.SerializeObject(obj, Formatting.Indented, new JsonConverter[] { new StringEnumConverter() });
            Debug.WriteLine(jsonString);
        }

        public static void dout<T>(T[] array)
        {
            dout(array.ToList<T>());
        }

        public static void dout<T>(List<T> list)
        {
            foreach (var x in list)
                dout(x);
        }

        // Handle error message
        public static void ErrorMessage(string message, params object[] values)
        {
            //dout("ERROR: " + message, values);
            if (message == null || string.IsNullOrWhiteSpace(string.Format(message, values))) return;  // don't print blank error lines
            Trace.WriteLine(string.Format("ERROR: " + message, values));
            string timestamp = DateTime.Now.ToString("HH:mm:ss");
            ErrorOutput?.Invoke(new MessageArgs(string.Format(timestamp + " ERROR: " + message, values)));
        }

        // Fire ErrorOutput event (but don't actually print with Debug.WriteLine or Trace.WriteLine)
        public static void errorMessageFire(string message, params object[] values)
        {
            ErrorOutput?.Invoke(new MessageArgs(string.Format("ERROR: " + message, values)));
        }

        // Deserialize a JSON string to object of type T
        public static T DeserializeJson<T>(string json)
        {
            return (T)JsonConvert.DeserializeObject(json, typeof(T));
        }

        // If you need to use a custom converter (for example a list that contains different data types)
        public static T DeserializeJson<T>(string json, JsonConverter converter)
        {
            return (T)JsonConvert.DeserializeObject<T>(json, converter);
        }

        // Create human-readable string for any object (using JSON)
        public static string Str(object obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.Indented, new JsonConverter[] { new StringEnumConverter() });
        }

        // Get a JSON object (as a string) from the given URL
        public static string GetJSON(string url, params object[] p)
        {
            string json = "";   // or "{}"?
            string formattedURL = url;
            try
            {
                if (p.Length > 0)
                    formattedURL = string.Format(url, p);
                using (WebClient wc = new WebClient())
                {
                    json = wc.DownloadString(formattedURL);
                }
            }
            catch (Exception ex)
            {
                ErrorMessage("GetJSON: Error downloading JSON from '{0}': {1}", formattedURL, ex.Message);
            }
            return json;
        }

        // Given the URL for a REST API and the Type of JSON object
        // Return a dictionary of the JSON objects indexed by string keys
        public static Dictionary<string, T> GetJsonDictionary<T>(string url)
        {
            string json = GetJSON(url);
            var res = DeserializeJson<Dictionary<string, T>>(json);
            return res;
        }

        // Given the URL for a REST API and the Type of JSON object
        // Return a list of the JSON objects
        public static List<T> GetJsonList<T>(string url)
        {
            string json = GetJSON(url);
            var res = DeserializeJson<List<T>>(json);
            return res;
        }

        // Given the URL for a REST API and the Type of JSON object
        // Return a JSON object of the given type
        public static T GetJsonObject<T>(string url)
        {
            string json = GetJSON(url);
            var res = DeserializeJson<T>(json);
            return res;
        }

        // Convert a byte array to a string of Hexadecimal
        public static string ToHexString(byte[] array)
        {
            StringBuilder hex = new StringBuilder(array.Length * 2);
            foreach (byte b in array)
            {
                hex.AppendFormat("{0:x2}", b);
            }
            return hex.ToString();
        }

        // I doubt the performance is good, but this is a super-simple way to do expression evaluation
        public static System.Data.DataTable table = new System.Data.DataTable();
        public static double Eval(String expression)
        {
            try
            {
                return Convert.ToDouble(table.Compute(expression, String.Empty));
            }
            catch (Exception ex)
            {
                dout(ex.ToString());
                dout("G::Eval failed: '{0}'", expression);
            }
            return double.NaN;
        }

        public static string GETJSON(string unformattedURL, params object[] p)
        {
            string url;
            if (p.Length == 0)
                url = unformattedURL;
            else
                url = string.Format(unformattedURL, p);

            string jsonData = string.Empty;
            using (var webClient = new WebClient())
            {
                //webClient.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
                webClient.Headers.Add("Content-Type", "application/json");
                //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;
                try
                {
                    jsonData = webClient.DownloadString(url);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("ERROR occurred retrieving JSON from '{0}' with WebClient in GETJSON method: {1}", url, ex.Message);
                }
            }
            return jsonData;
        }

        public static T DeserializeJSON<T>(string jsonData, JsonConverter[] converters = null) where T : new()
        {
            try
            {
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };
                if (converters != null) settings.Converters = converters;

                return !string.IsNullOrEmpty(jsonData)
                                   ? JsonConvert.DeserializeObject<T>(jsonData, settings)
                                   : new T();
            }
            catch (Exception ex)
            {
                ErrorMessage("{0}\nDeserializeJSON<T> method failed when deserializing jsonData: '{1}'", ex.Message, jsonData);
            }
            return new T();
        }

        public static T GET<T>(string unformattedURL, params object[] p) where T : new()
        {
            string jsonData = GETJSON(unformattedURL, p);
            return DeserializeJSON<T>(jsonData);
        }

        // Use a custom JsonConverter (passed as an argument to the method)
        public static T GET<T>(JsonConverter converter, string unformattedURL, params object[] p) where T : new()
        {
            string url;
            if (p.Length == 0)
                url = unformattedURL;
            else
                url = string.Format(unformattedURL, p);

            using (var webClient = new WebClient())
            {
                webClient.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
                var jsonData = string.Empty;
                try
                {
                    jsonData = webClient.DownloadString(url);
                }
                catch (Exception ex) {
                    Console.WriteLine(ex.Message);
                }
                return !string.IsNullOrEmpty(jsonData)
                           ? JsonConvert.DeserializeObject<T>(jsonData, converter)
                           : new T();
            }
        }

        public static T XGET<T>(Dictionary<string, string> webHeaders, string unformattedURL, params object[] p) where T : new()
        {
            string url;
            if (p.Length == 0)
                url = unformattedURL;
            else
                url = string.Format(unformattedURL, p);

            using (var webClient = new WebClient())
            {
                foreach (var k in webHeaders.Keys)
                {
                    webClient.Headers.Add(k, webHeaders[k]);
                }
                //webClient.Headers.Add("User-Agent", "GDAXClient");
                webClient.Headers.Add("Content-Type", "application/json");
                //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;
                var jsonData = string.Empty;
                try
                {
                    jsonData = webClient.DownloadString(url);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("ERROR occurred retrieving JSON from '{0}' with WebClient in GET<T> method: {1}", url, ex.Message);
                }
                try
                {
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    return !string.IsNullOrEmpty(jsonData)
                               ? JsonConvert.DeserializeObject<T>(jsonData, settings)
                               : new T();
                }
                catch (Exception ex)
                {
                    ErrorMessage("GET<T> method failed when deserializing jsonData from '{0}': {1}", url, ex.Message);
                }
                return new T();
            }
        }

        public static string GET_test(string unformattedURL, params object[] p)
        {
            string responseText = "";
            try
            {
                string url;
                if (p.Length == 0)
                    url = unformattedURL;
                else
                    url = string.Format(unformattedURL, p);
                
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.UserAgent = ".NET Framework Client";
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                var encoding = ASCIIEncoding.ASCII;
                using (var reader = new System.IO.StreamReader(response.GetResponseStream(), encoding))
                {
                    responseText = reader.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                HttpWebResponse xyz = ex.Response as HttpWebResponse;
                var encoding = ASCIIEncoding.ASCII;
                using (var reader = new System.IO.StreamReader(xyz.GetResponseStream(), encoding))
                {
                    responseText = reader.ReadToEnd();
                }
            }
            return responseText;
        }

        public static string JsonWebRequest(string url, string postData)
        {
            string ret = string.Empty;

            StreamWriter requestWriter;

            var webRequest = System.Net.WebRequest.Create(url) as HttpWebRequest;
            if (webRequest != null)
            {
                webRequest.Method = "POST";
                webRequest.ServicePoint.Expect100Continue = false;
                webRequest.Timeout = 20000;

                webRequest.ContentType = "application/json";
                // put any required HEADERS here:
                //webRequest.Headers.Add("X-API-KEY", "");
                //webRequest.Headers.Add("X-API-SECRET", "");
                //POST the data.
                using (requestWriter = new StreamWriter(webRequest.GetRequestStream()))
                {
                    requestWriter.Write(postData);
                }
            }

            HttpWebResponse resp = (HttpWebResponse)webRequest.GetResponse();
            Stream resStream = resp.GetResponseStream();
            StreamReader reader = new StreamReader(resStream);
            ret = reader.ReadToEnd();

            return ret;
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

        public static string LoadFile(string pathname)
        {
            string line;

            using (StreamReader reader = new StreamReader(pathname))
            {
                line = reader.ReadToEnd();
            }
            return line;
        }

        /*public static string Stringify(object obj)
        {
            string json = JsonConvert.SerializeObject(obj, Formatting.Indented);
            // {
            //   "Email": "james@example.com",
            //   "Active": true,
            //   "CreatedDate": "2013-01-20T00:00:00Z",
            //   "Roles": [
            //     "User",
            //     "Admin"
            //   ]
            // }
            return json;
        }*/

        public static long GetNonce()
        {
            return DateTime.Now.Ticks;
            //return DateTime.Now.Ticks * 10 / TimeSpan.TicksPerMillisecond; // use millisecond timestamp or whatever you want
            //return DateTime.Now.ToUnixTimestamp();
        }

        public static string GetNonceStr()
        {
            return DateTime.Now.ToUnixTimestamp().ToString();
        }

        public static string CalculateSignatureSHA512(string text, string secretKey)
        {
            using (var hmacsha512 = new HMACSHA512(Encoding.UTF8.GetBytes(secretKey)))
            {
                hmacsha512.ComputeHash(Encoding.UTF8.GetBytes(text));
                return string.Concat(hmacsha512.Hash.Select(b => b.ToString("x2")).ToArray()); // minimalistic hex-encoding and lower case
            }
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static string SHA256(string randomString)
        {
            System.Security.Cryptography.SHA256Managed crypt = new System.Security.Cryptography.SHA256Managed();
            System.Text.StringBuilder hash = new System.Text.StringBuilder();
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(randomString), 0, Encoding.UTF8.GetByteCount(randomString));
            foreach (byte theByte in crypto)
            {
                hash.Append(theByte.ToString("x2"));
            }
            return hash.ToString();
        }

        // Given a plain text string, a HashType and a secret key (byte[])
        // Return the string encoded with the given hash type and secret key
        // (default) secretKey is null, which means use a RANDOM key to encode
        public static string GetHash(string strPlain, HashType hshType, string secretKey=null)     //byte[] secretKey=null)
        {
            string strRet;
            byte[] secretKeyBytes = secretKey.ToBase64Bytes();
            switch (hshType)
            {
                case HashType.MD5: strRet = EncodeWithHash(strPlain, (secretKey == null ? new HMACMD5() : new HMACMD5(secretKeyBytes))); break;    // new HMACMD5(); break; // (randomly generated key if none provided)
                case HashType.SHA1: strRet = EncodeWithHash(strPlain, (secretKey == null ? new HMACSHA1() : new HMACSHA1(secretKeyBytes))); break;  // SHA1Managed(secretKey)); break;
                case HashType.SHA256: strRet = EncodeWithHash(strPlain, (secretKey == null ? new HMACSHA256() : new HMACSHA256(secretKeyBytes))); break;  // new SHA256Managed(secretKey)); break;
                case HashType.SHA384: strRet = EncodeWithHash(strPlain, (secretKey == null ? new HMACSHA384() : new HMACSHA384(secretKeyBytes))); break;  // new SHA384Managed()); break;
                case HashType.SHA512: strRet = EncodeWithHash(strPlain, (secretKey == null ? new HMACSHA512() : new HMACSHA512(secretKeyBytes))); break;  // new SHA512Managed()); break;
                default: strRet = "Invalid HashType"; break;
            }
            return strRet;
        } /* GetHash */

        public static bool CheckHash(string strOriginal, string strHash, HashType hshType, string secretKey=null)
        {
            string strOrigHash = GetHash(strOriginal, hshType, secretKey);
            return (strOrigHash == strHash);
        } /* CheckHash */

        
        public static string EncodeWithHash(string strPlain, HashAlgorithm hashAlgo)
        {
            UnicodeEncoding UE = new UnicodeEncoding();
            byte[] hashValue, messageBytes = UE.GetBytes(strPlain);
            //SHA1Managed SHhash = new SHA1Managed();                     // create an object that will hash our text
            // Calculate the hash and convert it to a hexadecimal string
            string strHex = "";
            hashValue = hashAlgo.ComputeHash(messageBytes);
            //hashValue = SHhash.ComputeHash(messageBytes);
            foreach (byte b in hashValue)
            {
                strHex += String.Format("{0:x2}", b);
            }
            return strHex;
        } /* EncodeWithHash */

        /*RSACryptoServiceProvider RSASign = new RSACryptoServiceProvider();
        //StreamReader sr = File.OpenText("PublicPrivate.txt");
        //string myKey = sr.ReadToEnd();
        //sr.Close();
        RSASign.SignData(apiSecret);
        byte[] signature = RSASign.SignData(arr, new SHA384());
        string head = "<!>Signature</!>";
        byte[] headBytes = Encoding.Default.GetBytes(head);
        byte[] arrayToSend = new byte[headBytes.Length + signature.Length];
        arrayToSend = headBytes.Concat(signature).ToArray();
        UserSock.Send(arrayToSend);*/

        public static string Zip(string value)
        {
            //Transform string into byte[]  
            byte[] byteArray = new byte[value.Length];
            int indexBA = 0;
            foreach (char item in value.ToCharArray())
            {
                byteArray[indexBA++] = (byte)item;
            }

            //Prepare for compress
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            System.IO.Compression.GZipStream sw = new System.IO.Compression.GZipStream(ms, System.IO.Compression.CompressionMode.Compress);

            //Compress
            sw.Write(byteArray, 0, byteArray.Length);
            //Close, DO NOT FLUSH cause bytes will go missing...
            sw.Close();

            //Transform byte[] zip data to string
            byteArray = ms.ToArray();
            System.Text.StringBuilder sB = new System.Text.StringBuilder(byteArray.Length);
            foreach (byte item in byteArray)
            {
                sB.Append((char)item);
            }
            ms.Close();
            sw.Dispose();
            ms.Dispose();
            return sB.ToString();
        }

        public static string UnZip(string value)
        {
            //Transform string into byte[]
            byte[] byteArray = new byte[value.Length];
            int indexBA = 0;
            foreach (char item in value.ToCharArray())
            {
                byteArray[indexBA++] = (byte)item;
            }

            //Prepare for decompress
            System.IO.MemoryStream ms = new System.IO.MemoryStream(byteArray);
            System.IO.Compression.GZipStream sr = new System.IO.Compression.GZipStream(ms,
                System.IO.Compression.CompressionMode.Decompress);

            //Reset variable to collect uncompressed result
            byteArray = new byte[byteArray.Length];

            //Decompress
            int rByte = sr.Read(byteArray, 0, byteArray.Length);

            //Transform byte[] unzip data to string
            System.Text.StringBuilder sB = new System.Text.StringBuilder(rByte);
            //Read the number of bytes GZipStream red and do not a for each bytes in
            //resultByteArray;
            for (int i = 0; i < rByte; i++)
            {
                sB.Append((char)byteArray[i]);
            }
            sr.Close();
            ms.Close();
            sr.Dispose();
            ms.Dispose();
            return sB.ToString();
        }



    } // end of class





} // end of namespace
