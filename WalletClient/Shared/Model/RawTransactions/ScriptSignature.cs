using Newtonsoft.Json;

namespace WalletClient.Shared.Model.RawTransactions
{
    public class ScriptSignature
    {
        [JsonProperty("asm")]
        public string Asm { get; set; }

        [JsonProperty("hex")]
        public string Hex { get; set; }
    }
}
