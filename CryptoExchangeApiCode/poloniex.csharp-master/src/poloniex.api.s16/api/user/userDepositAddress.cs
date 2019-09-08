using Newtonsoft.Json;

namespace XCT.BaseLib.API.Poloniex.User
{
    public interface IUserDepositAddress
    {
        bool IsGenerationSuccessful { get; }

        string Address { get; }
    }

    public class UserDepositAddress : IUserDepositAddress
    {
        [JsonProperty("success")]
        private byte IsGenerationSuccessfulInternal
        {
            set { IsGenerationSuccessful = value == 1; }
        }
        public bool IsGenerationSuccessful { get; private set; }

        [JsonProperty("response")]
        public string Address { get; private set; }
    }
}