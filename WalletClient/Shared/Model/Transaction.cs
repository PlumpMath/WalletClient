using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WalletClient.Shared.Model
{
    public class Transaction
    {
        [JsonProperty("amount")]
        public decimal Amount { get; set; }
        
        [JsonProperty("fee")]
        public decimal Fee { get; set; }

        [JsonProperty("blockindex")]
        public string BlockIndex { get; set; }

        [JsonProperty("confirmations")]
        public int Confirmations { get; set; }

        [JsonProperty("txid")]
        public string TransactionId { get; set; }

        [JsonProperty("block")]
        public long Block { get; set; }

        [JsonProperty("blockhash")]
        public string BlockHash { get; set; }

        [JsonProperty("blocktime")]
        public DateTime BlockTime { get; set; }

        [JsonProperty("details")]
        public List<TransactionDetail> TransactionDetails { get; set; }
        
        [JsonProperty("time")]
        public DateTime Time { get; set; }
        
        [JsonProperty("timereceived")]
        public DateTime TimeReceived { get; set; }
        
        public Transaction()
        {
            TransactionDetails = new List<TransactionDetail>();
        }
    }
}
