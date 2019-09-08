using System;
using System.IO;

namespace CryptoBank
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("\n*** WELCOME TO CRYPTO BANK ***\n");

            #region ---------- COMMAND LINE ARGUMENTS -----------------------------------------------------------------
            // *** RUN CRYPTOBANK "MODULE" BASED ON COMMAND LINE ARGUMENT ("1", "2", "3", ...)
            if (args.Length > 0)
            {
                if (args[0] == "1")
                {
                    var b = new Bank();
                    if (args.Length > 1 && args[1].ToUpper() == "EMAIL")
                        b.PrintPrimaryBalances(emailStatement: true);
                    else
                        b.PrintPrimaryBalances();
                }
                else if (args[0] == "2")
                {
                    var b = new Bank();
                    if (args.Length < 2)
                        b.PrintAllBalances();
                    else
                        b.PrintBalance(args[1]);
                }
                return;
            }
            else
            {
                Console.WriteLine("usage: dotnet CryptoBank.dll <#module>");
                Console.WriteLine("   ex: dotnet CryptoBank.dll 1              (display main balances and generate daily statement file)");
                Console.WriteLine("   ex: dotnet CryptoBank.dll 1 EMAIL        (display main balances and email statement to recipients)");
                Console.WriteLine("   ex: dotnet CryptoBank.dll 2              (display all balances)");
                Console.WriteLine("   ex: dotnet CryptoBank.dll 2 <EXCHANGE>   (display <EXCHANGE> balances)");
                Console.WriteLine("   ex: dotnet CryptoBank.dll 2 BINANCE      (display BINANCE balances)");
                Console.WriteLine();
            }
            #endregion ------------------------------------------------------------------------------------------------

            // *** THIS CODE WILL RUN IF NO COMMAND-LINE ARGUMENTS ARE PASSED - USE FOR TESTING ***
            var bank = new Bank();
            bank.PrintPrimaryBalances(emailStatement: false);
            return;

            //bal.Test();
            //bal.PrintAllBalances();
            bank.PrintPrimaryBalances();
            
			Console.WriteLine("Press ENTER to exit...");
			Console.ReadLine();
        }
    }
} // end of namespace
