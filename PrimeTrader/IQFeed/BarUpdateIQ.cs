using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools;

namespace IQFeed
{
    public class BarUpdateIQ : BarUpdate
    {
        public BarUpdateIQ(string sData)
        {
            //sData = "STREAMING,BH,@ESZ17,2017-10-18 01:00:00,2557.75,2558.00,2557.50,2557.50,20501,1684,0,\r\n"
            string[] data = sData.Split(',');
            this.Symbol = data[2];
            this.BarTime = DateTime.Parse(data[3]);
            this.Open = double.Parse(data[4]);
            this.High = double.Parse(data[5]);
            this.Low = double.Parse(data[6]);
            this.Close = double.Parse(data[7]);
            this.TotalVolume = int.Parse(data[8]);
            this.BarVolume = int.Parse(data[9]);
        }
    } // end of class
} // end of namespace