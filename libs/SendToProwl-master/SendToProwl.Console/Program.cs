//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="Andrew Beaton">
//     Copyright (c) Andrew Beaton. All rights reserved. 
// </copyright>
//-----------------------------------------------------------------------
namespace SendToProwl
{
    using System;

    public class Program
    {
        private static void Main(string[] args)
        {
            var notification = new ProwlNotification();
            var options = new CommandOptions();

            if (CommandLine.Parser.Default.ParseArguments(args, options))
            {
                notification.ApiKey = options.ApiKey;
                notification.Application = options.Application;
                notification.Event = options.Event;
                notification.Description = options.Description;
                notification.Priority = options.Priority;
            }

            var client = new ProwlClient();

            try
            {
                client.SendNotification(notification);
                Console.WriteLine("Notification successfully sent.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.ReadKey();
        }
    }
}