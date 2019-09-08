using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HitBtcApi.Model
{
    public class Error
    {
        public string message { get; set; }
        public int statusCode { get; set; }
        public string body { get; set; }
        public override string ToString()
        {
            return $"{statusCode} - {message} - {body}";
        }
    }
}
