using System;
using CryptoTools.Cryptography;

namespace Command
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                if (args[0].ToLower() == "encrypt")
                {
                    if (args.Length == 3)
                    {
                        Cryptography.EncryptFile(args[1], args[2]);
                        return;
                    }
                }
            }

            Console.WriteLine("usage: dotnet command.dll <command> <parameters>");
            Console.WriteLine();
            Console.WriteLine("       dotnet command.dll encrypt <file> <8-char-password>");
            Console.WriteLine();
            return;
        }
    }
} // end of namespace
