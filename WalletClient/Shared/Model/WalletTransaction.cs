using System;
using Newtonsoft.Json;

namespace WalletClient.Shared.Model
{
    public class WalletTransaction
    {
        [JsonProperty("account")]
        public string Account { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("category")]
        public TransactionCategory Category { get; set; }

        [JsonProperty("amount")]
        public decimal Amount { get; set; }

        [JsonProperty("fee")]
        public decimal Fee { get; set; }

        [JsonProperty("confirmations")]
        public int? Confirmations { get; set; }

        [JsonProperty("blockhash")]
        public string BlockHash { get; set; }

        [JsonProperty("blockindex")]
        public int? BlockIndex { get; set; }

        [JsonProperty("txid")]
        public string TransactionId { get; set; }

        [JsonProperty("time")]
        public DateTime Time { get; set; }

        [JsonProperty("timereceived")]
        public DateTime? TimeReceived { get; set; }

        [JsonProperty("comment")]
        public string Comment { get; set; }

        [JsonProperty("otheraccount")]
        public string OtherAccount { get; set; }
    }
}
