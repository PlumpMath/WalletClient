using System;
using Newtonsoft.Json;

namespace WalletClient.Shared.Model.RawTransactions
{
    public class UnspentTransaction
    {
        [JsonProperty("txid")]
        public string TransactionId { get; set; }

        [JsonProperty("vout")]
        public int Vout { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("scriptPubKey")]
        public string ScriptPublicKey { get; set; }

        [JsonProperty("amount")]
        public decimal Amount { get; set; }

        [JsonProperty("confirmations")]
        public UInt32 Confirmations { get; set; }
    }
}
