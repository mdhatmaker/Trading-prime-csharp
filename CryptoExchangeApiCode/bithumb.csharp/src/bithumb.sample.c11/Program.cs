using System;
using System.Text;

namespace Bithumb.Sample.Core
{
    class Program
    {
       /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            var provider = CodePagesEncodingProvider.Instance;
            Encoding.RegisterProvider(provider);

            Bithumb.Start(1);

            while (Console.ReadLine() != "quit")
                Console.WriteLine("Enter 'quit' to stop the services and end the process...");
        }
    }
}