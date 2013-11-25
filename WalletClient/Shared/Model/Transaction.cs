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

        [JsonProperty("details")]
        List<TransactionDetail> TransactionDetails { get; set; }
        
        public Transaction()
        {
            TransactionDetails = new List<TransactionDetail>();
        }
    }
}
