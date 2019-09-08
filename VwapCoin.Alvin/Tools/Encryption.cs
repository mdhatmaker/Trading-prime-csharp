using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Tools
{
    public static class Cryptography
    {
        // Version of EncryptFile that infers output filename by adding ".enc" to input filename
        public static void EncryptFile(string inputFile, string encryptionPassword)
        {
            EncryptFile(inputFile, inputFile + ".enc", encryptionPassword);
        }

        ///<summary>
        /// Encrypt a file using Rijndael algorithm.
        ///</summary>
        ///<param name="inputFile"></param>
        ///<param name="outputFile"></param>
        ///<param name="encryptionPassword"></param>
        public static void EncryptFile(string inputFile, string outputFile, string encryptionPassword)
        {
            try
            {
                if (encryptionPassword.Length != 8)
                {
                    Console.WriteLine("Encryption requires 8-char password.");
                    return;
                }
                //string encryptionPassword = @"myKey123"; // Your Key Here
                UnicodeEncoding UE = new UnicodeEncoding();
                byte[] key = UE.GetBytes(encryptionPassword);

                string cryptFile = outputFile;
                FileStream fsCrypt = new FileStream(cryptFile, FileMode.Create);

                RijndaelManaged RMCrypto = new RijndaelManaged();

                CryptoStream cs = new CryptoStream(fsCrypt,
                    RMCrypto.CreateEncryptor(key, key),
                    CryptoStreamMode.Write);

                FileStream fsIn = new FileStream(inputFile, FileMode.Open);

                int data;
                while ((data = fsIn.ReadByte()) != -1)
                    cs.WriteByte((byte)data);


                fsIn.Close();
                cs.Close();
                fsCrypt.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Encryption failed: {0}", ex.Message);
            }
        }

        ///<summary>
        /// Decrypt a file using Rijndael algorithm.
        ///</summary>
        ///<param name="inputFile"></param>
        ///<param name="outputFile"></param>
        ///<param name="encryptionPassword"></param>
        public static void DecryptFile(string inputFile, string outputFile, string encryptionPassword)
        {
            try
            {
                //string encryptionPassword = @"myKey123"; // Your Key Here

                UnicodeEncoding UE = new UnicodeEncoding();
                byte[] key = UE.GetBytes(encryptionPassword);

                FileStream fsCrypt = new FileStream(inputFile, FileMode.Open);

                RijndaelManaged RMCrypto = new RijndaelManaged();

                CryptoStream cs = new CryptoStream(fsCrypt,
                    RMCrypto.CreateDecryptor(key, key),
                    CryptoStreamMode.Read);

                FileStream fsOut = new FileStream(outputFile, FileMode.Create);

                int data;
                while ((data = cs.ReadByte()) != -1)
                    fsOut.WriteByte((byte)data);

                fsOut.Close();
                cs.Close();
                fsCrypt.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Decryption failed: {0}", ex.Message);
            }
        }

        ///<summary>
        /// Decrypt a file to a string using Rijndael algorithm.
        ///</summary>
        ///<param name="inputFile"></param>
        ///<param name="encryptionPassword"></param>
        public static string GetDecryptedText(string inputFile, string encryptionPassword)
        {
            string result = null;
            try
            {
                //string encryptionPassword = @"myKey123"; // Your Key Here

                UnicodeEncoding UE = new UnicodeEncoding();
                byte[] key = UE.GetBytes(encryptionPassword);

                FileStream fsCrypt = new FileStream(inputFile, FileMode.Open);

                RijndaelManaged RMCrypto = new RijndaelManaged();

                CryptoStream cs = new CryptoStream(fsCrypt,
                    RMCrypto.CreateDecryptor(key, key),
                    CryptoStreamMode.Read);

                MemoryStream msOut = new MemoryStream();

                int data;
                while ((data = cs.ReadByte()) != -1)
                    msOut.WriteByte((byte)data);

                result = System.Text.Encoding.Default.GetString(msOut.ToArray());

                msOut.Close();
                cs.Close();
                fsCrypt.Close();

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Decryption failed: {0}", ex.Message);
                return null;
            }
        }

    } // end of class Cryptography


    #region ---------- Credentials --------------------------------------------------------------------
    public class Credentials
    {
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
            get { return m_creds[name]; }
        }

        public static Credentials LoadDecryptedJson(string credentialsEncryptedFile, string encryptionPassword)
        {
            var text = Cryptography.GetDecryptedText(credentialsEncryptedFile, encryptionPassword);
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

        public static Credentials LoadDecryptedCsv(string credentialsEncryptedFile, string encryptionPassword)
        {
            var text = Cryptography.GetDecryptedText(credentialsEncryptedFile, encryptionPassword);

            var creds = new Credentials();
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
    #endregion ----------------------------------------------------------------------------------------


} // end of namespace
