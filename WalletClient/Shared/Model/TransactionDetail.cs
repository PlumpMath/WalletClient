using System;
using Newtonsoft.Json;

namespace WalletClient.Shared.Model
{
    public class TransactionDetail
    {
        [JsonProperty("fee")]
        public double Fee { get; set; }

        [JsonProperty("amount")]
        public double Amount { get; set; }

        [JsonProperty("blockindex")]
        public string BlockIndex { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }

        [JsonProperty("confirmations")]
        public int Confirmations { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("txid")]
        public string TransactionId { get; set; }

        [JsonProperty("block")]
        public long Block { get; set; }

        [JsonProperty("blockhash")]
        public string BlockHash { get; set; }

        [JsonProperty("account")]
        public string Account { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("time")]
        public DateTime Time { get; set; }
    }
}
