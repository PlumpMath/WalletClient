using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace WalletClient.Shared.Model.RawTransactions
{
    public class RawTransactionInfo : RawTransaction
    {
        [JsonProperty("hex")]
        public string Hex { get; set; }

        [JsonProperty("blockhash")]
        public string BlockHash { get; set; }

        [JsonProperty("confirmations")]
        public int Confirmations { get; set; }

        [JsonProperty("time")]
        public DateTime Time { get; set; }

        [JsonProperty("blocktime")]
        public DateTime BlockTime { get; set; }        
    }
}
