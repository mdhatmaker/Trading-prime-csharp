using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static CryptoTools.Global;

namespace CryptoTools.Cryptography
{
    public static class Cryptography
    {
        //private static HMACSHA256 m_hmac;

        // Use the following to encrypt the API file:
        public static void SimpleEncrypt(string file = "/Users/michael/Documents/cliff_apis.csv", string password = "12345678")
        {
            Cryptography.EncryptFile(file, password);    // Credentials.CredentialsPassword);
        } 


        public static void TestSignFile(string[] Fileargs)
        {
            string dataFile, signedFile;
            if (Fileargs.Length < 2)            // If no file names are specified, create them.
            {
                dataFile = @"text.txt";
                signedFile = "signedFile.enc";

                if (!File.Exists(dataFile))     // Create a sample data file if none exists.
                {
                    using (StreamWriter sw = File.CreateText(dataFile)) { sw.WriteLine("Here is a message to sign"); }
                }
            }
            else
            {
                dataFile = Fileargs[0];
                signedFile = Fileargs[1];
            }
            try
            {
                // Create a random key using a random number generator. This would be the secret key shared by sender and receiver.
                byte[] secretkey = new Byte[64];
                using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())   // RNGCryptoServiceProvider is an implementation of a random number generator.
                {
                    rng.GetBytes(secretkey);                            // The array is now filled with cryptographically strong random bytes.
                    SignFile(secretkey, dataFile, signedFile);          // Use the secret key to sign the message file.
                    VerifyFile(secretkey, signedFile);                  // Verify the signed file
                }
            }
            catch (IOException e) { Console.WriteLine("Error: File not found", e); }
        }  // end Test method

        // Computes a keyed hash for a source file and creates a target file with the keyed hash prepended to the contents of the source file.
        public static void SignFile(byte[] key, String sourceFile, String destFile)
        {
            // Initialize the keyed hash object.
            using (HMACSHA256 hmac = new HMACSHA256(key))
            {
                using (FileStream inStream = new FileStream(sourceFile, FileMode.Open))
                {
                    using (FileStream outStream = new FileStream(destFile, FileMode.Create))
                    {
                        // Compute the hash of the input file.
                        byte[] hashValue = hmac.ComputeHash(inStream);
                        // Reset inStream to the beginning of the file.
                        inStream.Position = 0;
                        // Write the computed hash value to the output file.
                        outStream.Write(hashValue, 0, hashValue.Length);
                        // Copy the contents of the sourceFile to the destFile.
                        int bytesRead;
                        // read 1K at a time
                        byte[] buffer = new byte[1024];
                        do
                        {
                            // Read from the wrapping CryptoStream.
                            bytesRead = inStream.Read(buffer, 0, 1024);
                            outStream.Write(buffer, 0, bytesRead);
                        } while (bytesRead > 0);
                    }
                }
            }
            return;
        } // end SignFile

        // Compares the key in the source file with a new key created for the data portion of the file.
        // If the keys compare the data has not been tampered with.
        public static bool VerifyFile(byte[] key, String sourceFile)
        {
            bool err = false;
            // Initialize the keyed hash object. 
            using (HMACSHA256 hmac = new HMACSHA256(key))
            {
                // Create an array to hold the keyed hash value read from the file.
                byte[] storedHash = new byte[hmac.HashSize / 8];
                // Create a FileStream for the source file.
                using (FileStream inStream = new FileStream(sourceFile, FileMode.Open))
                {
                    // Read in the storedHash.
                    inStream.Read(storedHash, 0, storedHash.Length);
                    // Compute the hash of the remaining contents of the file.
                    // The stream is properly positioned at the beginning of the content, 
                    // immediately after the stored hash value.
                    byte[] computedHash = hmac.ComputeHash(inStream);
                    // compare the computed hash with the stored value

                    for (int i = 0; i < storedHash.Length; i++)
                    {
                        if (computedHash[i] != storedHash[i])
                        {
                            err = true;
                        }
                    }
                }
            }
            if (err)
            {
                Console.WriteLine("Hash values differ! Signed file has been tampered with!");
                return false;
            }
            else
            {
                Console.WriteLine("Hash values agree -- no tampering occurred.");
                return true;
            }

        } //end VerifyFile

        // Calculate the SHA-256 hash for all files in a directory.
        public static void CalculateHashForDirectory(string[] args)
        {
            string directory = "";
            if (args.Length < 1)
            {
                Console.WriteLine("No arguments passed (expected: 1 argument for directory).");
                return;
                /*FolderBrowserDialog fbd = new FolderBrowserDialog();
                DialogResult dr = fbd.ShowDialog();
                if (dr == DialogResult.OK)
                    directory = fbd.SelectedPath;
                else
                {
                    Console.WriteLine("No directory selected.");
                    return;
                }*/
            }
            else
            {
                directory = args[0];
            }

            try
            {
                // Create a DirectoryInfo object representing the specified directory.
                DirectoryInfo dir = new DirectoryInfo(directory);
                // Get the FileInfo objects for every file in the directory.
                FileInfo[] files = dir.GetFiles();
                // Initialize a SHA256 hash object.
                SHA256 mySHA256 = SHA256Managed.Create();

                byte[] hashValue;
                // Compute and print the hash values for each file in directory.
                foreach (FileInfo fInfo in files)
                {
                    // Create a fileStream for the file.
                    FileStream fileStream = fInfo.Open(FileMode.Open);
                    // Be sure it's positioned to the beginning of the stream.
                    fileStream.Position = 0;
                    // Compute the hash of the fileStream.
                    hashValue = mySHA256.ComputeHash(fileStream);
                    // Write the name of the file to the Console.
                    Console.Write(fInfo.Name + ": ");
                    // Write the hash value to the Console.                    
                    PrintByteArray(hashValue);
                    // Close the file.
                    fileStream.Close();
                }
                return;
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine("Error: The directory specified could not be found.");
            }
            catch (IOException)
            {
                Console.WriteLine("Error: A file in the directory could not be accessed.");
            }
        } // end CalculateHashForDirectory


        #region ---------- ENCRYPT / DECRYPT WITH 8-CHAR PASSWORD -----------------------------------------------------
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
        #endregion ----------------------------------------------------------------------------------------------------

    } // end of class Cryptography




} // end of namespace
