using System.Collections.Generic;
using System.Linq;

namespace Info.Blockchain.API.Client
{
    public class QueryString
    {
        private readonly IDictionary<string, string> _queryString;

        public QueryString()
        {
            _queryString = new Dictionary<string, string>();
        }

        public void Add(string key, string value)
        {
            if (_queryString.ContainsKey(key))
            {
                throw new ClientApiException($"Query string already has a value for {key}");
            }
            _queryString[key] = value;
        }

        public int Count => _queryString.Count;

        public void AddOrUpdate(string key, string value)
        {
            _queryString[key] = value;
        }

        public override string ToString()
        {
            return "?" + string.Join("&", _queryString.Select(kv => $"{kv.Key}={kv.Value}"));
        }
    }
}