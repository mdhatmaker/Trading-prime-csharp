using System;
using System.Collections.Generic;
using System.Text;

namespace Tools.Websockets
{
    // We still need to deserialize our message into a TradeResponse.
    // Let's add a Helper class with a generic "ToEntity" function that takes
    // a string as parameter and returns an entity:
    internal static class Helper
    {
        internal static T ToEntity<T>(string data)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(data);
        }
    }
} // end of namespace
