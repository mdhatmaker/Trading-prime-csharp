namespace Huobi.Rest.CSharp.Demo.Model
{
    public class HBResponse<T> where T : new()
    {
        public string Status { get; set; }
        public T Data { get; set; }
    }
}
