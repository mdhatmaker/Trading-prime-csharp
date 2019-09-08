namespace Rokolab.BitstampClient.Models
{
    public class OrderStatusResponse
    {
        public string status { get; set; }
        public Transaction[] transactions { get; set; }
    }

    public class Transaction
    {
        public string fee { get; set; }
        public string price { get; set; }
        public string datetime { get; set; }
        public string usd { get; set; }
        public string btc { get; set; }
        public string tid { get; set; }
        public string type { get; set; }
    }
}