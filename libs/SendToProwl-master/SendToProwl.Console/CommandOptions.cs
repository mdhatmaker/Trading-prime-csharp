//-----------------------------------------------------------------------
// <copyright file="CommandOptions.cs" company="Andrew Beaton">
//     Copyright (c) Andrew Beaton. All rights reserved. 
// </copyright>
//-----------------------------------------------------------------------
namespace SendToProwl
{
    using System.Text;
    using CommandLine;

    /// <summary>
    /// Configuration of the command line options.
    /// </summary>
    public class CommandOptions
    {
        [Option('k', "key", HelpText = "The API key.")]
        public string ApiKey { get; set; }

        [Option('a', "application", HelpText = "The application name.")]
        public string Application { get; set; }
        
        [Option('e', "event", HelpText = "The event name.")]
        public string Event { get; set; }
        
        [Option('d', "Description", HelpText = "The notification description.")]
        public string Description { get; set; }

        [Option('u', "url", HelpText = "The Url to display.")]
        public string Url { get; set; }
        
        [Option('p', "priority", HelpText = "The notification priority.")]
        public int Priority { get; set; }

        /// <summary>
        /// Gets the command line help text.
        /// </summary>
        /// <returns>The command line help text.</returns>
        [HelpOption]
        public string GetUsage()
        {
            var usage = new StringBuilder();
            usage.AppendLine("SendToProwl");
            usage.AppendLine("Sends a notification message to Prowl.");
            usage.AppendLine(string.Empty);
            usage.AppendLine("Usage:");
            usage.AppendLine("SendToProwl.exe -k API Key -a Application Name -e Event Name -d Description [-u Url] [-p Priority]");
            usage.AppendLine(string.Empty);
            usage.AppendLine("-k -key             The Prowl API key.");
            usage.AppendLine("-a -application     The application name");
            usage.AppendLine("-e -event           The event name.");
            usage.AppendLine("-d -description     The notification description.");
            usage.AppendLine("-u -url             The Url to display. [Optional]");
            usage.AppendLine("-p -priority        The notification priority. [Optional. Default 0]");

            usage.AppendLine(string.Empty);
            usage.AppendLine("Examples:");
            usage.AppendLine("SendToProwl.exe -k xxxxx -a SendToProwl -e Testing -d Description");

            return usage.ToString();
        }
    }
}
