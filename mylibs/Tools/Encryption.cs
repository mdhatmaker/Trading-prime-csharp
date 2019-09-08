using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Json;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using static Tools.G;

namespace Tools
{
    public class ExchangeApiKey
    {
        public string exchange { get; set; }
        public string key { get; set; }
        public string secret { get; set; }
    }

    public class Encryption
    {
        //  Call this function to remove the key from memory after use for security
        [DllImport("KERNEL32.DLL", EntryPoint = "RtlZeroMemory")]
        public static extern bool ZeroMemory(IntPtr Destination, int Length);

        //private readonly IConfigurationService _configService;    // this used to be passed to constructor to allow us to Get keys

        //private const string _vector = "cipherVector";
        //private const string _key = "cipherKey";
        private string _cipherVector;
        private string _cipherKey;

        //public Encryption(string vector="somereallycooliv", string key="0123456789abcdef")
        public Encryption(string vector, string key)
        {
            if (vector.Length != 16 || key.Length != 16)
            {
                throw new ArgumentException("Encryption::ctor -> Both vector and key params must have 16-char length");
            }
            _cipherVector = vector;
            _cipherKey = key;
        }

        /// <summary>
        /// Creates a random salt that will be used to encrypt your file. This method is required on FileEncrypt.
        /// </summary>
        /// <returns></returns>
        public static byte[] GenerateRandomSalt()
        {
            byte[] data = new byte[32];

            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                for (int i = 0; i < 10; i++)
                {
                    // Fille the buffer with the generated data
                    rng.GetBytes(data);
                }
            }

            return data;
        }

        /// <summary>
        /// Encrypts a file from its path and a plain password.
        /// </summary>
        /// <param name="inputFile"></param>
        /// <param name="password"></param>
        public string FileEncrypt(string inputFile)
        {
            //http://stackoverflow.com/questions/27645527/aes-encryption-on-large-files

            //generate random salt
            byte[] salt = GenerateRandomSalt();

            //create output file name
            string outputFilename = inputFile + ".aes";
            FileStream fsCrypt = new FileStream(outputFilename, FileMode.Create);

            //convert password string to byte arrray
            byte[] passwordBytes = System.Text.Encoding.UTF8.GetBytes(_cipherKey);

            //Set Rijndael symmetric encryption algorithm
            RijndaelManaged AES = new RijndaelManaged();
            AES.KeySize = 256;
            AES.BlockSize = 128;
            AES.Padding = PaddingMode.PKCS7;

            //http://stackoverflow.com/questions/2659214/why-do-i-need-to-use-the-rfc2898derivebytes-class-in-net-instead-of-directly
            //"What it does is repeatedly hash the user password along with the salt." High iteration counts.
            var key = new Rfc2898DeriveBytes(passwordBytes, salt, 50000);
            AES.Key = key.GetBytes(AES.KeySize / 8);
            AES.IV = key.GetBytes(AES.BlockSize / 8);

            //Cipher modes: http://security.stackexchange.com/questions/52665/which-is-the-best-cipher-mode-and-padding-mode-for-aes-encryption
            AES.Mode = CipherMode.CFB;

            // write salt to the begining of the output file, so in this case can be random every time
            fsCrypt.Write(salt, 0, salt.Length);

            CryptoStream cs = new CryptoStream(fsCrypt, AES.CreateEncryptor(), CryptoStreamMode.Write);

            FileStream fsIn = new FileStream(inputFile, FileMode.Open);

            //create a buffer (1mb) so only this amount will allocate in the memory and not the whole file
            byte[] buffer = new byte[1048576];
            int read;

            try
            {
                while ((read = fsIn.Read(buffer, 0, buffer.Length)) > 0)
                {
                    //Application.DoEvents(); // -> for responsive GUI, using Task will be better!
                    Thread.Sleep(100);
                    cs.Write(buffer, 0, read);
                }

                // Close up
                fsIn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                cs.Close();
                fsCrypt.Close();
            }

            return outputFilename;
        }

        /// <summary>
        /// Decrypts an encrypted file with the FileEncrypt method through its path and the plain password.
        /// </summary>
        /// <param name="inputFile"></param>
        /// <param name="outputFile"></param>
        /// <param name="password"></param>
        public void FileDecrypt(string inputFile, string outputFile)
        {
            byte[] passwordBytes = System.Text.Encoding.UTF8.GetBytes(_cipherKey);
            byte[] salt = new byte[32];

            FileStream fsCrypt = new FileStream(inputFile, FileMode.Open);
            fsCrypt.Read(salt, 0, salt.Length);

            RijndaelManaged AES = new RijndaelManaged();
            AES.KeySize = 256;
            AES.BlockSize = 128;
            var key = new Rfc2898DeriveBytes(passwordBytes, salt, 50000);
            AES.Key = key.GetBytes(AES.KeySize / 8);
            AES.IV = key.GetBytes(AES.BlockSize / 8);
            AES.Padding = PaddingMode.PKCS7;
            AES.Mode = CipherMode.CFB;

            CryptoStream cs = new CryptoStream(fsCrypt, AES.CreateDecryptor(), CryptoStreamMode.Read);

            FileStream fsOut = new FileStream(outputFile, FileMode.Create);

            int read;
            byte[] buffer = new byte[1048576];

            try
            {
                while ((read = cs.Read(buffer, 0, buffer.Length)) > 0)
                {
                    Thread.Sleep(100);
                    //Application.DoEvents();
                    fsOut.Write(buffer, 0, read);
                }
            }
            catch (CryptographicException ex_CryptographicException)
            {
                Console.WriteLine("CryptographicException error: " + ex_CryptographicException.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            try
            {
                cs.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error by closing CryptoStream: " + ex.Message);
            }
            finally
            {
                fsOut.Close();
                fsCrypt.Close();
            }
        }


        public string EncryptString(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return "";
            }
            try
            {

                var key = Encoding.UTF8.GetBytes(_cipherKey);  // _configService.Get(_key));
                byte[] IV = Encoding.ASCII.GetBytes(_cipherVector);    // _configService.Get(_vector));

                using (var aesAlg = Aes.Create())
                {
                    using (var encryptor = aesAlg.CreateEncryptor(key, IV))
                    {
                        using (var msEncrypt = new MemoryStream())
                        {
                            using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                            using (var swEncrypt = new StreamWriter(csEncrypt))
                            {
                                swEncrypt.Write(text);
                            }

                            var decryptedContent = msEncrypt.ToArray();

                            var result = new byte[IV.Length + decryptedContent.Length];

                            Buffer.BlockCopy(IV, 0, result, 0, IV.Length);
                            Buffer.BlockCopy(decryptedContent, 0, result, IV.Length, decryptedContent.Length);

                            return Convert.ToBase64String(result);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                dout("Unable to encrypt string: {0}\n{1}", text, e);
                throw e;
            }
        }

        public string DecryptString(string cipherText)
        {
            if (string.IsNullOrEmpty(cipherText))
            {
                return "";
            }
            try
            {
                var fullCipher = Convert.FromBase64String(cipherText);

                byte[] IV = Encoding.ASCII.GetBytes(_cipherKey);    // _configService.Get(_vector));
                var cipher = new byte[fullCipher.Length - IV.Length];

                Buffer.BlockCopy(fullCipher, 0, IV, 0, IV.Length);
                Buffer.BlockCopy(fullCipher, IV.Length, cipher, 0, fullCipher.Length - IV.Length);
                var key = Encoding.UTF8.GetBytes(_cipherKey);   // _configService.Get(_key));

                using (var aesAlg = Aes.Create())
                {
                    using (var decryptor = aesAlg.CreateDecryptor(key, IV))
                    {
                        string result;
                        using (var msDecrypt = new MemoryStream(cipher))
                        {
                            using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                            {
                                using (var srDecrypt = new StreamReader(csDecrypt))
                                {
                                    result = srDecrypt.ReadToEnd();
                                }
                            }
                        }

                        return result;
                    }
                }
            }
            catch (Exception e)
            {
                dout("Unable to decrypt string: {0}\n{1}", cipherText, e);
                throw e;
            }
        }

        public static string GetHashSha256(string text, string secretKey)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(text);
            SHA256Managed hashstring = new SHA256Managed();

            byte[] hash = hashstring.ComputeHash(bytes);
            string hashString = string.Empty;
            foreach (byte x in hash)
            {
                hashString += String.Format("{0:x2}", x);
            }
            return hashString;
        }

        private static string CreateHMACSignature(string key, string totalParams)
        {
            var messageBytes = Encoding.UTF8.GetBytes(totalParams);
            var keyBytes = Encoding.UTF8.GetBytes(key);
            var hash = new HMACSHA256(keyBytes);
            var computedHash = hash.ComputeHash(messageBytes);
            return BitConverter.ToString(computedHash).Replace("-", "").ToLower();
        }

        public static string DecryptTextFile(string pathname, string password, string salt = "this is my salt.  There are many like it, but this one is mine.")
        {
            string result = null;
            var encryption = new EncryptionHelper(password, salt);
            FileInfo file = new FileInfo(pathname);
            using (FileStream fs = file.OpenRead())
            using (MemoryStream ms = new MemoryStream())
            {
                encryption.Decrypt(fs, ms);
                var bytes = ms.ToArray();
                result = Encoding.ASCII.GetString(bytes);
                //var sr = new StreamReader(ms);
                //result = sr.ReadToEnd();
                //var bytes = Encoding.Unicode.GetBytes(sr.ReadToEnd());
                //new string()
                //BinaryFormatter bf = new BinaryFormatter();
                //targetType target = bf.Deserialize(outMs) as targetType;
            }
            return result;
        }

        public static bool EncryptTextFile(string textToEncrypt, string pathname, string password, string salt = "this is my salt.  There are many like it, but this one is mine.")
        {
            try
            {
                var encryption = new EncryptionHelper(password, salt);
                FileInfo file = new FileInfo(pathname);
                //var bytes = Encoding.Unicode.GetBytes(textToEncrypt);
                var bytes = Encoding.ASCII.GetBytes(textToEncrypt);
                //var bytes = Encoding.UTF8.GetBytes(textToEncrypt);
                using (FileStream fs = file.OpenWrite())
                using (MemoryStream ms = new MemoryStream(bytes))
                {
                    encryption.Encrypt(ms, fs);
                    
                    //BinaryFormatter bf = new BinaryFormatter();
                    //targetType target = bf.Deserialize(outMs) as targetType;
                }
                return true;
            }
            catch (Exception ex)
            {
                ErrorMessage("Encryption::EncryptTextFile => ERROR: {0}", ex.Message);
                return false;
            }       
        }

    } // end of class Encryption

    
    public class EncryptionHelper
    {
        static SymmetricAlgorithm encryption;
        //static string password;
        //static string salt;

        public EncryptionHelper(string password, string salt = "this is my salt.  There are many like it, but this one is mine.")
        {
            encryption = new RijndaelManaged();
            Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(password, Encoding.ASCII.GetBytes(salt));

            encryption.Key = key.GetBytes(encryption.KeySize / 8);
            encryption.IV = key.GetBytes(encryption.BlockSize / 8);
            encryption.Padding = PaddingMode.PKCS7;
        }

        public void Encrypt(Stream inStream, Stream OutStream)
        {
            ICryptoTransform encryptor = encryption.CreateEncryptor();
            inStream.Position = 0;
            CryptoStream encryptStream = new CryptoStream(OutStream, encryptor, CryptoStreamMode.Write);
            inStream.CopyTo(encryptStream);
            encryptStream.FlushFinalBlock();
        }

        public void Decrypt(Stream inStream, Stream OutStream)
        {
            ICryptoTransform encryptor = encryption.CreateDecryptor();
            inStream.Position = 0;
            CryptoStream encryptStream = new CryptoStream(inStream, encryptor, CryptoStreamMode.Read);
            encryptStream.CopyTo(OutStream);
            OutStream.Position = 0;
        }

    } // end of class EncryptionHelper


} // end of namespace
