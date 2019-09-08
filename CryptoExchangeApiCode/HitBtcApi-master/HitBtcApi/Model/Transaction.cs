using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HitBtcApi.Model
{
    public class Transaction
    {
        public string id { get; set; }
        public string type { get; set; }
        public string status { get; set; }
        public int created { get; set; }
        public int finished { get; set; }
        public double amount_from { get; set; }
        public string currency_code_from { get; set; }
        public double amount_to { get; set; }
        public string currency_code_to { get; set; }
        public object destination_data { get; set; }
        public int commission_percent { get; set; }
        public string bitcoin_address { get; set; }
        public string bitcoin_return_address { get; set; }
        public string external_data { get; set; }
    }

    public class TransactionObject
    {
        public Transaction transaction { get; set; }
    }

    public class TransactionList
    {
        public List<Transaction> transactions { get; set; }
    }

    public class PayoutTransaction
    {
        public string transaction { get; set; }
    }
}
