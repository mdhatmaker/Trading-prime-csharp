using System;

namespace SendToProwl
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("*** PROWL API TEST ***");

            var notification = new ProwlNotification()
            {
                ApiKey = "75fcd3a230ac062616e14c15ed5f79bdb5d0d199",
                Application = "Send To Prowl",
                Event = "Test Notification",
                Description = "Just testing notifications",
                Priority = 1
            };

            var client = new ProwlClient();
            client.SendNotification(notification);
        }
    }
}
