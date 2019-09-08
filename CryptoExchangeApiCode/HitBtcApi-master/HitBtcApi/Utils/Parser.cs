using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HitBtcApi.API
{
    public static class Parser
    {
        public static T Parse<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
            //return JsonConvert.DeserializeObject<dynamic>(json);
        }
}
}
