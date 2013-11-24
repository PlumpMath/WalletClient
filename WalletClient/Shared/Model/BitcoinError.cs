using Newtonsoft.Json;

namespace WalletClient.Shared.Model
{
    public class BitcoinError
    {
        [JsonProperty("code")]
        public int ErrorId { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
