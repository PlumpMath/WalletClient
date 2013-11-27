using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace WalletClient.Shared.Model.RawTransactions
{
    public class RawTransaction
    {
        [JsonProperty("txid")]
        public string TransactionId { get; set; }

        [JsonProperty("version")]
        public Int32 Version { get; set; }

        [JsonProperty("locktime")]
        public Int32 LockTime { get; set; }

        [JsonProperty("vin")]
        public List<Input> Inputs { get; set; }

        [JsonProperty("vout")]
        public List<Output> Outputs { get; set; }
                  
        public string ToJsonString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
