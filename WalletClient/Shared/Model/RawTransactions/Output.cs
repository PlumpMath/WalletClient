using Newtonsoft.Json;

namespace WalletClient.Shared.Model.RawTransactions
{
    public class Output
    {
        [JsonProperty("value")]
        public decimal Value { get; set; }

        [JsonProperty("n")]
        public int N { get; set; }

        [JsonProperty("scriptPubKey")]
        public ScriptPublicKey ScriptPublicKey { get; set; }
    }
}
