using System.Collections.Generic;
using Newtonsoft.Json;

namespace WalletClient.Shared.Model.RawTransactions
{
    public class ScriptPublicKey
    {
        [JsonProperty("asm")]
        public string Asm { get; set; }

        [JsonProperty("hex")]
        public string Hex {get;set;}

        [JsonProperty("reqSigs")]
        public int RequiredSignatures { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("addresses")]
        public List<string> Addresses { get; set; }
    }
}
