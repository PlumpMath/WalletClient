using Newtonsoft.Json;

namespace WalletClient.BlockChainInfo.Model
{
    public class WalletInfo
    {
        [JsonProperty("balance")]
        public decimal Balance { get; set; }

        [JsonProperty("errors")]
        public string Errors { get; set; }

        [JsonProperty("paytxfee")]
        public decimal Fee { get; set; }

        [JsonProperty("connected")]
        public int Connections { get; set; }

        [JsonProperty("proxy")]
        public string Proxy { get; set; }

        [JsonProperty("testnet")]
        public bool TestNet { get; set; }

        [JsonProperty("difficulty")]
        public decimal Difficulty { get; set; }

        [JsonProperty("blocks")]
        public int Blocks { get; set; }
    }
}
