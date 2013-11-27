using Newtonsoft.Json;

namespace WalletClient.Shared.Model.RawTransactions
{
    public class Input
    {
        [JsonProperty("txid")]
        public string TransactionId { get; set; }

        [JsonProperty("vout")]
        public int Vout;

        [JsonProperty("scriptSig")]
        public ScriptSignature ScriptSignature { get; set; }

        [JsonProperty("sequence")]
        public long Sequence;
    }
}
