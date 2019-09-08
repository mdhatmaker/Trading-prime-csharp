using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using CryptoTools.CryptoFile;

namespace CryptoTools
{
    public class Credentials
    {
        public static string CredentialsFile => Path.Combine(GFile.GetString("apis.path.txt"), "system.apis.enc");   // read ".enc" file from application path
        // TODO: MUST take this out of code!!!!
        public static string CredentialsPassword => @"mywookie";

        public int Count { get { return m_creds.Count; } }

        private SortedDictionary<string, CredentialEntry> m_creds = new SortedDictionary<string, CredentialEntry>();

        public void Add(string name, CredentialEntry credentialEntry)
        {
            m_creds.Add(name, credentialEntry);
        }

        public void Add(string name, string key, string secret)
        {
            m_creds.Add(name, new CredentialEntry(key, secret));
        }

        public void Add(string name, string key, string secret, string passphrase)
        {
            m_creds.Add(name, new CredentialEntry(key, secret, passphrase));
        }

        public void Add(string name, JArray keySecretArray)
        {
            var list = keySecretArray.ToObject<List<string>>();
            this.Add(name, list[0], list[1]);
        }

        public void Add(string[] arr)
        {
            if (arr.Length != 4) return;                // must have name,apikey,apisecret,passphrase (or other 4-item array)
            this.Add(arr[0], arr[1], arr[2], arr[3]);
        }

        public CredentialEntry this[string name]
        {
			get { return m_creds.ContainsKey(name) ? m_creds[name] : null; }
        }

        public static Credentials LoadEncryptedJson(string credentialsEncryptedFile, string encryptionPassword)
        {
            var text = Cryptography.Cryptography.GetDecryptedText(credentialsEncryptedFile, encryptionPassword);
            JObject jo = JsonConvert.DeserializeObject(text) as JObject;

            var creds = new Credentials();
            foreach (var jp in jo)
            {
                string name = jp.Key;
                JArray value = jp.Value as JArray;
                //Console.WriteLine("Name: {0}\nValue: {1}\n", name, value);
                creds.Add(name, value); //value.ToObject<List<string>>();
                //Console.WriteLine(creds.Count);
            }
            return creds;
        }

		public static Credentials LoadEncryptedCsv(string credentialsEncryptedFile, string encryptionPassword)
		{
			var creds = new Credentials();

			var text = Cryptography.Cryptography.GetDecryptedText(credentialsEncryptedFile, encryptionPassword);

			if (text != null)
			{
				var lines = text.Split('\n');
				foreach (var l in lines)
				{
					var line = l.Trim('\n').Trim('\r').Trim();      // remove carriage return, line feed, whitespace
					if (string.IsNullOrWhiteSpace(line)) continue;  // skip blank lines
					var split = line.Split(',');
					if (split.Length != 4)                          // skip lines that do not have 4 data items
					{
						// display error message?
						continue;
					}
					creds.Add(split);
					//Console.WriteLine(creds.Count);
				}
			}
            return creds;
        }
    } // end of class Credentials

    public class CredentialEntry
    {
        public string Key { get; private set; }
        public string Secret { get; private set; }
        public string Passphrase { get; private set; }

        public CredentialEntry(string key, string secret)
        {
            this.Key = key;
            this.Secret = secret;
            this.Passphrase = "";
        }

        public CredentialEntry(string key, string secret, string passphrase)
        {
            this.Key = key;
            this.Secret = secret;
            this.Passphrase = passphrase;
        }
    } // end of class CredentialEntry

} // end of namespace

