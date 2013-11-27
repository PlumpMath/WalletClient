using Newtonsoft.Json;

namespace WalletClient.Bitcoind.Model
{
    public class WalletInfo
    {
        [JsonProperty("version")]
        public int Version { get; set; }

        [JsonProperty("protocolversion")]
        public int ProtocolVersion { get; set; }

        [JsonProperty("walletversion")]
        public int WalletVersion { get; set; }

        [JsonProperty("balance")]
        public decimal Balance { get; set; }

        [JsonProperty("blocks")]
        public int Blocks { get; set; }

        [JsonProperty("timeoffset")]
        public int TimeOffset { get; set; }

        [JsonProperty("connections")]
        public int Connections { get; set; }

        [JsonProperty("proxy")]
        public string Proxy { get; set; }

        [JsonProperty("difficulty")]
        public decimal Difficulty { get; set; }

        [JsonProperty("testnet")]
        public bool TestNet { get; set; }

        [JsonProperty("keypoololdest")]
        public int KeyPoolOldest { get; set; }

        [JsonProperty("keypoolsize")]
        public int KeyPoolSize { get; set; }

        [JsonProperty("paytxfee")]
        public decimal Fee { get; set; }

        [JsonProperty("errors")]
        public string Errors { get; set; }
            
    }
}
