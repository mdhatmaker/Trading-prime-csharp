C# Kraken API

.Net implementation for Kraken's REST API. Handles rate-limiting (1 request/5s) using Jack Leitch's RateGate.

The solution includes two projects:

- KrakenClient.csproj which exposes all the API methods found here: https://www.kraken.com/help/api  

- KrakenClientConsole.csproj for testing.

Example usage

Make sure to add the following keys in the appSettings section of the calling assembly's  configuration file:

<appSettings>
      <add key="KrakenBaseAddress" value="https://api.kraken.com"/>
      <add key="KrakenApiVersion" value="0"/>
      <add key="KrakenKey" value=" YOUR KRAKEN KEY"/>
      <add key="KrakenSecret" value="YOUR KRAKEN SECRET"/>
</appSettings>

Include the KrakenClient assembly as well as Jayrock (avaible online with nuget package manager) for handling Json objects.

using KrakenClient;
using Jayrock.Json;
using Jayrock.Json.Conversion;

Call public method:

Query public ticker info for XBT/EUR pair:

var ticker = client.GetTicker(new List<string> { "XXBTZEUR" });

Example output:

{"error":[],"result":{"XXBTZEUR":{"a":["656.00000","1"],"b":["655.60000","1"],"c":["656.00000","0.13612804"],"v":["341.76377806","691.62335355"],"p":["625.69390","615.10113"],"t":[1175,2027],"l":["605.65000","595.25000"],"h":["656.00000","656.00000"],"o":"609.70664"}}}


Call private method:

var tradesHistory = client.GetTradesHistory(string.Empty);

Example output:

{"error":[],"result":{"trades":{},"count":"0"}}

More examples are provided in KrakenClientConsole.csproj




