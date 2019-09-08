using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HitBtcApi.Model
{
    public class Ticker
    {
        /// <summary>
        /// Last price
        /// </summary>
        public string last { get; set; }

        /// <summary>
        /// Highest buy order
        /// </summary>
        public string bid { get; set; }

        /// <summary>
        /// Lowest sell order
        /// </summary>
        public string ask { get; set; }

        /// <summary>
        /// Highest trade price per last 24h + last incomplete minute
        /// </summary>
        public string high { get; set; }

        /// <summary>
        /// Lowest trade price per last 24h + last incomplete minute
        /// </summary>
        public string low { get; set; }

        /// <summary>
        /// Volume per last 24h + last incomplete minute
        /// </summary>
        public string volume { get; set; }

        /// <summary>
        /// Price in which instrument open
        /// </summary>
        public string open { get; set; }

        /// <summary>
        /// Volume in second currency per last 24h + last incomplete minute
        /// </summary>
        public string volume_quote { get; set; }

        /// <summary>
        /// Server time in UNIX timestamp format
        /// </summary>
        public long timestamp { get; set; }


        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"last:{last}");
            sb.AppendLine($"bid:{bid}");
            sb.AppendLine($"ask:{ask}");
            sb.AppendLine($"high:{high}");
            sb.AppendLine($"low:{low}");
            sb.AppendLine($"volume:{volume}");
            sb.AppendLine($"open:{open}");
            sb.AppendLine($"volume_quote:{volume_quote}");
            sb.AppendLine($"timestamp:{timestamp}");

            return sb.ToString();
        }
    }
}
